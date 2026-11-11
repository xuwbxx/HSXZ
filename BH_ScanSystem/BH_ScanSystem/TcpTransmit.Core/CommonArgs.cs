using System;

namespace TcpTransmit.Core {
 


    /// <summary>
    /// Exception event arguments for exception event.
    /// </summary>
    public class ExceptionEventArgs  {
        #region Fields

        #endregion

        #region Constructor

        public ExceptionEventArgs(BaseSocketConnection connection, Exception exception)
        {
            Exception = exception;
            Connection = connection;
        }

        #endregion

        #region Properties

        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the ISocketConnection from event.
        /// </summary>
        public BaseSocketConnection Connection { get; private set; }

        /// <summary>
        /// 是否关闭连接
        /// </summary>
        public bool IsCloseConnection { get; set; }

        #endregion
    }


    internal class WriteData {
        #region Fields

        #endregion

        #region Constructor

        public WriteData(BaseSocketConnection connection, bool sentByServer){
            Connection = connection;
            SentByServer = sentByServer;
        }

        #endregion

        #region Properties

        public BaseSocketConnection Connection { get; set; }

        public bool SentByServer { get; private set; }

        #endregion
    }


    /// <summary>
    /// 信息包装器
    /// </summary>
    internal class MessageBuffer
    {
        #region 构造器

        public MessageBuffer(byte[] buffer, int count, bool sentByServer)
        {
            Buffer = buffer;
            Count = count;
            SentByServer = sentByServer;
        }

        #endregion

        #region 属性

        public byte[] Buffer { get; set; }

        public int Count { get; set; }

        public bool SentByServer { get; set; }

        #endregion
    }

    /// <summary>
    /// Reconnect attempted exception.
    /// </summary>
    public class ReconnectAttemptException : Exception {
        public ReconnectAttemptException(string message, BaseConnectionCreator creator, Exception innerException,
                                         int attempt, bool maxReached)
            : base(message, innerException){
            Attempt = attempt;
            MaxReached = maxReached;
            Creator = creator;
        }

        public int Attempt { get; private set; }

        public bool MaxReached { get; private set; }

        public BaseConnectionCreator Creator { get; private set; }
    }
}