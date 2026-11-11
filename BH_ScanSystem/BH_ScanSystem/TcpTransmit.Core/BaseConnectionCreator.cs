using System.Net;
using TcpTransmit.Core.System;

namespace TcpTransmit.Core {
    /// <summary>
    /// 连接创造者
    /// </summary>
    public abstract class BaseConnectionCreator : BaseDisposable {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="host">宿主</param>
        /// <param name="name">名字</param>
        /// <param name="localEndPoint">ip地址</param>
        protected BaseConnectionCreator(BaseConnectionHost host, string name, IPEndPoint localEndPoint){

            Host = host;
            Name = name;
            LocalEndPoint = localEndPoint;

        }

        #region Abstract Methods
        /// <summary>
        /// 开始
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// 结束
        /// </summary>
        public abstract void Stop();

        #endregion

        #region Properties
        /// <summary>
        /// 宿主
        /// </summary>
        internal BaseConnectionHost Host { get; private set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// ip地址,客户端代表要连接的地址
        /// 服务器端代表要监听的地址
        /// </summary>
        public IPEndPoint LocalEndPoint{ get; private set;}

        #endregion
    }
}