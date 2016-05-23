using System;
using System.Text;

namespace TSharp.Core.Osgi
{
	/// <summary>
	/// 服务注册定义
	/// <para>2011/2/25</para>
	/// 	<para>THINKPADT61</para>
	/// 	<para>tangjingbo</para>
	/// </summary>
	public class RegServiceAttribute : RegExtensionAttribute
	{
		private Level _ContainerLevel = Level.REQUSET;
		private bool isCached = true;
		private bool isDefault;

		/// <summary>
		/// 是否作为默认服务
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is default; otherwise, <c>false</c>.
		/// </value>
		public bool IsDefault
		{
			get { return isDefault; }
			set { isDefault = value; }
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="RegServiceAttribute"/> class.
		/// </summary>
		/// <param name="instanceType">服务类类型</param>
		public RegServiceAttribute(Type instanceType)
			: this(null, instanceType)
		{
		}

		/// <summary>
		/// 注册一个服务
		/// </summary>
		/// <param name="serviceType">服务类型</param>
		/// <param name="level">容器级别.</param>
		public RegServiceAttribute(Type serviceType, Level level)
			: this(null, serviceType)
		{
			this.Level = level;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RegServiceAttribute"/> class.
		/// </summary>
		/// <param name="intfType">服务接口类型</param>
		/// <param name="implType">服务实现类型</param>
		public RegServiceAttribute(Type intfType, Type implType)
		{
			IntfType = intfType;
			ImplType = implType;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="RegServiceAttribute" /> class.
        /// </summary>
        /// <param name="intfType">Type of the intf.</param>
        /// <param name="implType">Type of the impl.</param>
        /// <param name="level">The level.</param>
		public RegServiceAttribute(Type intfType, Type implType, Level level)
			: this(intfType, implType)
		{
			this.Level = level;
		}

		/// <summary>
		/// 注册到UnityContainer中的Name，可为null
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// 服务分组名成
		/// </summary>
		/// <value>
		/// The group.
		/// </value>
		public string Group { get; set; }

		/// <summary>
		/// 接口类型或抽象类
		/// </summary>
		public Type IntfType { get; private set; }

		/// <summary>
		/// 实现intfType的最终类型，必须有一个默认的无参构造类
		/// </summary>
		public Type ImplType { get; private set; }

		/// <summary>
		/// 服务驻留级别
		/// </summary>
		/// <value>The level.</value>
		public Level Level
		{
			get { return _ContainerLevel; }
			set { _ContainerLevel = value; }
		}

		/// <summary>
		/// 获取或设置一个值，指示实例是否缓存在容器
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is cached; otherwise, <c>false</c>.
		/// </value>
		public bool IsCached
		{
			get { return isCached; }
			set { isCached = value; }
		}

		/// <summary>
		/// 检查Level是否合法
		/// </summary>
		public bool Check(StringBuilder errors)
		{
			//TODO:检查Level是否合法
			return true;
		}
	}
}