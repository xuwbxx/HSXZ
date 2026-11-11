using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Tool
{
    public class XmlHelper
    {
        /// <summary>
        /// 将XML字符串反序列化为对象集合列表
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">XML字符</param>
        /// <returns></returns>
        public static List<T> DeserializeToList<T>(string xml, string field) where T : new()
        {
            List<T> list = new List<T>();

            if (!string.IsNullOrWhiteSpace(xml))
            {
                XElement dom = XElement.Parse(xml);

                List<XElement> xes = dom.Descendants(field).ToList();
                PropertyInfo[] ps = typeof(T).GetProperties();
                foreach (XElement xe in xes)
                {
                    T t = new T();
                    foreach (PropertyInfo pi in ps)
                    {
                        t.GetType().GetProperty(pi.Name).SetValue(t, xe.Attribute(pi.Name).Value, null);
                    }
                    list.Add(t);
                }
            }

            return list;
        }

        /// <summary>
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="xml">XML字符</param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(string xml, string field) where T : new()
        {
            T t = new T();
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XElement dom = XElement.Parse(xml);
                XElement xe = dom.Descendants(field).FirstOrDefault();
                if (xe == null)
                {
                    PropertyInfo[] ps = typeof(T).GetProperties();
                    foreach (PropertyInfo pi in ps)
                    {
                        t.GetType().GetProperty(pi.Name).SetValue(t, xe.Attribute(pi.Name).Value, null);
                    }
                }
            }
            return t;
        }
    }
}
