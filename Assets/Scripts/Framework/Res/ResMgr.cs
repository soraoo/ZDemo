using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using ZXC.Res;
using UnityFx.Async.Promises;

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

        public override IEnumerator Init()
        {
            //缓存manifest
            yield return DoInit();
        }

        public void LoadAssetBundle(string bundlePath, LoadAssetDelegate<AssetBundle> onFinished)
        {
            bundlePath = string.Format("{0}/{1}", ResUtility.GetAssetBundlesPath(), bundlePath);
            AssetBundle assetBundle = null;
            if (cacheAssetBundleDic.TryGetValue(bundlePath, out assetBundle))
            {
                onFinished(true, null, assetBundle);
            }
            else
            {
                //load dependences
                var dependencies = manifest.GetAllDependencies(bundlePath);
                foreach (var dependence in dependencies)
                {
                    if (cacheAssetBundleDic.ContainsKey(dependence))
                    {
                        assetBundleRefCountDic[dependence]++;
                        continue;
                    }

                    var loader = CreateAssetLoader<AssetBundle>(dependence);
                    Chain.Start().Coroutine(QueueInLoadAssetBundle(loader, (s, e, ab) =>
                    {
                        if (s)
                        {
                            assetBundleRefCountDic.Add(dependence, 1);
                            cacheAssetBundleDic.Add(bundlePath, ab);
                        }
                    }));
                }
                Chain.Start().Coroutine(QueueInLoadAssetBundle(CreateAssetLoader<AssetBundle>(bundlePath), onFinished));
            }
        }

        public void LoadAsset<T>(AssetId assetId, LoadAssetDelegate<T> onFinished) where T : Object
        {
            Object asset = null;
            if (cacheResDic.TryGetValue(assetId, out asset))
            {
                onFinished(true, string.Empty, asset as T);
            }
            else
            {
#if UNITY_EDITOR
                LoadAssetFromLocal<T>(assetId, onFinished);
#else
                LoadAssetFromBundle<T>(assetId, onFinished);
#endif
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

        private IEnumerator DoInit()
        {
            //load asset bundle mainifest
            var request = AssetBundle.LoadFromFileAsync(string.Format("{0}/{1}", ResUtility.GetAssetBundlesPath(), ResUtility.ASSET_BUNDLE_FOLDER_NAME));
            while (!request.isDone)
            {
                yield return null;
            }
            var assetBundle = request.assetBundle;
            manifest = assetBundle.LoadAsset<AssetBundleManifest>(ResUtility.ASSET_BUNDLE_MANIFEST);
        }

#if UNITY_EDITOR
        private void LoadAssetFromLocal<T>(AssetId assetId, LoadAssetDelegate<T> onFinished) where T : Object
        {
            T asset = null;
            bool isSuccess = false;
            string errMsg = string.Empty;
            try
            {
                asset = AssetDatabase.LoadAssetAtPath<T>(assetId.ToString());
            }
            catch (System.Exception e)
            {
                isSuccess = false;
                errMsg = e.ToString();
            }
            finally
            {
                onFinished(isSuccess, errMsg, asset);
            }
        }
#endif

        private void LoadAssetFromBundle<T>(AssetId assetId, LoadAssetDelegate<T> onFinished) where T : Object
        {
            Object asset = null;
            if (cacheResDic.TryGetValue(assetId, out asset))
            {
                onFinished(true, string.Empty, asset as T);
            }
            else
            {
                AssetBundle bundle = null;
                if (cacheAssetBundleDic.TryGetValue(assetId.BundleId, out bundle))
                {
                    var loader = CreateAssetLoader<T>(assetId.ToString());
                }
                else
                {
                    //get depedence
                    var loader = CreateAssetLoader<AssetBundle>(assetId.BundleId);
                }
            }
        }

        private IEnumerator QueueInLoadAssetBundle(AssetLoader<AssetBundle> bundleLoader, LoadAssetDelegate<AssetBundle> onFinished)
        {
            assetBundleLoaderQueue.Enqueue(bundleLoader);
            int loadCount = 0;
            while (assetBundleLoaderQueue.Count != 0)
            {
                if (loadCount >= LoadBundleCount1Frame)
                    yield return null;
                var loader = assetBundleLoaderQueue.Dequeue();
                var op = loader.LoadAssetBundle();
                
                op.Then(assetbundle =>
                {
                    if (onFinished != null)
                        onFinished(true, string.Empty, assetbundle);
                })
                .Catch(e =>
                {
                    if (onFinished != null)
                        onFinished(false, e.Message, null);
                });
                loadCount++;
            }
        }

        private void QueueInLoadRes<T>(AssetLoader<T> loader) where T : Object
        {

        }

        private AssetLoader<T> CreateAssetLoader<T>(string assetPath) where T : Object
        {
            return ObjectFactory.GetFactory(FactoryType.Pool).CreateObject<AssetLoader<T>>(assetPath);
        }
    }
}