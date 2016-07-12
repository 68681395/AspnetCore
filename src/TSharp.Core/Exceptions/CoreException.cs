using System;

namespace TSharp.Core.Exceptions
{
    /// <summary>
    /// 异常编码接口
    /// </summary>
    /// <remarks>
    /// 代码合并后，将接口移动到独立接口
    /// </remarks>
    public interface IErrorCode
    {
        /// <summary>
        /// 获取异常编码
        /// </summary>
        /// <value>The error code.</value>
        string ErrorCode { get; }
    }
    /// <summary>
    /// Core异常基类
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangjingbo at 2009-7-29 15:02
    /// </remarks>
    public class CoreException : Exception, IErrorCode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CoreException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CoreException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 获取异常编码
        /// </summary>
        /// <value>The error code.</value>
        public virtual string ErrorCode
        {
            get { return this.GetType().Name; }
        }
    }
}