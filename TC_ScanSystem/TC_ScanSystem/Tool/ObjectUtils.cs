using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tool
{
    public static class ObjectUtils
    {
        /// <summary>
        /// Default Skip List for "CopyObjectValue"
        /// </summary>
        public static List<string> DefaultSkipList = new List<string>()
        {
            "CreatedBy","CreatedByID","CreatedByTime","CreateBy","CreateByID","CreatedTime","CreatedByName","CreateByName",
            "UpdatedBy","UpdatedByID","UpdatedByTime","UpdateBy","UpdateByID","UpdatedTime","UpdatedByName","UpdaedTime"
        };

        /// <summary>
        /// 根据源对象给目标对象的属性赋值（仅赋值给有相同名称和类别的属性）
        /// </summary>
        /// <param name="source">源对象</param>
        /// <param name="destination">目标对象</param>
        /// <param name="skipProperties">忽略的属性名列表</param>
        /// <param name="fillEmptyOnly">是否只给空值赋值（默认为否）</param>
        /// <returns>影响属性数量</returns>
        public static int CopyObjectValue(object source, object destination, List<string> skipProperties, bool fillEmptyOnly = false)
        {
            int count = 0;
            var typeSource = source.GetType();
            var typeDestination = destination.GetType();

            // 获取属性
            var propsSource = typeSource.GetProperties();
            var propsDestination = typeDestination.GetProperties();

            // 过滤掉忽略的属性
            skipProperties = skipProperties != null ? skipProperties : new List<string>();
            propsSource = Array.FindAll(propsSource, x => !skipProperties.Contains(x.Name));

            // 遍历赋值源对象的属性
            foreach (var prop in propsSource)
            {
                var typeProp = prop.PropertyType;
                // 在赋值目标对象中寻找相同名称和类型的对象
                var propDestination = propsDestination.FirstOrDefault(x => x.Name == prop.Name && x.PropertyType == prop.PropertyType);
                if (propDestination != null && propDestination.CanWrite && prop.CanRead)
                {
                    // 若目标属性值【不】为空且【只为空值赋值】为真，则不做赋值
                    if (fillEmptyOnly && propDestination.GetValue(destination, null) != null)
                    {
                        //continue;
                    }
                    // 若目标属性和源属性值不同，则赋值
                    else if (propDestination.GetValue(destination, null) != prop.GetValue(source, null))
                    {
                        propDestination.SetValue(destination, prop.GetValue(source, null), null);
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
