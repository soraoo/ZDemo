using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using ZXC;

namespace ZXC.Res
{
    public class AssetLoader<T> : IRecycleObject where T : UnityEngine.Object
    {
        private class AssetInfo<TInfo> : IAssetInfo<TInfo> where TInfo : UnityEngine.Object
        {
            public string AssetPath { get; set; }

            public bool IsCompleted { get; set; }

            public bool IsSuccess { get; set; }

            public bool IsError { get; set; }

            public string ErrorMsg { get; set; }

            public float Progress { get; set; }

            public object ResultObject { get { return Asset; } }

            public TInfo Asset { get; set; }

            public void Dispose()
            {
                Asset = null;
            }
        }

        private AssetInfo<T> assetInfo;


        public AssetLoader()
        {
            InitAssetInfo();
        }

        public void Create()
        {

        }

        public void Recycle()
        {
            assetInfo.IsCompleted = false;
            assetInfo.IsSuccess = false;
            assetInfo.IsError = false;
            assetInfo.ErrorMsg = string.Empty;
            assetInfo.Progress = 0f;
        }

        /// <summary>
        /// 释放引用
        /// </summary>
        public void Dispose()
        {
            assetInfo = null;
            OnDispose();
        }

        protected void InitAssetInfo()
        {
            assetInfo = new AssetInfo<T>();
            assetInfo.IsCompleted = false;
            assetInfo.IsSuccess = false;
            assetInfo.IsError = false;
            assetInfo.ErrorMsg = string.Empty;
            assetInfo.Progress = 0f;
        }

        public async Task<AssetBundle> LoadAssetBundle(string assetPath)
        {
            assetInfo.AssetPath = assetPath;
            string path = string.Format($"{ResUtility.GetAssetBundlesPath()}/{assetInfo.AssetPath}");
            return await AssetBundle.LoadFromFileAsync(path);
        }

        public async Task<T> LoadAsset(AssetBundle assetBundle, string assetPath)
        {
            assetInfo.AssetPath = assetPath;
            return await assetBundle.LoadAssetAsync(assetInfo.AssetPath) as T;
        }

        // public async Task<IAsyncOperation<T>> LoadAllAsset(AssetBundle assetBundle)
        // {
        //     return await LoadAsync(assetBundle, DoLoadAllAssetBundle(assetBundle));
        // }

        // private async Task<IAsyncOperation<T>> LoadAsync(AssetBundle assetBundle, IEnumerator itor)
        // {
        //     asc = new AsyncCompletionSource<T>();
        //     Chain.Start()
        //         .Coroutine(itor);
        //     return asc;
        // }

        /// <summary>
        /// 子类析构
        /// </summary>
        protected void OnDispose()
        {

        }

        private async Task<AssetBundle> DoLoadAssetBundleAsync()
        {
            return await AssetBundle.LoadFromFileAsync(assetInfo.AssetPath);
            // AssetBundle assetBundle = null;
            // try
            // {
            //     assetBundle = 
            // }
            // catch (System.Exception)
            // {
                
            // }
            // finally
            // {
            //     assetInfo.Progress = 1f;
            //     assetInfo.IsCompleted = true;
            // }
            // return  assetBundle;
            //  = request.isDone;
            // assetInfo.Asset = request.assetBundle as T;
            // assetInfo.IsSuccess = assetInfo.Asset != null;
            // assetInfo.IsError = assetInfo.Asset == null;
            // if (assetInfo.IsError && assetInfo.Asset == null)
            // {
            //     assetInfo.ErrorMsg = string.Format("load assetBundle {0} is null", assetInfo.Asset.name);
            //     asc.SetException(new AssetLoaderException(assetInfo.ErrorMsg));
            // }
            // else
            // {
            //     asc.SetResult(assetInfo.Asset);
            // }
        }

        // private IEnumerator DoLoadAssetAsync(AssetBundle assetBundle)
        // {
        //     var request = assetBundle.LoadAssetAsync(assetInfo.AssetPath);
        //     while (!request.isDone)
        //     {
        //         assetInfo.Progress = request.progress;
        //         asc.SetProgress(assetInfo.Progress);
        //         yield return null;
        //     }
        //     assetInfo.Progress = 1f;
        //     asc.SetProgress(assetInfo.Progress);
        //     assetInfo.IsCompleted = request.isDone;
        //     assetInfo.Asset = request.asset as T;
        //     assetInfo.IsSuccess = assetInfo.Asset != null;
        //     assetInfo.IsError = assetInfo.Asset == null;
        //     if (assetInfo.IsError && assetInfo.Asset == null)
        //     {
        //         assetInfo.ErrorMsg = string.Format("load asset {0} is null", assetInfo.Asset.name);
        //         asc.SetException(new AssetLoaderException(assetInfo.ErrorMsg));
        //     }
        //     else
        //     {
        //         asc.SetResult(assetInfo.Asset);
        //     }
        // }

        // private IEnumerator DoLoadAllAssetBundle(AssetBundle assetBundle)
        // {
        //     var request = assetBundle.LoadAllAssetsAsync<T>();
        //     while (!request.isDone)
        //     {
        //         assetInfo.Progress = request.progress;
        //         asc.SetProgress(assetInfo.Progress);
        //         yield return null;
        //     }
        //     assetInfo.Progress = 1f;
        //     asc.SetProgress(assetInfo.Progress);
        //     assetInfo.IsCompleted = request.isDone;
        //     assetInfo.Asset = request.allAssets as T;
        //     assetInfo.IsSuccess = assetInfo.Asset != null;
        //     assetInfo.IsError = assetInfo.Asset == null;
        //     if (assetInfo.IsError && assetInfo.Asset == null)
        //     {
        //         assetInfo.ErrorMsg = string.Format("load all asset in {0} is null", assetInfo.AssetPath);
        //         asc.SetException(new AssetLoaderException(assetInfo.ErrorMsg));
        //     }
        //     else
        //     {
        //         asc.SetResult(assetInfo.Asset);
        //     }
        // }
    }
}