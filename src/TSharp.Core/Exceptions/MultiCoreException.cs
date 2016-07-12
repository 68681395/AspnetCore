// ***********************************************************************
// Assembly         : TSharp.Core
// Author           : tangjingbo
// Created          : 10-10-2013
//
// Last Modified By : tangjingbo
// Last Modified On : 10-10-2013
// ***********************************************************************
// <copyright file="MultiCoreException.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Text;

namespace TSharp.Core.Exceptions
{
    /// <summary>
    /// Class MultiCoreException
    /// </summary>
    public class MultiCoreException : CoreException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCoreException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exs">The exs.</param>
        public MultiCoreException(string message, Exception[] exs)
            : base(message)
        {
            Errors = exs;
        }
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public Exception[] Errors
        {
            get;
            private set;
        } 
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
        ///   </PermissionSet>
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int index = 0, n = Errors.Length; index < n; index++)
            {
                var exception = Errors[index];
                sb.AppendFormat("第个{0}异常：", index);
                sb.AppendLine();
                sb.Append(exception);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}