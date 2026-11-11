using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace TrainingsApp.Helper
{
    public static class EnumHelper
    {
        public static T GetEnumByDescription<T>(string description)
        {
            Type enumType = typeof(T);
            foreach (EnumItem item in GetEnumItems(enumType))
            {
                if (item.Value.ToString().ToLower() == description.Trim().ToLower())
                {
                    return (T)item.Key;
                }
            }
            return default(T);
        }

        public static T GetEnumByName<T>(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                Type enumType = typeof(T);
                foreach (EnumItem item in GetEnumItems(enumType))
                {
                    if (item.Value.ToString().ToLower() == name.Trim().ToLower())
                    {
                        return (T)item.Key;
                    }
                }
            }
            return default(T);
        }

        public static T GetEnumByValue<T>(object key)
        {
            if (key == null)
            {
                return default(T);
            }
            try
            {
                Type enumType = typeof(T);
                foreach (EnumItem item in GetEnumItems(enumType))
                {
                    if (item.Key.ToString().Trim().ToLower() == key.ToString().Trim().ToLower())
                    {
                        return (T)item.Key;
                    }
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }

        public static List<EnumItem> GetEnumItems(Type enumType)
        {
            return GetEnumItems(enumType, false);
        }

        public static List<EnumItem> GetEnumItems(Type enumType, bool withAll)
        {
            List<EnumItem> list = new List<EnumItem>();
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }
            if (withAll)
            {
                list.Add(new EnumItem(null, string.Empty));
            }
            Type attributeType = typeof(DescriptionAttribute);
            foreach (FieldInfo info in enumType.GetFields())
            {
                if (info.FieldType.IsEnum)
                {
                    int num = (int)enumType.InvokeMember(info.Name, BindingFlags.GetField, null, null, null);
                    string description = string.Empty;
                    object[] customAttributes = info.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length > 0)
                    {
                        DescriptionAttribute attribute = (DescriptionAttribute)customAttributes[0];
                        description = attribute.Description;
                    }
                    else
                    {
                        description = info.Name;
                    }
                    list.Add(new EnumItem(num, description));
                }
            }
            return list;
        }

        public static List<EnumItem> GetEnumItemsWithAll(Type enumType)
        {
            return GetEnumItems(enumType, true);
        }

        public static int GetEnumValueByDescription<T>(string description)
        {
            Type enumType = typeof(T);
            foreach (EnumItem item in GetEnumItems(enumType))
            {
                if (item.Value.ToString().ToLower() == description.Trim().ToLower())
                {
                    return Convert.ToInt32(item.Key);
                }
            }
            return -1;
        }

        public static string GetEnumValueDescription<T>(T t)
        {
            Type enumType = typeof(T);
            foreach (EnumItem item in GetEnumItems(enumType))
            {
                if (Convert.ToInt32(item.Key) == Convert.ToInt32(t))
                {
                    return item.Value.ToString();
                }
            }
            return string.Empty;
        }
    }
}

