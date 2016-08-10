using System;
using System.Configuration;
using System.Web.Caching;
using System.Web;

namespace ATVCommon.Cached
{
    public class CacheController
    {
        private const long _DefaultCacheExpire = 2592000; // in seconds (30 days)

        public static int ParentCategoryId
        {
            get
            {
                if (Lib.QueryString.EventID > 0)
                {
                    return Constants.PARENT_ZONE_ID_FOR_EVENT_LIST_CACHE;
                }
                else
                {
                    return Lib.QueryString.ParentCategoryID;
                }
            }
        }

        public static bool IsAllowDistributedCached
        {
            get
            {
                return (ConfigurationManager.AppSettings["AllowDistCache"] == "1");
            }
        }

        public static bool IsCacheExists(string key)
        {
            if (IsAllowDistributedCached)
            {
                return DistCached.GetInstance(ParentCategoryId).Get(key) != null;
            }
            else
            {
                return false;
            }
        }
        public static bool IsCacheExists(int parentCatId, string key)
        {
            if (IsAllowDistributedCached)
            {
                return DistCached.GetInstance(parentCatId).Get(key) != null;
            }
            else
            {
                return false;
            }
        }

        public static void Add(string key, object value, long timeExpire)
        {
            if (IsAllowDistributedCached)
            {
                string lastUpdateKey = string.Format(Constants.CACHE_NAME_LAST_UPDATE, key);
                if (timeExpire > 0)
                {
                    DistCached.GetInstance(ParentCategoryId).Add(key, value, timeExpire * 1000);
                    DistCached.GetInstance(ParentCategoryId).Add(lastUpdateKey, DateTime.Now.ToString(), timeExpire * 1000);
                }
                else
                {
                    DistCached.GetInstance(ParentCategoryId).Add(key, value, _DefaultCacheExpire * 1000);
                    DistCached.GetInstance(ParentCategoryId).Add(lastUpdateKey, DateTime.Now.ToString(), _DefaultCacheExpire * 1000);
                }
            }
        }
        public static void Add(int parentCatId, string key, object value, long timeExpire)
        {
            if (IsAllowDistributedCached)
            {
                string lastUpdateKey = string.Format(Constants.CACHE_NAME_LAST_UPDATE, key);
                if (timeExpire > 0)
                {
                    DistCached.GetInstance(parentCatId).Add(key, value, timeExpire * 1000);
                    DistCached.GetInstance(parentCatId).Add(lastUpdateKey, DateTime.Now.ToString(), timeExpire * 1000);
                }
                else
                {
                    DistCached.GetInstance(parentCatId).Add(key, value, _DefaultCacheExpire * 1000);
                    DistCached.GetInstance(parentCatId).Add(lastUpdateKey, DateTime.Now.ToString(), _DefaultCacheExpire * 1000);
                }
            }
        }

        public static void SaveToCacheDependency(string cacheName, object data)
        {
            string database = System.Configuration.ConfigurationSettings.AppSettings["CoreDb"];
            SqlCacheDependency sqlDep = new SqlCacheDependency(database, "HtmlCached");
            if (data != null)
                HttpContext.Current.Cache.Insert(cacheName, data, sqlDep);
        }

        public static object GetCache(string key)
        {
            if (IsAllowDistributedCached)
            {
                //Lib.WriteLogToFile(DistCached.GetProvidersState());
                DateTime start = DateTime.Now;
                object data = DistCached.GetInstance(ParentCategoryId).Get(key);
                DateTime end = DateTime.Now;
                Lib.WriteLogToFile(key + "(" + (end.Ticks - start.Ticks).ToString() + "): " + end.ToString("HH:mm:ss.FFFFFFF") + "     " + start.ToString("HH:mm:ss.FFFFFFF"));
                return data;
            }
            else
            {
                return null;
            }
        }
        public static object GetCache(int parentCatId, string key)
        {
            if (IsAllowDistributedCached)
            {
                //Lib.WriteLogToFile(DistCached.GetProvidersState());
                DateTime start = DateTime.Now;
                object data = DistCached.GetInstance(parentCatId).Get(key);
                DateTime end = DateTime.Now;
                Lib.WriteLogToFile(key + "(" + (end.Ticks - start.Ticks).ToString() + "): " + end.ToString("HH:mm:ss.FFFFFFF") + "     " + start.ToString("HH:mm:ss.FFFFFFF"));
                return data;
            }
            else
            {
                return null;
            }
        }

