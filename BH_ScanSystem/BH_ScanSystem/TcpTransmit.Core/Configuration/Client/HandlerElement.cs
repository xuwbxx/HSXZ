using System.Configuration;

namespace TcpTransmit.Core.Configuration.Client {
    public class HandlerElement : ConfigurationElement {
        /// <summary>
        /// 命名空间
        /// </summary>
        [ConfigurationProperty("AssemblyName", IsRequired = true)]
        public string AssemblyName{
            get { return this["AssemblyName"] as string; }
        }

        /// <summary>
        /// 类名
        /// </summary>
        [ConfigurationProperty("ClassName", IsRequired = true)]
        public string ClassName{
            get { return this["ClassName"] as string; }
        }


        /// <summary>
        /// 信息说明
        /// </summary>
        [ConfigurationProperty("Info", DefaultValue = "")]
        public string Info{
            get { return this["Info"] as string; }
        }

        /// <summary>
        /// 额外参数
        /// </summary>
        [ConfigurationProperty("Argument", DefaultValue = "")]
        public string Argument{
            get { return this["Argument"] as string; }
        }
    }
}