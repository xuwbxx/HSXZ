using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Tool
{
	public class CacheUtils
	{
		public static void Set(string key, object value, int minutes)
		{
			HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes((double)minutes), Cache.NoSlidingExpiration);
		}

		public static object Get(string key)
		{
			return HttpRuntime.Cache[key];
		}

		public static void Remove(string keyword)
		{
			foreach (object current in HttpRuntime.Cache)
			{
				string text = ((DictionaryEntry)current).Key.ToString();
				bool flag = text.StartsWith(keyword);
				if (flag)
				{
					HttpRuntime.Cache.Remove(text);
				}
			}
		}
	}
}
