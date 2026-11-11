using System;
using System.Threading;

namespace TcpTransmit.Core.System {
    /// <summary>
    /// 线程休息类,在启动的时候调用,让程序平稳的启动
    /// </summary>
    public class ThreadEx {
        #region LoopSleep
        /// <summary>
        /// 让线程休息
        /// </summary>
        /// <param name="loopIndex"></param>
        public static void LoopSleep(ref int loopIndex){
            if ((Environment.ProcessorCount == 1) || (++loopIndex%(Environment.ProcessorCount*50)) == 0){
                //----- Single-core!
                //----- Switch to another running thread!
                Thread.Sleep(5);
            }
            else{
                //----- Multi-core / HT!
                //----- Loop n iterations!
                Thread.SpinWait(20);
            }
        }

        #endregion

        #region SleepEx

        public static void SleepEx(int milliseconds){
            Thread.Sleep(milliseconds <= 0 ? 1 : milliseconds);
        }

        #endregion
    }
}