using System;
using TSharp.Core.Exceptions;
using TSharp.Core.Osgi;

namespace TSharp.Core.Simple
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangj15 at 2012-5-4 13:18
    /// </remarks>
    public class ServiceLocatorException : CoreException
    {
        private Level l;
        private Type t;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorException"/> class.
        /// </summary>
        /// <param name="queryType">Type of the query.</param>
        /// <param name="level">The level.</param>
        /// <param name="innerException">The inner exception.</param>
        public ServiceLocatorException(Type queryType, Level level, Exception innerException)
            : base(string.Format("服务定位错误！Level:{0} ,Type:{1}", level, queryType.FullName), innerException)
        {
            t = queryType;
            l = level;
        }
    }
}