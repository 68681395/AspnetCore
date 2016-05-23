
using TSharp.Core.Osgi.Internal;
using TSharp.Core.Osgi;


namespace TSharp.Core.Simple
{
    partial class SimpleServiceSituation
    {
        internal SimpleLocatorWrapper CreateRootContainer()
        {
            log.Debug("创建 Root");

            var container = new SimpleLocatorWrapper(Level.ROOT);

            Configure(ServiceManager.RootMap, container);

            return container;
        }

        private SimpleLocatorWrapper GetRootUnity()
        {
            return CreateWebRoot();
        }

#if !WebSvrLocator
        private static readonly object rootAsynHelper = new object();
        private static SimpleLocatorWrapper _root;
#endif

        private SimpleLocatorWrapper CreateWebRoot()
        {
#if WebSvrLocator
            var container = ctx.Application[CACHEKEY_SYSTEM_CONTAINER] as SimpleContainer;
            if (container != null)
                return container;
            ctx.Application.Lock();
            try
            {
                if ((container = ctx.Application[CACHEKEY_SYSTEM_CONTAINER] as SimpleContainer) == null)
                    ctx.Application[CACHEKEY_SYSTEM_CONTAINER] = container = CreateRootContainer();
            }
            finally
            {
                ctx.Application.UnLock();
            }
            return container;
#else
            if (_root != null)
                return _root;
            lock (rootAsynHelper)
            {
                if (_root != null)
                    return _root;
                _root = CreateRootContainer();
                return _root;
            }
#endif
        }
    }
}