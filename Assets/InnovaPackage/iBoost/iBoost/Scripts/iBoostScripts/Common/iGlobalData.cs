using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iBoost
{
    public class iGlobalData<T>
    {
        private static Dictionary<string, T> stored = new Dictionary<string, T>();

        public static T Get(string key)
        {
            return stored.ContainsKey(key) ? stored[key] : default(T);
        }

        public static void Set(string key, T data)
        {
            stored[key] = data;
        }

        public static void Delete(string key)
        {
            if(stored.ContainsKey(key))
            {
                stored.Remove(key);
            }
        }

        public static void Clear()
        {
            stored.Clear();
        }
    }
}
