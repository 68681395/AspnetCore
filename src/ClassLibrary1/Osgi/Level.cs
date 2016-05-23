namespace TSharp.Core.Osgi
{
    /// <summary>
    /// 服务缓存级别
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    public enum Level : int
    {
        /// <summary>
        /// 
        /// </summary>
        ROOT = 1 << 0,
        /// <summary>
        /// 
        /// </summary>
        SESSION = 1 << 1,
        /// <summary>
        /// 
        /// </summary>
        REQUSET = 1 << 2,
        /// <summary>
        /// 
        /// </summary>
        THREAD = 1 << 3,

        /// <summary>
        /// TRANSIENT
        /// </summary>
        TRANSIENT = 1 << 31
    }
}