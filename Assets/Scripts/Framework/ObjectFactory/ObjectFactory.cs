using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXC.Factory;

namespace ZXC
{
    public enum FactoryType
    {
        Single,
        Temp,
        Pool
    }

    public static class ObjectFactory
    {
        private static readonly int MaxPoolCount = 1000;
        private static readonly bool LimitPoolCount = false;
        private static SingleObjectFactory singleObjectFactory = new SingleObjectFactory();
        private static TempObjectFactory tempObjectFactory = new TempObjectFactory();
        private static PoolObjectFactory poolObjectFactory = new PoolObjectFactory(MaxPoolCount, LimitPoolCount);

        public static IObjectFactory GetFactory(FactoryType factoryType)
        {
            IObjectFactory factory = null;
            switch (factoryType)
            {
                case FactoryType.Single:
                    factory = singleObjectFactory;
                    break;
                case FactoryType.Temp:
                    factory = tempObjectFactory;
                    break;
                case FactoryType.Pool:
                    factory = poolObjectFactory;
                    break;
            }
            return factory;
        }
    }
}