using System;
using System.Collections.Generic;
using TSharp.Core.Osgi;

namespace TSharp.Core
{
    /// <summary>
    /// 服务定位器访问
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author> 
    public interface IServiceLocator : IDisposable
    {
        /// <summary>
        /// 获取服务定位器缓存级别
        /// </summary>
        Level Level { get; }

        /// <summary>
        /// 根据服务类型获取服务实例对象，该实例是缓存在服务注册级别上的
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        TService Get<TService>(params object[] existing);

        /// <summary>
        /// 根据服务类型获取所有服务实例对象
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        IEnumerable<TService> GetAll<TService>(params object[] existing);

        /// <summary>
        /// 根据服务类型和注册名获取服务实例对象
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="existing">The existing.</param>
        /// <returns>``0.</returns>
        TService Get<TService>(string name, params object[] existing);

        /// <summary>
        /// 根据服务类型获取服务实例对象，该实例是缓存在服务注册级别上的
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="existing">The existing.</param>
        /// <returns>System.Object.</returns>
        object Get(Type type, params object[] existing);

        /// <summary>
        /// 根据服务类型获取所有服务实例对象
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="existing">The existing.</param>
        /// <returns>IEnumerable{System.Object}.</returns>
        IEnumerable<object> GetAll(Type type, params object[] existing);

        /// <summary>
        /// 根据服务类型和注册名获取服务实例对象
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <param name="existing">The existing.</param>
        /// <returns>System.Object.</returns>
        object Get(Type type, string name, params object[] existing);

        /// <summary>
        /// 返回经过指定Type装配后对象
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="existing">The existing.</param>
        /// <returns>System.Object.</returns>
        object BuildUp(Type type, object instance, params object[] existing);
    }
}