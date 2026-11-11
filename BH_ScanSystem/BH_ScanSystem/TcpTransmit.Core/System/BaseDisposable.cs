using System;

namespace TcpTransmit.Core.System {
    /// <summary>
    /// 释放资源的基类
    /// </summary>
    public abstract class BaseDisposable : IDisposable {
        #region 字段
        protected readonly object myLock = new object();
        protected bool disposed;
        #endregion

        #region 方法
        /// <summary>
        /// This method is called when object is being disposed. Override this method to free resources.
        /// </summary>
        /// <param name="canAccessFinalizable">
        /// Indicates if the method can access Finalizable member objects.
        /// If canAccessFinalizable = false the method was called by GC and you can access finalizable member objects.
        /// If canAccessFinalizable = true the method was called by user and you can access all member objects.
        /// </param>
        protected virtual void Free(bool canAccessFinalizable){
            disposed = true;
        }

        /// <summary>
        /// 查看资源是否已经释放
        /// </summary>
        protected void CheckDisposedWithException(){
            if (disposed)
                throw new ObjectDisposedException(ToString());
        }
        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose(){
            if (disposed)
                return;

            lock (myLock){
                if (!disposed){
                    try{
                        Free(true);
                    }
                    finally{
                        disposed = true;
                    }
                }
            }
        }
        #endregion

        #endregion
    }
}