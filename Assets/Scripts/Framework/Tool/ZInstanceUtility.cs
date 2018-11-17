using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;

namespace ZXC
{
    public static class ZInstanceUtility
    {
        public static object CreateInstance(Type type, params object[] param)
        {
            object instance = null;
            var ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == param.Length);
            if (ctor == null)
            {
                throw new Exception("Constructor not found in " + type);
            }
            else
            {
                instance = ctor.Invoke(param);
            }
            return instance;
        }
	}
}

