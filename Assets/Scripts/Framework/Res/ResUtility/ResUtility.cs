using UnityEngine;

namespace ZXC
{
    public static class ResUtility
    {
        public const string ASSET_BUNDLE_FOLDER_NAME = "AssetBundles";
        public const string ASSET_BUNDLE_MANIFEST = "AssetBundleManifest";

        public static string ConvertAssetIdToAssetBundlePath(AssetId assetId)
        {
            return string.Format("{0}/{1}", GetAssetBundlesPath(), assetId.ToString());
        }

        public static string GetAssetBundlesPath()
        {
            return string.Format("{0}/{1}", Application.streamingAssetsPath, ASSET_BUNDLE_FOLDER_NAME);
        }
    }
}