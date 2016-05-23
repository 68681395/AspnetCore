using System;

namespace TSharp.Core.Osgi
{
	/// <summary>
	/// 初始化调用静态构造函数
	/// <para>2011/1/25</para>
	/// 	<para>TANGJINGBO</para>
	/// 	<author>tangjingbo</author>
	/// </summary>
	public class RegTypeInitAttribute : RegExtensionAttribute
	{
		/// <summary>  
		/// 如果initType是抽象的，cctor将不会被执行
		/// 如果initType是继承自某类，如果没有自己的cctor或者静态成员访问父类，则父类的cctor不会执行 
		/// </summary>
		/// <param name="initType">Type of the init.</param>
		public RegTypeInitAttribute(Type initType)
		{
			InitType = initType;
		}

		internal Type InitType { get; private set; }
	}
}