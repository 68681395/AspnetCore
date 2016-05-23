namespace TSharp.Core.Osgi
{
    /// <summary>
    /// 
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangj15 at 2012-3-16 9:59
    /// </remarks>
    public interface IOsgiEventHandler
    {
        /// <summary>
        /// Osgi开始启动事件，早于扩展点的Register，OnInit，OnLoad
        /// </summary>
        /// <param name="engine">The engine.</param>
        void Start(OsgiEngine engine);
        /// <summary>
        /// Osgi开始启动完成事件，晚于扩展点的OnInit，OnLoad，是osgi启动的最后事件
        /// </summary>
        /// <param name="engine">The engine.</param>
        void StartCompleted(OsgiEngine engine);

        /// <summary>
        /// Osgi停止启动事件，早于扩展点的 UnLoad
        /// </summary>
        /// <param name="engine">The engine.</param>
        void Stop(OsgiEngine engine);
        /// <summary>
        /// Osgi停止启动完成事件，晚于扩展点的 UnRegister
        /// </summary>
        /// <param name="engine">The engine.</param>
        void StopCompleted(OsgiEngine engine);
    }
}