using UnityEngine;

namespace ZXC
{
    public static class ResUtility
    {
        public const string ASSET_BUNDLE_FOLDER_NAME = "AssetBundles";
        public const string ASSET_BUNDLE_MANIFEST = "AssetBundleManifest";

        public static string ConvertAssetIdToAssetBundlePath(AssetId assetId)
        {
            return $"{GetAssetBundlesPath()}/{assetId.ToString()}";
        }

        public static string GetAssetBundlesPath()
        {
            return $"{Application.streamingAssetsPath}/{ASSET_BUNDLE_FOLDER_NAME}";
        }
    }
}