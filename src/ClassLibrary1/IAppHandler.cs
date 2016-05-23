namespace TSharp.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangj15 at 2012-5-4 13:14
    /// </remarks>
    public interface IAppHandler
    {
        /// <summary>
        /// 程序开始调用，等同于Application_Start()
        /// </summary>
        /// <param name="args">The <see cref="TSharp.Core.AppEventArgs"/> instance containing the event data.</param>
        void OnStart(AppEventArgs args);

        /// <summary>
        /// 程序停止时调用，等同于Application_Stop()
        /// </summary>
        /// <param name="args">The <see cref="TSharp.Core.AppEventArgs"/> instance containing the event data.</param>
        void OnStop(AppEventArgs args);
    }
}