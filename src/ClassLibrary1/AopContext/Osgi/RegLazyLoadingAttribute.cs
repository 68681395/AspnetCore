using System;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// 注册晚加载
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    public class RegLazyLoadingAttribute : RegExtensionAttribute
    {
        /// <summary>
        /// 接口类型或抽象类
        /// </summary>
        public Type IntfType { get; private set; }

        /// <summary>
        /// 实现intfType的最终类型，必须有一个默认的无参构造类
        /// </summary>
        public Type ImplType { get; private set; }

        /// <summary>
        /// 优先级，总是使用优先级最高的实现类
        /// </summary>
        public LoadingPriority Priority { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegLazyLoadingAttribute"/> class.
        /// </summary>
        /// <param name="intfType">Type of the intf.</param>
        /// <param name="implType">Type of the impl.</param>
        public RegLazyLoadingAttribute(Type intfType, Type implType)
        {
            IntfType = intfType;
            ImplType = implType;
            Priority = LoadingPriority.Normal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegLazyLoadingAttribute"/> class.
        /// </summary>
        /// <param name="intfType">Type of the intf.</param>
        /// <param name="implType">Type of the impl.</param>
        /// <param name="priority">The priority.</param>
        public RegLazyLoadingAttribute(Type intfType, Type implType, LoadingPriority priority)
        {
            IntfType = intfType;
            ImplType = implType;
            Priority = priority;
        }
    }
}