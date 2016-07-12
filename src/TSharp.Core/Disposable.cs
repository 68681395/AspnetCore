using System;

namespace TSharp.Core
{
    /// <summary>
    /// 资源释放基类。所有需要释放的类需实现此类。或者按此类方式实现 IDisposable
    /// </summary>
    /// <author>
    /// Tang Jing bo
    /// </author>
    /// <remarks>
    /// Created : 2009-12-12
    /// </remarks>
    public abstract class Disposable : IDisposable
    { 
        /// <summary>
        /// 是否已经释放资源的标志
        /// </summary>
        private bool disposed;

        #region IDisposable Members

        /// <summary>
        /// 提供给外部用户显示调用的方法，实际操作是在类的带参数的虚函数Dispose(bool disposing)中实现
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 这里释放托管资源
                }
                // 这里释放所有非托管资源
            }
            disposed = true;

            /**** 重写实现范例
                          private bool disposed;
                          protected override void Dispose(bool disposing)
                          {
                              if (!disposed)
                              {
                                  try
                                  {
                                      if (disposing)
                                      {
                                          // 释放托管资源
                                      }
                                      // 这里释放所有非托管资源
                                      disposed = true;
                                  }
                                  finally
                                  {
                                      base.Dispose(disposing);
                                  }
                              }
                          }
             * */
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="Disposable"/> is reclaimed by garbage collection.
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }
    }
}