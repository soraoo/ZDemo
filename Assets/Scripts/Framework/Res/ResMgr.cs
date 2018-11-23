using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ZXC.Res;

namespace ZXC
{
    public class ResMgr : ZMonoSingleton<ResMgr>
    {
        private HashSet<string> permanentAssetBundleHashSet;
        private Dictionary<string, AssetBundle> cacheAssetBundleDic;
        private Dictionary<string, int> assetBundleRefCountDic;
        private Dictionary<AssetId, Object> cacheResDic;
        private AssetBundleManifest manifest;

        public override async Task Init()
        {
            //缓存manifest
            await DoInit();
        }

        public async Task<AssetBundle> LoadAssetBundle(string bundlePath)
        {
            AssetBundle assetBundle = null;
            if (cacheAssetBundleDic.TryGetValue(bundlePath, out assetBundle))
            {
                return assetBundle;
            }
            else
            {
                try
                {
                    //load dependences
                    await LoadAssetBundleDependences(manifest.GetAllDependencies(bundlePath));
                    var loader = CreateAssetLoader<AssetBundle>();
                    assetBundle = await loader.LoadAssetBundle(bundlePath);
                    RecycleAssetLoader(loader);
                    assetBundleRefCountDic.Add(bundlePath, 1);
                    cacheAssetBundleDic.Add(bundlePath, assetBundle);
                }
                catch (System.Exception e)
                {
                    ZLog.Debug(e.Message + "=====" + e.StackTrace);
                }
                return assetBundle;
            }
        }

        public async Task<T> LoadAsset<T>(AssetId assetId) where T : Object
        {
            Object o = null;
            if (cacheResDic.TryGetValue(assetId, out o))
            {
                return o as T;
            }
            else
            {
                // #if UNITY_EDITOR
                //                 LoadAssetFromLocal<T>(assetId, onFinished);
                // #else
                AssetBundle bundle = null;
                T asset = null;
                try
                {
                    if (cacheAssetBundleDic.TryGetValue(assetId.BundleId, out bundle))
                    {
                        asset = await LoadAssetFromBundle<T>(assetId, bundle);
                    }
                    else
                    {
                        bundle = await LoadAssetBundle(assetId.BundleId);
                        asset = await LoadAssetFromBundle<T>(assetId, bundle);
                    }
                }
                catch (System.Exception e)
                {
                    ZLog.Debug(e.Message + "=====" + e.StackTrace);
                }
                return asset;
                //#endif  
            }
        }

        public void UnloadAssetBundle(string bundlePath)
        {
            AssetBundle assetBundle = null;
            if (cacheAssetBundleDic.TryGetValue(bundlePath, out assetBundle))
            {
                var dependences = manifest.GetAllDependencies(bundlePath);
                int count = dependences.Length;
                for (int i = count - 1; i >= 0; i--)
                {
                    var dependence = dependences[i];
                    Unload(dependence);
                }
                Unload(bundlePath);
            }
            else
            {
                ZLog.Error($"No bundle cache named->{bundlePath}");
            }
        }

        public void GC()
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        private void Unload(string name)
        {
            assetBundleRefCountDic[name]--;
            if (assetBundleRefCountDic[name] <= 0)
            {
                assetBundleRefCountDic.Remove(name);
                cacheAssetBundleDic[name].Unload(true);
                cacheAssetBundleDic.Remove(name);
            }
        }

        protected override void AfterAwake()
        {
            permanentAssetBundleHashSet = new HashSet<string>();
            cacheResDic = new Dictionary<AssetId, Object>();
            cacheAssetBundleDic = new Dictionary<string, AssetBundle>();
            assetBundleRefCountDic = new Dictionary<string, int>();
        }

        private async Task DoInit()
        {
            //load asset bundle mainifest
            var assetBundle = await AssetBundle.LoadFromFileAsync(string.Format("{0}/{1}", ResUtility.GetAssetBundlesPath(), ResUtility.ASSET_BUNDLE_FOLDER_NAME));
            manifest = assetBundle.LoadAsset<AssetBundleManifest>(ResUtility.ASSET_BUNDLE_MANIFEST);
        }

        // #if UNITY_EDITOR
        //         private void LoadAssetFromLocal<T>(AssetId assetId, LoadAssetDelegate<T> onFinished) where T : Object
        //         {
        //             T asset = null;
        //             bool isSuccess = false;
        //             string errMsg = string.Empty;
        //             try
        //             {
        //                 asset = AssetDatabase.LoadAssetAtPath<T>(assetId.ToString());
        //             }
        //             catch (System.Exception e)
        //             {
        //                 isSuccess = false;
        //                 errMsg = e.ToString();
        //             }
        //             finally
        //             {
        //                 onFinished(isSuccess, errMsg, asset);
        //             }
        //         }
        // #endif

        private async Task LoadAssetBundleDependences(string[] dependencies)
        {
            foreach (var dependence in dependencies)
            {
                if (cacheAssetBundleDic.ContainsKey(dependence))
                {
                    assetBundleRefCountDic[dependence]++;
                    continue;
                }
                var depedenceLoader = CreateAssetLoader<AssetBundle>();
                var depedenceAB = await depedenceLoader.LoadAssetBundle(dependence);
                RecycleAssetLoader(depedenceLoader);
                assetBundleRefCountDic.Add(dependence, 1);
                cacheAssetBundleDic.Add(dependence, depedenceAB);
            }
        }

        private async Task<T> LoadAssetFromBundle<T>(AssetId assetId, AssetBundle bundle) where T : Object
        {
            var loader = CreateAssetLoader<T>();
            T asset = await loader.LoadAsset(bundle, assetId.ResId);
            RecycleAssetLoader(loader);
            cacheResDic.Add(assetId, asset);
            return asset;
        }

        private AssetLoader<T> CreateAssetLoader<T>() where T : Object
        {
            return ObjectFactory.GetFactory(FactoryType.Pool).CreateObject<AssetLoader<T>>();
        }

        private void RecycleAssetLoader<T>(AssetLoader<T> loader) where T : Object
        {
            ObjectFactory.GetFactory(FactoryType.Pool).ReleaseObject(loader);
        }
    }
}