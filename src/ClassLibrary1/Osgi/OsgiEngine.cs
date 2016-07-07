#if !NET35
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Common.Logging;
using TSharp.Core.Osgi.Internal;
using System.Collections.Concurrent;
using System.Runtime.Loader;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// OSGI配置引擎
    /// <para>2011/3/4</para>
    /// 	<para>TANGJINGBO</para>
    /// 	<author>tangjingbo</author>
    /// </summary>
    public sealed class OsgiEngine : Disposable
    {
        private static readonly ILog Log;
        static OsgiEngine()
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
        public static OsgiEngine Current { get; private set; }

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

        private readonly ConcurrentDictionary<Type, ExtensionPoint> _extensionPoints;
        private readonly ConcurrentDictionary<string, MultiVersionAssembly> _assemblys;

        /// <summary>
        /// Inits the web engine.
        /// </summary>
        /// <returns>OsgiEngine.</returns>
        public static OsgiEngine InitWebEngine()
        {

            //AopContext.SetHttpContextFactory(() => WebContext.Instance);
            string basePath = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            return Init(basePath, Path.Combine(basePath, "bin"), "", true);
        }
        /// <summary>
        /// Inits the winform engine.
        /// </summary>
        /// <param name="pluginPath">The plugin path.</param>
        /// <returns>OsgiEngine.</returns>
        public static OsgiEngine InitWinformEngine(string pluginPath)
        {
            // AopContext.SetHttpContextFactory(() => WindowContext.Instance);
            string basePath = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            return Init(basePath, basePath, pluginPath, true);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static OsgiEngine InitWinformEngine()
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
        /// <returns>OsgiEngine.</returns>
        /// <exception cref="Exceptions.CoreException">Osgi引擎已经初始化，不能进行多次初始化！</exception>
        public static OsgiEngine Init(string rootPath, string runtimePath, string libPath,
                          bool disableExtAttrLoadException)
        {
            if (Current == null)
            {
                lock (locker)
                {
                    Current = new OsgiEngine(rootPath, runtimePath, libPath, disableExtAttrLoadException);
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
        private OsgiEngine(string rootPath, string runtimePath, string libPath,
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
            _extensionPoints = new ConcurrentDictionary<Type, ExtensionPoint>();
            _assemblys = new ConcurrentDictionary<string, MultiVersionAssembly>();
        }
        private void Init()
        {


            //InitAssembly(this.GetType().Assembly);
            InitAssembly(this.GetType().GetTypeInfo().Assembly);

            foreach (var assm in AppDomain.CurrentDomain.GetAssemblies())
                InitAssembly(assm);
            if (!String.IsNullOrEmpty(RuntimePath))
            {
                var dllFiles = Directory.GetFiles(RuntimePath, "*.dll");
                foreach (string dllFile in dllFiles)
                {
                    Assembly assembly = null;
                    try
                    {
                        assembly = LoadFrom(dllFile);
                    }
                    catch (BadImageFormatException)
                    {
                        //  Log.Error(string.Format(",已配置引擎无法加载DLL被自动忽略。‘{0}’", dllFile), e);
                        continue;
                    }
                    if (assembly != null) InitAssembly(assembly);
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
                        assembly = LoadFrom(dllFile);
                    }
                    catch (BadImageFormatException e)
                    {
                        Log.Error("配置引擎无法加载DLL：" + dllFile, e);
                        continue;
                    }
                    if (assembly != null) InitAssembly(assembly);
                }
            }
            var extensions = new HashSet<RegExtensionAttributeItem>();
            foreach (string key in _assemblys.Keys)
            {
                DetectExtensionPointAndRegExtensionAttr(_assemblys[key].LatestVersionAssembly, extensions);
            }
            foreach (RegExtensionAttributeItem extension in extensions)
            {
                ExtensionPoint point;
                if (_extensionPoints.TryGetValue(extension.ExtensionAttribute.GetType(), out point))
                    point.EngineAdd(extension);
                else
                    Log.Warn(string.Format("没有找到程序集'{0}’中扩展'{1}'没有对应的管理类", extension.Assembly.FullName,
                                           extension.ExtensionAttribute.GetType().FullName));
            }


            OsgiEventManager.Events.Foreach(x => x.Start(this));

            foreach (var point in _extensionPoints.Values)
                point.RegisterAll();

            foreach (var point in _extensionPoints.Values)
            {
                point.OnInit();
            }
            foreach (var point in _extensionPoints.Values)
            {
                point.OnLoad();
            }
            OsgiEventManager.Events.Foreach(x => x.StartCompleted(this));

        }

        private Assembly LoadFrom(string dllFile)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(dllFile);
        }

        private void InitAssembly(string assemblyQualifiedName)
        {
            var assm = Assembly.Load(new AssemblyName(assemblyQualifiedName));

            InitAssembly(assm);
        }
        private void InitAssembly(Assembly assembly)
        {
            var verAssembly = _assemblys.GetOrAdd(assembly.GetName().Name, _ => new MultiVersionAssembly());

            if (verAssembly.IsLatestVersion(assembly))
            {
                verAssembly.Add(assembly);
            }
        }


        private void DetectExtensionPointAndRegExtensionAttr(Assembly assembly, HashSet<RegExtensionAttributeItem> extensions)
        {
            try
            {
                var extensionPointAttrs = assembly.GetCustomAttributes<RegExtensionPointAttribute>();
                if (extensionPointAttrs != null)
                    foreach (RegExtensionPointAttribute extensionPointAttr in extensionPointAttrs)
                    {
                        _extensionPoints[extensionPointAttr.AttributeType] = extensionPointAttr.ExtensionPoint;
                    }
                var extensionAttrs = assembly.GetCustomAttributes<RegExtensionAttribute>();
                if (extensionAttrs != null)
                    foreach (RegExtensionAttribute extensionAttr in extensionAttrs)
                    {
                        var item = new RegExtensionAttributeItem(assembly, extensionAttr);
                        ExtensionPoint point;
                        if (_extensionPoints.TryGetValue(extensionAttr.GetType(), out point))
                            point.EngineAdd(item);
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

        /// <summary>
        /// 扩展项
        /// <para>2011/3/4</para>
        /// 	<para>TANGJINGBO</para>
        /// 	<author>tangjingbo</author>
        /// </summary>
        internal sealed class RegExtensionAttributeItem
        {
            /// <summary>
            /// 扩展项所属程序集
            /// </summary>
            /// <value>The assembly.</value>
            public Assembly Assembly { get; private set; }

            /// <summary>
            /// 获取扩展标记
            /// </summary>
            /// <value>The extension attribute.</value>
            public RegExtensionAttribute ExtensionAttribute { get; private set; }

            /// <summary>
            /// 扩展项构造
            /// </summary>
            /// <param name="assembly">The assembly.</param>
            /// <param name="extensionAttribute">The extension attribute.</param>
            public RegExtensionAttributeItem(Assembly assembly, RegExtensionAttribute extensionAttribute)
            {
                Assembly = assembly;
                ExtensionAttribute = extensionAttribute;
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
                        OsgiEventManager.Events.Foreach(x =>
                            {
                                try
                                {
                                    x.Stop(this);
                                }
                                catch (Exception ex)
                                {
                                    Log.Error(string.Format("Osgi:OsgiEvent执行Stop(OsgiEngine)时异常！类型:'{0}'", x.GetType().FullName), ex);
                                }
                            });

                        foreach (var point in _extensionPoints.Values)
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
                        foreach (var point in _extensionPoints.Values)
                            try
                            {
                                point.UnRegisterAll();
                            }
                            catch (Exception ex)
                            {
                                Log.Error(string.Format("Osgi:扩展点执行UnRegister()时异常！类型:'{0}'", point.GetType().FullName), ex);
                            }
                        OsgiEventManager.Events.Foreach(x =>
                        {
                            try
                            {
                                x.StopCompleted(this);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(string.Format("Osgi:OsgiEvent执行StopCompleted(OsgiEngine)时异常！类型:'{0}'", x.GetType().FullName), ex);
                            }
                        });
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