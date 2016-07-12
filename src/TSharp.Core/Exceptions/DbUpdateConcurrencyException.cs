using System;

namespace TSharp.Core.Exceptions
{
    /// <summary>
    /// 修改数据库时，对数据库没有任何影响，可能数据库已经发生变化
    /// </summary>
    public class DbUpdateConcurrencyException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbUpdateConcurrencyException"/> class.
        /// </summary>
        /// <param name="message">The error.</param>
        /// <param name="concurrency">The concurrency.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public DbUpdateConcurrencyException(string message, Exception concurrency)
            : base(message, concurrency)
        {

        }
    }
}