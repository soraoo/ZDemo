using System;
using System.Reflection;

namespace ZXC
{
    public abstract class ZSingleton<T> where T : ZSingleton<T>
    {
        private static object lockObj = new object();

        protected static T instance;

        protected ZSingleton()
        {

        }

        public static T Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = ObjectFactory.GetFactory(FactoryType.Single).CreateObject<T>() as T;
                    }
                    return instance;
                }
            }
        }

        public virtual void Dispose()
        {
            ObjectFactory.GetFactory(FactoryType.Single).ReleaseObject(instance);
            instance = null;
        }
    }
}