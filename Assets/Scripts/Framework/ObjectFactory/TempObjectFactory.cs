using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.Factory
{
    public sealed class TempObjectFactory : IObjectFactory
    {
        public T CreateObject<T>(params object[] param) where T : class
        {
            var type = typeof(T);
            var instance = ZInstanceUtility.CreateInstance(type, param);
            return instance as T;
        }

        public void ReleaseObject(object obj)
        {
        }
    }
}