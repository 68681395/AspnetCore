namespace TSharp.Core
{
    /// <summary>
    /// 服务定位器创建工厂，通过晚加载查找创建实例
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    public interface IServiceSituation
	{
		/// <summary>
		/// 请求级别服务定位器。winform首先Session子容器，然后返回；web中每次请求有不同实例，请求内共享。
		/// </summary>
		/// <returns></returns>
		IServiceLocator GetRequest();

		/// <summary>
		/// 会话级别服务定位器。先找会话服务，然后找系统服务
		/// </summary>
		/// <returns></returns>
		IServiceLocator GetSession();

		/// <summary>
		/// 线程级别服务定位器。查找线程级服务，然后再查找系统服务
		/// </summary>
		/// <returns></returns>
		IServiceLocator GetThread();

		/// <summary>
		/// 系统级别服务定位器.web程序存储在HttpAppliction中，应用程序域内多线程共享
		/// <para>by tangjingbo at 2009-11-4 14:37</para>
		/// </summary>
		/// <returns></returns>
		IServiceLocator GetRoot();
	}
}