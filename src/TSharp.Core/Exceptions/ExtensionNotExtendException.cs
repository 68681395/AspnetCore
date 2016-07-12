using System;

namespace TSharp.Core.Exceptions
{
	/// <summary>
	/// 扩展标记和扩展管理类没有从指定基类继承
	/// <para>2011/3/4</para>
	/// 	<para>TANGJINGBO</para>
	/// 	<author>tangjingbo</author>
	/// </summary>
	public class ExtensionNotExtendException : CoreException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionNotExtendException"/> class.
		/// </summary>
		/// <param name="msg">The MSG.</param>
		public ExtensionNotExtendException(string msg) : base(msg)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExtensionNotExtendException"/> class.
		/// </summary>
		/// <param name="msg">The MSG.</param>
		/// <param name="ex">The ex.</param>
		public ExtensionNotExtendException(string msg, Exception ex)
			: base(msg, ex)
		{
		}
	}
}