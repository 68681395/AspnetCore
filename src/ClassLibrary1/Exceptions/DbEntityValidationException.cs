using System;

namespace TSharp.Core.Exceptions
{
    /// <summary>
    /// 更新数据库时，发生数据约束错误，无法更新数据库
    /// </summary>
    public class DbEntityValidationException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbEntityValidationException"/> class.
        /// </summary>
        /// <param name="message">The error.</param>
        /// <param name="dbEntityValidationException">The db entity validation exception.</param>
        public DbEntityValidationException(string message, Exception dbEntityValidationException)
            : base(message, dbEntityValidationException)
        {

        }

    
    }
}
