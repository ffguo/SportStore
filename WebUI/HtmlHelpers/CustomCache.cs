using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class CustomCache
    {
        static CustomCache()
        {
            Task.Run(() =>
            {

            });
        }

        //多次请求结果一样，希望直接用上一次的结果
        //可能有多个类似的需要重用
        private static Dictionary<string, KeyValuePair<object, DateTime>> CustomCacheDictionary = new Dictionary<string, KeyValuePair<object, DateTime>>();

        /// <summary>
        /// 不单单是有key  还得没过期
        /// 
        /// 数据过期了，我不访问，就不清理，岂不是很浪费内存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exist(string key)
        {
            if(CustomCacheDictionary.ContainsKey(key))
            {
                // 没过期
                if(CustomCacheDictionary[key].Value > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    CustomCacheDictionary.Remove(key);
                    return false;
                }
            }
            return false;
        }

        public static T Get<T>(string key)
        {
            return (T)CustomCacheDictionary[key].Key;
        }

        public static void Add(string key, object value, int outTime = 60000)
        {
            CustomCacheDictionary.Add(key, new KeyValuePair<object, DateTime>(value, DateTime.Now.AddMilliseconds(outTime)));
        }

        public static void Remove(string key)
        {
            CustomCacheDictionary.Remove(key);
        }

        public static void RemoveAll()
        {
            CustomCacheDictionary.Clear();
        }

        public static void RemoveCondition(Func<string, bool> func)
        {
            List<string> list = new List<string>();
            foreach (string key in CustomCacheDictionary.Keys)
            {
                if(func(key))
                {
                    list.Add(key);
                }
            }
            list.ForEach(key => CustomCacheDictionary.Remove(key));
        }
    }
}
