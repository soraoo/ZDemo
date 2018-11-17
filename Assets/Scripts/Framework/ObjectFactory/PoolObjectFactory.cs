using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.Factory
{
    public sealed class PoolObjectFactory : IObjectFactory
    {
        private class PoolData
        {
            public bool InUse { get; set; }
            public object Obj { get; set; }
        }

        private readonly List<PoolData> poolList;
        private readonly int maxPoolCount;
        private readonly bool limitCount;
        private int poolCount;

        public PoolObjectFactory(int maxPoolCount, bool limitCount)
        {
            this.maxPoolCount = maxPoolCount;
            this.limitCount = limitCount;
            poolList = new List<PoolData>();
            poolCount = 0;
        }

        public T CreateObject<T>(params object[] param) where T : class
        {
            return GetObject(typeof(T), param) as T;
        }

        public void ReleaseObject(object obj)
        {
            if (poolCount > maxPoolCount)
            {
                if (obj is IDisposable)
                {
                    (obj as IDisposable).Dispose();
                }
                var poolData = GetPoolData(obj);
                lock (poolList)
                {
                    poolList.Remove(poolData);
                }
            }
            else
            {
                RecycleObject(obj);
            }
        }

        private object GetObject(Type type, params object[] param)
        {
            lock (poolList)
            {
                for (int i = 0; i < poolCount; i++)
                {
                    var poolData = poolList[i];
                    if (!poolData.InUse)
                    {
                        poolData.InUse = true;
                        return poolData.Obj;
                    }
                }

                if (poolCount >= maxPoolCount && limitCount)
                {
                    ZLog.Warning("object pool has max");
                    return null;
                }

                object obj = ZInstanceUtility.CreateInstance(type, param);
                var newPoolData = new PoolData
                {
                    InUse = false,
                    Obj = obj
                };
                poolList.Add(newPoolData);
                poolCount++;
                return obj;
            }
        }

        private void RecycleObject(object obj)
        {
            lock (poolList)
            {
                var poolData = GetPoolData(obj);
                if (poolData != null)
                {
                    poolData.InUse = false;
                }
            }
        }

        private PoolData GetPoolData(object obj)
        {
            for (int i = 0; i < poolCount; i++)
            {
                var poolData = poolList[i];
                if (poolData == obj)
                {
                    return poolData;
                }
            }
            return null;
        }
    }
}