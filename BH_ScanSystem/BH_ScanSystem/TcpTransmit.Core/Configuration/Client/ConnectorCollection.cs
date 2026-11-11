using System.Configuration;

namespace TcpTransmit.Core.Configuration.Client
{
    public class ConnectorCollection : ConfigurationElementCollection {
        #region Overrides of ConfigurationElementCollection

        protected override ConfigurationElement CreateNewElement(){
            return new ConnectorElement();
        }

        protected override object GetElementKey(ConfigurationElement element){
            return ((ConnectorElement) element).Name;
        }

        #endregion

        /// <summary>
        /// 获取对象类型,必须重载
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType{
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// 集合元素名
        /// </summary>
        protected override string ElementName{
            get { return "Connector"; }
        }


        /// <summary>
        /// 集合数量
        /// </summary>
        public new int Count{
            get { return base.Count; }
        }

        /// <summary>
        /// 通过指定key获取元素对象的索引器
        /// </summary>
        /// <param name="name">hashtbale的key</param>
        /// <returns></returns>
        public new ConnectorElement this[string name]{
            get { return (ConnectorElement) BaseGet(name); }
        }

        /// <summary>
        /// 通过下标访问元素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ConnectorElement this[int index]{
            get { return (ConnectorElement) BaseGet(index); }
            set{
                if (BaseGet(index) != null) BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }
    }
}