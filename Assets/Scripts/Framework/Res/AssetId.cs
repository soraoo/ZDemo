using System;

namespace ZXC
{
    public class AssetId
    {
        public string BundleId { get; private set; }

        public string ResId { get; private set; }

        /// <summary>
        /// 创建AssetId的工厂方法（未使用Pool，而是直接new）
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public static AssetId Create(string bundleId, string resId)
        {
            return new AssetId(bundleId, resId);
        }

        public AssetId(string bundleId, string resId)
        {
            BundleId = bundleId;
            ResId = resId;
        }

        /// <summary>
        /// 格式化输出
        /// </summary>
        /// <returns>B/R</returns>
        public override string ToString()
        {
            return BundleId + "/" + ResId;
        }

        public override bool Equals(object obj)
        {
            if(obj is AssetId)
            {
                return this == obj as AssetId;
            }
            else
            {
                throw new InvalidCastException("the args obj is not a assetId");
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator == (AssetId lhs, AssetId rhs)
        {
            return lhs.BundleId == rhs.BundleId &&
                lhs.ResId == rhs.ResId;
        }

        public static bool operator != (AssetId lhs, AssetId rhs)
        {
            return lhs.BundleId != rhs.BundleId || 
                lhs.ResId == rhs.ResId;
        }
    }
}