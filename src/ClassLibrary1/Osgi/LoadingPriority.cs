using System.ComponentModel;

namespace TSharp.Core.Osgi
{
    /// <summary>
    /// Core定义的优先级
    /// </summary>
    public enum LoadingPriority : byte
    {
#pragma warning disable 3008
        /// <summary>
        /// Core内部使用，外部不得使用
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        __InnerLowest = 0,
        /// <summary>
        /// 最低
        /// </summary>
        Lowest = 1,
        /// <summary>
        /// 低
        /// </summary>
        Low = 63,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 127,
        /// <summary>
        /// 高
        /// </summary>
        High = 191,
        /// <summary>
        /// 最高
        /// </summary>
        Highest = 255,
    }
}