using System;
using System.Linq.Expressions;
using TSharp.Core.Osgi;
namespace TSharp.Core
{
    /// <summary>
    /// AOP容器上下文
    /// <para>by tangjingbo at 2009-11-4 14:37</para>
    /// </summary>
    public static partial class AopContext
    {
        private static IServiceSituation _serviceSituation;

        #region 获取和重新创建上下文

        private static readonly object AsynSituation = new object();

        /// <summary>
        /// aop上下文
        /// </summary>
        public static IServiceSituation Services
        {
            get
            {
                if (_serviceSituation != null)
                    return _serviceSituation;
                lock (AsynSituation)
                {
                    if (_serviceSituation == null)
                        _serviceSituation = CreateServiceSituation();
                    return _serviceSituation;
                }
            }
        }

        /// <summary>
        /// 重新创建服务访问上下文，返回原有上下文
        /// </summary>
        /// <returns></returns>
        public static IServiceSituation ResetServiceSituation()
        {
            lock (AsynSituation)
            {
                IServiceSituation origin = _serviceSituation;
                _serviceSituation = CreateServiceSituation();
                return origin;
            }
        }

        #endregion

        /// <summary>
        /// 获取默认服务定位器
        /// </summary>
        /// <value>The default.</value>
        public static IServiceLocator GetDefaultLactor()
        {
            return Services.GetRequest();
        }

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <returns></returns>
        public static IContext GetContext()
        {
            return _contextFactory();
        }

      

        /// <summary>
        /// Sets the HTTP context factory.
        /// </summary>
        /// <param name="fac">The fac.</param>
        public static void SetHttpContextFactory(Expression<Func<IContext>> fac)
        {
            _contextFactory = fac.Compile();
        }
        internal static readonly string KeyRequestReadWriteUnitOfWork = "Key:Request:ReadWriteUnitOfWork".GetAppSetting("Key:Request:ReadWriteUnitOfWork");

     
        internal static readonly string KeyRequestReadonlyUnitOfWork = "Key:Request:ReadonlyUnitOfWork".GetAppSetting("Key:Request:ReadonlyUnitOfWork");

    
    }
}