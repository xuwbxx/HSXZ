using System;

namespace TcpTransmit.Core.System {
    /// <summary>
    /// 可终结模式基类
    /// </summary>
    public abstract class BaseFinalizable : BaseDisposable {
        #region 终结方法

        ~BaseFinalizable(){
            Free(false);
        }

        #endregion

        #region 方法

        #region 释放资源

        /// <summary>
        /// 覆盖基类的Dispose方法
        /// </summary>
        public new void Dispose(){
            if (disposed)
                return;
            lock (myLock){
                if (!disposed){
                    try{
                        Free(true);
                    }
                    finally{
                        disposed = true;
                        GC.SuppressFinalize(this);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}