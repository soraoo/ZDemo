using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.Factory
{
    public sealed class SingleObjectFactory : IObjectFactory
    {
        private static Dictionary<Type, object> cacheSingleObjectDic = null;
        private static readonly object lockObj = new object();
        private Dictionary<Type, object> CacheSingleObjectDic
        {
            get
            {
                lock (lockObj)
                {
                    if (cacheSingleObjectDic == null)
                    {
                        cacheSingleObjectDic = new Dictionary<Type, object>();
                    }
                    return cacheSingleObjectDic;
                }
            }
        }

        public T CreateObject<T>(params object[] param) where T : class
        {
            var type = typeof(T);
            if (cacheSingleObjectDic.ContainsKey(type))
            {
                return cacheSingleObjectDic[type] as T;
            }
            lock (lockObj)
            {
                var instance = ZInstanceUtility.CreateInstance(type, param);
                cacheSingleObjectDic.Add(type, instance);
                return cacheSingleObjectDic[type] as T;
            }
        }

        public void ReleaseObject(object obj)
        {
            var type = obj.GetType();
            if (cacheSingleObjectDic.ContainsKey(type))
            {
                lock (lockObj)
                {
                    cacheSingleObjectDic[type] = null;
                    cacheSingleObjectDic.Remove(type);
                }
            }
        }
    }
}