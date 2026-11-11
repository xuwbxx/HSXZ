using System.Configuration;
using System.Net;

namespace TcpTransmit.Core.Configuration.Server {
    public class ListenerElement : ConfigurationElement {
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
        /// 每个线程监听个数
        /// </summary>
        [ConfigurationProperty("BackLog", IsRequired = true, DefaultValue = 5),
         IntegerValidator(MinValue = 1, MaxValue = int.MaxValue)]
        public int BackLog
        {
            get { return int.Parse(this["BackLog"].ToString()); }
        }


        /// <summary>
        /// 监听线程数
        /// </summary>
        [ConfigurationProperty("AcceptThreads", IsRequired = true, DefaultValue = 2),
         IntegerValidator(MinValue = 1, MaxValue = 50)]
        public int AcceptThreads
        {
            get { return byte.Parse(this["AcceptThreads"].ToString()); }
        }
    }
}