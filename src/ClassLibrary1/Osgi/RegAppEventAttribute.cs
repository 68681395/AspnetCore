using System;
using System.Linq.Expressions;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// 注册程序事件
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangjingbo at 2009-8-4 12:37
    /// </remarks>
    public class RegAppEventAttribute : RegExtensionAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegAppEventAttribute"/> class.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        public RegAppEventAttribute(Type eventType)
        {
            EventType = eventType;
            New = Expression.Lambda<Func<IAppHandler>>(Expression.New(eventType)).Compile();
        }
        internal readonly Func<IAppHandler> New;
        internal Type EventType { get; private set; }
    }
}