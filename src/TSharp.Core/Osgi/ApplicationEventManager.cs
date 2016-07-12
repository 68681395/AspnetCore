using System;
using Common.Logging;
using System.Collections.Concurrent;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// 程序事件管理
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    internal class ApplicationEventManager : ExtensionPoint<RegAppEventAttribute>
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<RegAppEventAttribute, IAppHandler> HandlerTypes
            = new ConcurrentDictionary<RegAppEventAttribute, IAppHandler>();


        /// <summary>
        /// Registers the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void Register(System.Reflection.Assembly assembly, RegAppEventAttribute attribute)
        {
            HandlerTypes.TryAdd(attribute, null);
        }

        /// <summary>
        /// Uns the register.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="attribute">The attribute.</param>
        protected override void UnRegister(System.Reflection.Assembly assembly, RegAppEventAttribute attribute)
        {
            IAppHandler value;
            HandlerTypes.TryRemove(attribute, out value);
        }
        protected internal override void OnInit()
        {
            base.OnInit();
            foreach (var key in HandlerTypes.Keys)
            {
                try
                {
                    HandlerTypes[key] = key.New();
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Osgi:创建AppHandler时异常！类型:'{0}'", key.EventType.FullName), ex);
                }
            }
        }
        protected internal override void OnLoad()
        {
            base.OnLoad();

            var arg = new AppEventArgs()
            {
                Sender = this,
            };
            foreach (var i in HandlerTypes.Values)
            {
                if (arg.Cancel)
                    break;
                try
                {
                    i.OnStart(arg);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Osgi:执行OnStart(AppEventArgs)时异常！类型:'{0}'", i.GetType().FullName), ex);
                }
            }

        }
        protected internal override void UnLoad()
        {
            base.UnLoad();
            var arg = new AppEventArgs()
            {
                Sender = this,
            };
            foreach (var i in HandlerTypes.Values)
            {
                if (arg.Cancel)
                    break;
                try
                {
                    i.OnStop(arg);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Osgi:执行OnStop(AppEventArgs)时异常！类型:'{0}'", i.GetType().FullName), ex);
                }
            }
        }
    }
}