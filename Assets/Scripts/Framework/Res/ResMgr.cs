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

        private Queue<AssetLoader<AssetBundle>> assetBundleLoaderQueue;
        private Queue<AssetLoader<Object>> resLoaderQueue;
        private int loadBundleCount1Frame = 1;
        public int LoadBundleCount1Frame { get; set; }
        private int loadResCount1Frame = 1;
        public int LoadResCount1Frame { get; set; }

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
                //load dependences
                await LoadAssetBundleDependences(manifest.GetAllDependencies(bundlePath));
                var loader = CreateAssetLoader<AssetBundle>();
                assetBundle = await loader.LoadAssetBundle(bundlePath);
                RecycleAssetLoader(loader);
                assetBundleRefCountDic.Add(bundlePath, 1);
                cacheAssetBundleDic.Add(bundlePath, assetBundle);
                return assetBundle;
            }
        }

        public async Task<T> LoadAsset<T>(AssetId assetId) where T : Object
        {
            Object asset = null;
            if (cacheResDic.TryGetValue(assetId, out asset))
            {
                return asset as T;
            }
            else
            {
                // #if UNITY_EDITOR
                //                 LoadAssetFromLocal<T>(assetId, onFinished);
                // #else
                AssetBundle bundle = null;
                if (cacheAssetBundleDic.TryGetValue(assetId.BundleId, out bundle))
                {
                    return await LoadAssetFromBundle<T>(assetId, bundle);
                }
                else
                {
                    bundle = await LoadAssetBundle(assetId.BundleId);
                    return await LoadAssetFromBundle<T>(assetId, bundle);
                }
                //#endif  
            }
        }

        protected override void AfterAwake()
        {
            permanentAssetBundleHashSet = new HashSet<string>();
            cacheResDic = new Dictionary<AssetId, Object>();
            cacheAssetBundleDic = new Dictionary<string, AssetBundle>();
            assetBundleRefCountDic = new Dictionary<string, int>();
            assetBundleLoaderQueue = new Queue<AssetLoader<AssetBundle>>();
            resLoaderQueue = new Queue<AssetLoader<Object>>();
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

        // private async void QueueInLoadAssetBundle(AssetLoader<AssetBundle> bundleLoader)
        // {
        //     assetBundleLoaderQueue.Enqueue(bundleLoader);
        //     int loadCount = 0;
        //     while (assetBundleLoaderQueue.Count != 0)
        //     {
        //         if (loadCount >= LoadBundleCount1Frame)
        //             await Task.Delay(1);
        //         var loader = assetBundleLoaderQueue.Dequeue();
        //         var ab = await loader.LoadAssetBundle();
        //         loadCount++;
        //     }
        // }

        //         private void QueueInLoadRes<T>(AssetLoader<T> loader) where T : Object
        //         {

        //         }

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