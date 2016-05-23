using System.Collections.Generic;
using System.Reflection;


namespace TSharp.Core.Osgi.Internal
{
    /// <summary>
    /// 服务注册管理
    /// <para>2010/12/24</para>
    /// 	<para>THINKPADT61</para>
    /// 	<para>tangjingbo</para>
    /// </summary>
    public class ServiceManager : ExtensionPoint<RegServiceAttribute>
    {
        private static readonly ServiceInfoList rootMap = new ServiceInfoList();

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<RegServiceAttribute> RootMap
        {
            get
            {
                return rootMap;
            }
        }

        private static void RegisterService(RegServiceAttribute attribute)
        {
            rootMap.Add(attribute);
        }

        /// <summary>
        /// 注销后期绑定关系，如果该类型被多次注册，将移除优先级最低的
        /// </summary>
        private static void UnRegisterService(RegServiceAttribute attribute)
        {
            rootMap.Remove(attribute);
        }

        #region IExtensionPoint 成员

        /// <summary>
        /// Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void Register(Assembly assembly, RegServiceAttribute attribute)
        {
            RegisterService(attribute);
        }

        /// <summary>
        /// Uns the register.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void UnRegister(Assembly assembly, RegServiceAttribute attribute)
        {
            UnRegisterService(attribute);
        }

        #endregion
    }
}