using System.Configuration;

namespace TcpTransmit.Core.Configuration.Server {
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
        [ConfigurationProperty("Listeners", IsRequired = true)]
        public ListenerCollection Listeners {
            get { return (ListenerCollection) this["Listeners"]; }
            set { this["Listeners"] = value; }
        }


        /// <summary>
        /// 获取处理者
        /// </summary>
        [ConfigurationProperty("Handler", IsRequired = true)]
        public HandlerElement Handler {
            get { return (HandlerElement) this["Handler"]; }
            set { this["Handler"] = value; }
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
        /// 每个连接的最大休息时间(单位分钟)
        /// </summary>
        [ConfigurationProperty("IdleTimeOutValue", IsRequired = true, DefaultValue = 5000),
         IntegerValidator(MinValue = 0, MaxValue = int.MaxValue)]
        public int IdleTimeOutValue {
            get { return (int) this["IdleTimeOutValue"]; }
        }

        /// <summary>
        /// 检查的时间间隔
        /// </summary>
        [ConfigurationProperty("IdleCheckInterval", IsRequired = true, DefaultValue = 10),
         IntegerValidator(MinValue = 0, MaxValue = int.MaxValue)]
        public int IdleCheckInterval {
            get { return (int) this["IdleCheckInterval"]; }
        }


        /// <summary>
        /// 池中SocketAsyncEventArgs最小的数量
        /// </summary>
        [ConfigurationProperty("MinPoolsAsyncEventArgsNum", IsRequired = false, DefaultValue = 100)]
        public int MinPoolsAsyncEventArgsNum {
            get { return (int) this["MinPoolsAsyncEventArgsNum"]; }
        }

        /// <summary>
        /// 池中SocketAsyncEventArgs最大的数量
        /// </summary>
        [ConfigurationProperty("MaxPoolsAsyncEventArgsNum", IsRequired = false, DefaultValue = 500)]
        public int MaxPoolsAsyncEventArgsNum {
            get { return (int) this["MaxPoolsAsyncEventArgsNum"]; }
        }
    }
}