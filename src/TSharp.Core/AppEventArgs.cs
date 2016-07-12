using System;
using System.Collections.Generic;

namespace TSharp.Core
{

    #region AppEventArgs

    /// <summary>
    /// 应用程序事件参数
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangjingbo at 2009-8-4 13:44
    /// </remarks>
    public class AppEventArgs : EventArgs
    {
        private readonly IDictionary<string, object> items = new Dictionary<string, object>();

        /// <summary>
        /// 事件上下文传递参数的列表
        /// </summary>
        public IDictionary<string, object> Items
        {
            get { return items; }
        }

        /// <summary>
        /// 是否中断后边的事件执行
        /// </summary>
        /// <value>
        ///   <c>true</c> if cancel; otherwise, <c>false</c>.
        /// </value>
        public bool Cancel { get; set; }

        /// <summary>
        /// 事件触发对象
        /// </summary>
        public object Sender { get; set; }
    }

    #endregion

    #region IApplicationEvent

    #endregion 

}