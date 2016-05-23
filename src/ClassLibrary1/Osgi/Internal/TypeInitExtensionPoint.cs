using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Common.Logging;

namespace TSharp.Core.Osgi.Internal
{
    /// <summary>
    /// 类类型初始化扩展点
    /// <para>2010/12/24</para>
    /// 	<para>THINKPADT61</para>
    /// 	<para>tangjingbo</para>
    /// </summary>
    internal class TypeInitExtensionPoint : ExtensionPoint<RegTypeInitAttribute>
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        private readonly List<Type> _types = new List<Type>(50);

        /// <summary>
        /// Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void Register(Assembly assembly, RegTypeInitAttribute attribute)
        {
            _types.Add(attribute.InitType);
        }

        /// <summary>
        /// Uns the register.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void UnRegister(Assembly assembly, RegTypeInitAttribute attribute)
        {
        }

        /// <summary>
        /// 加载时执行
        /// </summary>
        protected internal override void OnLoad()
        {
            foreach (Type initType in _types)
            {
                try
                {
                    RuntimeHelpers.RunClassConstructor(initType.TypeHandle);
                    /*
                     * 如果initType是抽象的，cctor将不会被执行
                     * 如果initType是继承自某类，如果没有自己的cctor或者静态成员访问父类，则父类的cctor不会执行                    * 
                     */
                }
                catch (TypeInitializationException ex)
                {
                    Debug.WriteLine(ex.ToString());
                    log.Error(ex);
                }
            }
            _types.Clear();
            base.OnLoad();
        }
    }
}