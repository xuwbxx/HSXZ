using System.Configuration;

namespace TcpTransmit.Core.Configuration.Client {
    public class TcpTransmitSection : ConfigurationSection {
        /// <summary>
        /// 初始化及获取实例
        /// </summary>
        private static readonly TcpTransmitSection instance =
            (TcpTransmitSection) ConfigurationManager.GetSection("TcpTransmit");

        public static TcpTransmitSection Instance {
            get { return instance; }
        }


        /// <summary>
        /// 获取监听器集合
        /// </summary>
        [ConfigurationProperty("Clients", IsRequired = true)]
        public ClientsCollection Clients {
            get { return (ClientsCollection) this["Clients"]; }
            set { this["Clients"] = value; }
        }

        /// <summary>
        /// Socket接收缓存大小
        /// </summary>
        [ConfigurationProperty("SocketBufferSize", IsRequired = true, DefaultValue = 4096),
         IntegerValidator(MinValue = 1024, MaxValue = int.MaxValue)]
        public int SocketBufferSize {
            get { return (int) this["SocketBufferSize"]; }
        }

        /// <summary>
        /// 处理缓存大小
        /// </summary>
        [ConfigurationProperty("MessageBufferSize", IsRequired = true, DefaultValue = 8192),
         IntegerValidator(MinValue = 1024, MaxValue = int.MaxValue)]
        public int MessageBufferSize {
            get { return (int) this["MessageBufferSize"]; }
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs最小的数量
        /// </summary>
        [ConfigurationProperty("MinPoolsAsyncEventArgsNum", IsRequired = false, DefaultValue = 2)]
        public int MinPoolsAsyncEventArgsNum {
            get { return (int) this["MinPoolsAsyncEventArgsNum"]; }
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs最大的数量
        /// </summary>
        [ConfigurationProperty("MaxPoolsAsyncEventArgsNum", IsRequired = false, DefaultValue = 5)]
        public int MaxPoolsAsyncEventArgsNum {
            get { return (int) this["MaxPoolsAsyncEventArgsNum"]; }
        }
    }
}