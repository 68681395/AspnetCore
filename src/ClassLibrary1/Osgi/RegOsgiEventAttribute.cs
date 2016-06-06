using System;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// Osgi事件注册
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author> 
    public sealed class RegOsgiEventAttribute : RegExtensionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegOsgiEventAttribute"/> class.
        /// </summary>
        /// <param name="evtType">Type of the evt.</param>
        public RegOsgiEventAttribute(Type evtType)
        {
            EventType = evtType;
        }

        internal Type EventType { get; private set; }
    }
}