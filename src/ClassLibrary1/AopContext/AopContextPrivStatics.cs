using System;
using TSharp.Core.Osgi;

namespace TSharp.Core
{
    static partial class AopContext
    {
        #region 常量

        /// <summary>
        /// 系统容器key
        /// </summary>
        public readonly static string CacheKeyContainerSystem = "CacheKey:Container:System".GetAppSetting("CONTAINER_SYSTEM");

        /// <summary>
        /// 会话容器KEY
        /// </summary>
        public readonly static string CacheKeyContainerSession = "CacheKey:Container:Session".GetAppSetting("CONTAINER_SESSION");

        /// <summary>
        /// 请求容器key
        /// </summary>
        public readonly static string CacheKeyContainerRequset = "CacheKey:Container:Requset".GetAppSetting("CONTAINER_REQUEST");

        /// <summary>
        /// 线程容器Key
        /// </summary>
        public readonly static string CacheKeyContainerThread = "CacheKey:Container:Thread".GetAppSetting("CONTAINER_THREAD");


        /// <summary>
        /// 系统级别容器名称
        /// </summary>
        public readonly static string ContainerNameSystem = "ContainerName:System".GetAppSetting("system");

        /// <summary>
        /// 会话级别容器名称
        /// </summary>
        public readonly static string ContainerNameSession = "ContainerName:Session".GetAppSetting("session");

        /// <summary>
        /// 请求级别容器名称
        /// </summary>
        public readonly static string ContainerNameRequest = "ContainerName:Request".GetAppSetting("request");

        /// <summary>
        /// 请求级别容器名称
        /// </summary>
        public readonly static string ContainerNameThread = "ContainerName:Thread".GetAppSetting("thread");

        #endregion

        private static Func<IContext> _contextFactory =
           () => WebContext.Instance;
        private static IServiceSituation CreateServiceSituation()
        {
            return LazyLoading.New<IServiceSituation>();
        }
    }
}