        public static T Get<T>(string key)
        {
            if (IsAllowDistributedCached)
            {
                if (IsCacheExists(ParentCategoryId, key))
                {
                    DateTime start = DateTime.Now;
                    T data = DistCached.GetInstance(ParentCategoryId).Get<T>(key);
                    DateTime end = DateTime.Now;
                    Lib.WriteLogToFile(key + "(" + (end.Ticks - start.Ticks).ToString() + "): " + end.ToString("HH:mm:ss.FFFFFFF") + "     " + start.ToString("HH:mm:ss.FFFFFFF"));
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }
        public static T Get<T>(int parentCatId, string key)
        {
            if (IsAllowDistributedCached)
            {
                if (IsCacheExists(parentCatId, key))
                {
                    DateTime start = DateTime.Now;
                    T data = DistCached.GetInstance(parentCatId).Get<T>(key);
                    DateTime end = DateTime.Now;
                    Lib.WriteLogToFile(key + "(" + (end.Ticks - start.Ticks).ToString() + "): " + end.ToString("HH:mm:ss.FFFFFFF") + "     " + start.ToString("HH:mm:ss.FFFFFFF"));
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                return default(T);
            }
        }

        public static void Remove(string key)
        {
            if (IsAllowDistributedCached)
            {
                string lastUpdateKey = string.Format(Constants.CACHE_NAME_LAST_UPDATE, key);
                DistCached.GetInstance(ParentCategoryId).Remove(key);
                DistCached.GetInstance(ParentCategoryId).Remove(lastUpdateKey);
            }
        }
        public static void Remove(int parentCatId, string key)
        {
            if (IsAllowDistributedCached)
            {
                string lastUpdateKey = string.Format(Constants.CACHE_NAME_LAST_UPDATE, key);
                DistCached.GetInstance(parentCatId).Remove(key);
                DistCached.GetInstance(parentCatId).Add(lastUpdateKey, DateTime.Now.ToString(), 864000 * 1000);
            }
        }


        public static void RemoveAll()
        {
            if (IsAllowDistributedCached)
            {
                DistCached.GetInstance(ParentCategoryId).RemoveAll();
            }
        }
        public static void RemoveAll(int parentCatId)
        {
            if (IsAllowDistributedCached)
            {
                DistCached.GetInstance(parentCatId).RemoveAll();
            }
        }

        /// <summary>
        /// Lấy thời gian cập nhật cache lần cuối
        /// </summary>
        /// <param name="key">Cache name</param>
        /// <returns>Nếu chưa có cache thì trả về DateTime.MinValue</returns>
        public static DateTime GetLastUpdateCache(string key)
        {
            if (IsAllowDistributedCached)
            {
                string lastUpdateKey = string.Format(Constants.CACHE_NAME_LAST_UPDATE, key);
                try
                {
                    object lastUpdate = DistCached.GetInstance(ParentCategoryId).Get(lastUpdateKey);
                    if (null != lastUpdate)
                    {
                        return Convert.ToDateTime(lastUpdate);
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        /// <summary>
        /// Lấy thời gian cập nhật cache lần cuối
        /// </summary>
        /// <param name="parentCatId"></param>
        /// <param name="key"></param>
        /// <returns>Nếu chưa có cache thì trả về DateTime.MinValue</returns>
        public static DateTime GetLastUpdateCache(int parentCatId, string key)
        {
            if (IsAllowDistributedCached)
            {
                string lastUpdateKey = string.Format(Constants.CACHE_NAME_LAST_UPDATE, key);
                try
                {
                    object lastUpdate = DistCached.GetInstance(parentCatId).Get(lastUpdateKey);
                    if (null != lastUpdate)
                    {
                        return Convert.ToDateTime(lastUpdate);
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}