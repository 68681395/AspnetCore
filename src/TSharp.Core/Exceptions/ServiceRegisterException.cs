using System;

namespace TSharp.Core.Exceptions
{
    /// <summary>
    /// 服务注册异常
    /// <para>2010/12/24</para>
    /// 	<para>THINKPADT61</para>
    /// 	<para>tangjingbo</para>
    /// </summary>
    public class ServiceRegisterException : CoreException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceRegisterException"/> class.
		/// </summary>
		/// <param name="message">The arg0.</param>
		public ServiceRegisterException(String message)
			: base(message)
		{
		}
	}
}