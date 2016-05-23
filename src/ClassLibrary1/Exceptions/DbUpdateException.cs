using System;

namespace TSharp.Core.Exceptions
{
    /// <summary>
    /// 数据库更新异常，例如删除已被引用的数据、更新外键为不存在的值等。
    /// </summary>
    public class DbUpdateException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbUpdateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public DbUpdateException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}