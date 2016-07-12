﻿using System;
using System.Reflection;
using System.Collections.Generic;
using Common.Logging;

namespace TSharp.Core.Osgi.Internal
{
    /// <summary>
    /// 扩展点收集器基类
    /// <para>2010/12/24</para>
    /// 	<para>THINKPADT61</para>
    /// 	<para>tangjingbo</para>
    /// </summary>
    public abstract class ExtensionPoint
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        internal ExtensionPoint()
        {
        }

        internal List<OsgiEngine.RegExtensionAttributeItem> AllRegisters =
            new List<OsgiEngine.RegExtensionAttributeItem>(2000);

        internal virtual void Add(TSharp.Core.Osgi.OsgiEngine.RegExtensionAttributeItem regAttribute)
        {
            this.AllRegisters.Add(regAttribute);
        }

        internal virtual void DoRegisterAll()
        {
            this.AllRegisters.Sort((x, y) => x.ExtensionAttribute.Order.CompareTo(y.ExtensionAttribute.Order));
            foreach (var item in this.AllRegisters)
                try
                {
                    this.InnerRegister(item.Assembly, item.ExtensionAttribute);
                }
                catch (Exception ex)
                {
                    var message = string.Format("程序集{0}中注册属性{1}时发生错误。",  item.Assembly, item.ExtensionAttribute);
                    ex = new Exception(message, ex);
                    log.Error(message, ex);
                    if (OnErrorBreak(ex))
                        throw ex;
                }
        }
        internal virtual void DoUnRegisterAll()
        {
            foreach (var item in this.AllRegisters)
                try
                {
                    this.InnerUnRegister(item.Assembly, item.ExtensionAttribute);
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("程序集{0}中反注册属性{1}时发生错误。", ex, item.Assembly, item.ExtensionAttribute);
                    if (OnErrorBreak(ex))
                        throw ex;
                }
        }
        /// <summary>
        /// 注册扩展
        /// </summary>
        /// <param name="assembly">程序集.</param>
        /// <param name="attribute">扩展.</param>
        internal abstract void InnerRegister(Assembly assembly, ExtensionAttribute attribute);

        /// <summary>
        /// 注销扩展
        /// </summary>
        /// <param name="assembly">程序集.</param>
        /// <param name="attribute">扩展.</param>
        internal abstract void InnerUnRegister(Assembly assembly, ExtensionAttribute attribute);

        /// <summary>
        /// 加载时执行
        /// </summary>
        protected internal virtual void OnLoad()
        {
        }
        /// <summary>
        /// Dispose前时执行
        /// </summary>
        protected internal virtual void UnLoad()
        {
        }
        /// <summary>
        /// 初始化
        /// </summary>
        protected internal virtual void OnInit()
        {
        }
      
        /// <summary>
        /// 当扩展点注册发生错误时是否中断，返回true是抛出异常，中断程序
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected internal virtual Boolean OnErrorBreak(Exception ex)
        {
            return true;
        }
    }
}