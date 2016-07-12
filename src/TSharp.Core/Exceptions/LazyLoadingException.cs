using System;
using TSharp.Core.Exceptions;

namespace TSharp.Core.Osgi
{
	/// <summary>
	/// 晚加载异常
	/// </summary>
	/// <author>
	/// tangjingbo
	/// </author>
	///[Serializable]
	public class LazyLoadingException : CoreException
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyLoadingException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
		public LazyLoadingException(String msg)
			: base(msg)
		{
		}
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/>
        ///   </PermissionSet>
        public override string ToString()
        {
            return base.ToString();
        }
	}
}