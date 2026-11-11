using System.Configuration;
using System.Net;

namespace TcpTransmit.Core.Configuration.Client
{
    public class ConnectorElement : ConfigurationElement {
        /// <summary>
        /// 发送器名
        /// </summary>
        public IPEndPoint IpEndPoint{
            get { return new IPEndPoint(IPAddress.Parse(IpAdress), Port); }
        }

        /// <summary>
        /// ip地址
        /// </summary>
        [ConfigurationProperty("IpAdress", IsRequired = true)]
        public string IpAdress{
            get { return this["IpAdress"].ToString(); }
        }

        /// <summary>
        /// 端口
        /// </summary>
        [ConfigurationProperty("Port", IsRequired = true, DefaultValue = 8099),
         IntegerValidator(MinValue = 1024, MaxValue = int.MaxValue)]
        public int Port{
            get { return int.Parse(this["Port"].ToString()); }
        }
        
        
        /// <summary>
        /// 监听名
        /// </summary>
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name{

            get { return this["Name"].ToString(); }
        }

        /// <summary>
        /// 连接间隔,毫秒数
        /// </summary>
        [ConfigurationProperty("ReconnectAttemptInterval", IsRequired = true, DefaultValue = 1000),
         IntegerValidator(MinValue = 200, MaxValue =int.MaxValue)]
        public int ReconnectAttemptInterval
        {
            get { return int.Parse(this["ReconnectAttemptInterval"].ToString()); }
        }


        /// <summary>
        /// 最大尝试连接次数
        /// </summary>
        [ConfigurationProperty("ReconnectAttempts", IsRequired = true, DefaultValue = 20),
         IntegerValidator( MaxValue = 500)]
        public int ReconnectAttempts
        {
            get { return int.Parse(this["ReconnectAttempts"].ToString()); }
        }
    }
}