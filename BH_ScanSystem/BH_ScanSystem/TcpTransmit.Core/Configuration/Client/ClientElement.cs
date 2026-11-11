using System.Configuration;

namespace TcpTransmit.Core.Configuration.Client {
    public class ClientElement : ConfigurationElement {
        /// <summary>
        /// 客户端名
        /// </summary>
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name{
            get { return this["Name"].ToString(); }
        }


        /// <summary>
        /// 获取监听器集合
        /// </summary>
        [ConfigurationProperty("Connectors", IsRequired = true)]
        public ConnectorCollection Connectors{
            get { return (ConnectorCollection) this["Connectors"]; }
            set { this["Connectors"] = value; }
        }


        /// <summary>
        /// 获取处理者
        /// </summary>
        [ConfigurationProperty("Handler", IsRequired = true)]
        public HandlerElement Handler{
            get { return (HandlerElement) this["Handler"]; }
            set { this["Handler"] = value; }
        }
    }
}