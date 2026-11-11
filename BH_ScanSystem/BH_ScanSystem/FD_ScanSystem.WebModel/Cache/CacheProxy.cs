using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Caching;
using System.Threading;

namespace FD_ScanSystem.WebModel.Cache
{
    public class CacheProxy
    {
        #region Delegates

        /// <summary>
        /// 数据库回调
        /// </summary>
        public delegate T DbCallback<out T>() where T : class;

        #endregion

        //private static readonly ILog Log = LogManager.GetLogger("CacheProxy");

        private static MemoryCache MemoryCache = new MemoryCache("CacheProxy");

        public static bool Containes(string key)
        {
            object objectT = MemoryCache.Get(key);
            return objectT != null;
        }

        /// <summary>
        /// 使用MemCache缓存时的方法,过期时间1小时,等待锁时间500毫秒
        /// </summary>
        /// <typeparam name="T">返回值类型,必须是class</typeparam>
        /// <param name="callBack">缓存中不存在时调用的数据库回调方法</param>
        /// <param name="key">缓存key</param>
        /// <returns>当等不到锁时会返回空值,注意判断</returns>
        public static T Get<T>(DbCallback<T> callBack, string key) where T : class
        {
            return Get(callBack, key, 60, 500);
        }

        /// <summary>
        /// 使用MemCache缓存时的方法,等待锁时间500毫秒
        /// </summary>
        /// <typeparam name="T">返回值类型,必须是class</typeparam>
        /// <param name="callBack">缓存中不存在时调用的数据库回调方法</param>
        /// <param name="key">缓存key</param>
        /// <param name="expiry">缓存过期时间(单位分钟),memcached比普通缓存多1个小时,采用相对过期机制</param>
        /// <returns>当等不到锁时会返回空值,注意判断</returns>
        public static T Get<T>(DbCallback<T> callBack, string key, int expiry) where T : class
        {
            return Get(callBack, key, expiry, 500);
        }

        /// <summary>
        /// 使用MemCache缓存时的方法
        /// </summary>
        /// <typeparam name="T">返回值类型,必须是class</typeparam>
        /// <param name="callBack">缓存中不存在时调用的数据库回调方法</param>
        /// <param name="key">缓存key</param>
        /// <param name="expiry">缓存过期时间(单位分钟),memcached比普通缓存多1个小时,采用相对过期机制</param>
        /// <param name="waitMillseconds">等待时间,如果时间到了返回默认值</param>
        /// <returns>当等不到锁时会返回空值,注意判断</returns>
        public static T Get<T>(DbCallback<T> callBack, string key, int expiry, int waitMillseconds) where T : class
        {
            object objectT = MemoryCache.Get(key);
            if (objectT != null)
                return (T)objectT;



            if (Monitor.TryEnter(key, waitMillseconds))
            {
                try
                {
                    if ((objectT = MemoryCache.Get(key)) != null)
                        return (T)objectT;
                    T valueFromDb = callBack();
                    if (valueFromDb == null)
                    {
                        return null;
                    }
                    MemoryCache.Set(key, valueFromDb,
                                    new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, expiry, 0) });
                    return valueFromDb;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    Monitor.Exit(key);
                }
            }
            return default(T);
        }


        /// <summary>
        /// 使用Memory缓存时的方法
        /// </summary>
        /// <typeparam name="T">返回值类型,必须是class</typeparam>
        /// <param name="callBack">缓存中不存在时调用的数据库回调方法</param>
        /// <param name="key">缓存key</param>
        /// <param name="expiry">缓存过期时间</param>
        /// <returns></returns>
        public static T GetMemory<T>(DbCallback<T> callBack, string key, int expiry) where T : class
        {
            T tTemp = GetMemory<T>(key);
            if (tTemp != default(T))
                return tTemp;

            object objectT = null;
            lock (key)
            {
                if ((objectT = MemoryCache.Get(key)) != null)
                    return (T)objectT;

                T valueFromDb = callBack();
                if (valueFromDb == null)
                {
                    return null;
                }
                MemoryCache.Set(key, valueFromDb,
                                new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, expiry, 0) });
                return valueFromDb;
            }
        }

        /// <summary>
        /// 使用Memory缓存时的方法
        /// </summary>
        /// <typeparam name="T">返回值类型,必须是class</typeparam>
        /// <param name="callBack">缓存中不存在时调用的数据库回调方法</param>
        /// <param name="key">缓存key</param>
        /// <param name="expiry">缓存过期时间</param>
        /// <returns></returns>
        public static T GetMemory<T>(string key) where T : class
        {
            object objectT = MemoryCache.Get(key);
            if (objectT != null)
                return (T)objectT;
            else
                return null;

        }

        /// <summary>
        /// 删除某个键,同时删除内存和memcache
        /// 线程安全
        /// </summary>
        /// <param name="key"></param>
        public static void Delete(string key)
        {
            lock (key)
            {
                MemoryCache.Remove(key);
            }
        }

        /// <summary>
        /// 从Memory中删除某个值
        /// 线程安全
        /// </summary>
        /// <param name="key">键</param>
        public static void DeleteMemory(string key)
        {
            lock (key)
            {
                MemoryCache.Remove(key);
            }
        }

        /// <summary>
        /// 使用时要自己保证数据是最新的,保持60分钟
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static void Update<T>(T value, string key) where T : class
        {
            Update(value, key, 60);
        }

        /// <summary>
        /// 使用时要自己保证数据是最新的
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static void Update<T>(T value, string key, int expiry) where T : class
        {
            lock (key)
            {
                MemoryCache.Set(key, value,
                                new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, expiry, 0) });
            }
        }


        /// <summary>
        /// 更新Memory中的值,
        /// 使用时要保证数据是最新的
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="value">值</param>
        /// <param name="key">键</param>
        /// <param name="expiry">过期时间</param>
        public static void UpdateMemory<T>(T value, string key, int expiry) where T : class
        {
            lock (key)
            {
                MemoryCache.Set(key, value,
                                new CacheItemPolicy { SlidingExpiration = new TimeSpan(0, expiry, 0) });
            }
        }


        public static void DeleteAll()
        {
            lock (MemoryCache)
            {
                MemoryCache.Trim(120);
                MemoryCache.Dispose();
                MemoryCache = new MemoryCache("VoteSystem");
            }
        }
    }
}
