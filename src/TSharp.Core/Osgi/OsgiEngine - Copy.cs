#if !NET35
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Common.Logging;
using TSharp.Core.Osgi.Internal;
using System.Collections.Concurrent;
using System.Runtime.Loader;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// OSGI配置引擎
    /// <para>2011/3/4</para>
    /// 	<para>TANGJINGBO</para>
    /// 	<author>tangjingbo</author>
    /// </summary>
    public sealed class OsgiEngineByAssembly : Disposable
    {
        private static readonly ILog Log;

        static OsgiEngineByAssembly()
        {
            try
            {
                Log = LogManager.GetCurrentClassLogger();
            }
            catch { }
        }
        /// <summary>
        /// 获取当前引擎单例
        /// </summary>
        /// <value>The current.</value>
        public static OsgiEngineByAssembly Current { get; private set; }

        /// <summary>
        /// 根物理路径
        /// </summary>
        public string RootPath { private set; get; }

        /// <summary>
        /// 程序集库路径
        /// </summary>
        public string LibPath { get; private set; }

        /// <summary>
        /// Gets or sets the runtime path.
        /// </summary>
        /// <value>The runtime path.</value>
        public string RuntimePath { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether [disable ext attr load exception].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [disable ext attr load exception]; otherwise, <c>false</c>.
        /// </value>
        public bool DisableExtAttrLoadException { get; set; }

        private static readonly ConcurrentDictionary<Type, ExtensionPoint> _allExtensionPoints = new ConcurrentDictionary<Type, ExtensionPoint>();

        private static readonly ConcurrentDictionary<string, MultiVersionAssembly> _allAssemblys = new ConcurrentDictionary<string, MultiVersionAssembly>();

        /// <summary>
        /// Inits the web engine.
        /// </summary>
        /// <returns>OsgiEngineByAssembly.</returns>
        public static OsgiEngineByAssembly InitWebEngine()
        {
            var rootPath = PlatformServices.Default.Application.ApplicationBasePath;
            //AopContext.SetHttpContextFactory(() => WebContext.Instance);
            string basePath = Path.GetDirectoryName(rootPath);
            return Init(basePath, Path.Combine(basePath, "bin"), "", true);
        }
        /// <summary>
        /// Inits the winform engine.
        /// </summary>
        /// <param name="pluginPath">The plugin path.</param>
        /// <returns>OsgiEngineByAssembly.</returns>
        public static OsgiEngineByAssembly InitWinformEngine(string pluginPath)
        {

            var rootPath = PlatformServices.Default.Application.ApplicationBasePath;
            // AopContext.SetHttpContextFactory(() => WindowContext.Instance);
            string basePath = Path.GetDirectoryName(rootPath);
            return Init(basePath, basePath, pluginPath, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static OsgiEngineByAssembly InitWinformEngine()
        {
            return InitWinformEngine("plugin");
        }

        private static readonly int[] locker = new int[0];
        /// <summary>
        /// Inits OSGI 引擎
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="runtimePath">The runtime path.</param>
        /// <param name="libPath">The lib path.</param>
        /// <param name="disableExtAttrLoadException">if set to <c>true</c> [disable ext attr load exception].</param>
        /// <returns>OsgiEngineByAssembly.</returns>
        /// <exception cref="Exceptions.CoreException">Osgi引擎已经初始化，不能进行多次初始化！</exception>
        public static OsgiEngineByAssembly Init(string rootPath, string runtimePath, string libPath,
                          bool disableExtAttrLoadException)
        {
            if (Current == null)
            {
                lock (locker)
                {
                    Current = new OsgiEngineByAssembly(rootPath, runtimePath, libPath, disableExtAttrLoadException);
                    Current.Init();
                }
                return Current;
            }
            throw new Exceptions.CoreException("Osgi引擎已经初始化，不能进行多次初始化！");
        }
        /// <summary>
        /// OSGI 引擎
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="runtimePath">The runtime path.</param>
        /// <param name="libPath">程序集库路径</param>
        /// <param name="disableExtAttrLoadException">if set to <c>true</c> [disable ext attr load exception].</param>
        private OsgiEngineByAssembly(string rootPath, string runtimePath, string libPath,
                          bool disableExtAttrLoadException)
        {
            RootPath = rootPath;
            DisableExtAttrLoadException = disableExtAttrLoadException;
            RuntimePath = runtimePath;
            if (!string.IsNullOrWhiteSpace(libPath))
                if (Path.IsPathRooted(libPath))
                    LibPath = libPath;
                else
                {
                    LibPath = Path.Combine(runtimePath, libPath);
                }

        }

        //protected virtual AppDomain BuildChildDomain(AppDomain parentDomain)
        //{
        //    if (parentDomain == null) throw new System.ArgumentNullException("parentDomain");

        //    Evidence evidence = new Evidence(parentDomain.Evidence);
        //    AppDomainSetup setup = parentDomain.SetupInformation;
        //    return AppDomain.CreateDomain("DiscoveryRegion", evidence, setup);
        //}
        public IEnumerable<Assembly> GetAssemblies()
        {
            var entryAssembly = Assembly.GetEntryAssembly();
            yield return entryAssembly;

            foreach (var assemblyName in entryAssembly.GetReferencedAssemblies())
            {
                yield return Assembly.Load(assemblyName);
            }
        }
        private void Init()
        {
            LoadAssembly(this.GetType().GetTypeInfo().Assembly);

            foreach (var assm in GetAssemblies())
                LoadAssembly(assm);
            if (!String.IsNullOrEmpty(RuntimePath))
            {
                var dllFiles = Directory.GetFiles(RuntimePath, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    Assembly assembly = null;
                    try
                    {
                        assembly = LoadFromPath(dllFile);
                    }
                    catch (BadImageFormatException)
                    {
                        //  Log.Error(string.Format(",已配置引擎无法加载DLL被自动忽略。‘{0}’", dllFile), e);
                        continue;
                    }
                    if (assembly != null) LoadAssembly(assembly);
                }
            }
            if (!string.IsNullOrEmpty(LibPath) && Directory.Exists(LibPath))
            {
                var dllFiles = Directory.GetFiles(LibPath, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    Assembly assembly = null;
                    try
                    {
                        assembly = LoadFromPath(dllFile);
                    }
                    catch (BadImageFormatException e)
                    {
                        Log.Error("配置引擎无法加载DLL：" + dllFile, e);
                        continue;
                    }
                    if (assembly != null) LoadAssembly(assembly);
                }
            }


        }

        private void NewMethod(Assembly assembly)
        {
            var extensions = new HashSet<ExtensionItem>();
            foreach (string key in _allAssemblys.Keys)
            {
                DetectExtensionPointAndRegExtensionAttr(_allAssemblys[key].LatestVersionAssembly, extensions);
            }
            foreach (ExtensionItem extension in extensions)
            {
                ExtensionPoint point;
                if (_allExtensionPoints.TryGetValue(extension.ExtensionAttribute.GetType(), out point))
                    point.Add(extension);
                else
                    Log.Warn(string.Format("没有找到程序集'{0}’中扩展'{1}'没有对应的管理类", extension.Assembly.FullName,
                                           extension.ExtensionAttribute.GetType().FullName));
            }

            foreach (var point in _allExtensionPoints.Values)
                point.DoRegisterAll();

            foreach (var point in _allExtensionPoints.Values)
            {
                point.OnInit();
            }
            foreach (var point in _allExtensionPoints.Values)
            {
                point.OnLoad();
            }
        }

        private Assembly LoadFromPath(string assemblyPath)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
        }

        private void LoadAssembly(Assembly assembly)
        {
            var verAssembly = _allAssemblys.GetOrAdd(assembly.GetName().Name, _ => new MultiVersionAssembly());

            if (verAssembly.IsLatestVersion(assembly))
            {
                verAssembly.Add(assembly);
            }
        }


        private void DetectExtensionPointAndRegExtensionAttr(Assembly assembly, HashSet<ExtensionItem> extensions)
        {
            try
            {
                var extensionPointAttrs = assembly.GetCustomAttributes<ExtensionPointAttribute>();
                if (extensionPointAttrs != null)
                    foreach (ExtensionPointAttribute extensionPointAttr in extensionPointAttrs)
                    {
                        _allExtensionPoints[extensionPointAttr.AttributeType] = extensionPointAttr.ExtensionPoint;
                    }
                var extensionAttrs = assembly.GetCustomAttributes<ExtensionAttribute>();
                if (extensionAttrs != null)
                    foreach (ExtensionAttribute extensionAttr in extensionAttrs)
                    {
                        var item = new ExtensionItem(assembly, extensionAttr);
                        ExtensionPoint point;
                        if (_allExtensionPoints.TryGetValue(extensionAttr.GetType(), out point))
                            point.Add(item);
                        else if (extensions != null)
                            extensions.Add(item);
                        else
                            Log.Warn(string.Format("未找到{0}的管理类时，extensions参数不能为null。", extensionAttr.GetType().FullName),
                                     new ArgumentNullException("extensions"));
                    }
            }
            catch (Exception ex)
            {
                Log.Error("OSGI 引擎调用RegisterAssembly(Assembly assembly, HashSet<ExtensionAttributeItem> extensions)方法异常", ex);
                if (!DisableExtAttrLoadException)
                    throw ex;
            }
        }



        private bool disposed = false;
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    try
                    {


                        foreach (var point in _allExtensionPoints.Values)
                        {
                            try
                            {
                                point.UnLoad();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(string.Format("Osgi:扩展点执行UnLoad()时异常！类型:'{0}'", point.GetType().FullName), ex);
                            }

                        }
                        foreach (var point in _allExtensionPoints.Values)
                            try
                            {
                                point.DoUnRegisterAll();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(string.Format("Osgi:扩展点执行UnRegister()时异常！类型:'{0}'", point.GetType().FullName), ex);
                            }

                        OsgiEventManager.Clear();

                        Current = null;
                    }
                    catch
                    { }
                }
                // 这里释放所有非托管资源
            }
            disposed = true;
        }
    }
}

#endif