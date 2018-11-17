using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityFx.Async;
using ZXC;

namespace ZXC.Res
{
    public class AssetLoader<T> : IDisposable where T : UnityEngine.Object
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
        private AsyncCompletionSource<T> asc;


        public AssetLoader(string assetPath)
        {
            InitAssetInfo(assetPath);
        }

        /// <summary>
        /// 释放引用
        /// </summary>
        public void Dispose()
        {
            assetInfo = null;
            asc = null;
            OnDispose();
        }

        protected void InitAssetInfo(string assetPath)
        {
            assetInfo = new AssetInfo<T>();
            assetInfo.AssetPath = assetPath;
            assetInfo.IsCompleted = false;
            assetInfo.IsSuccess = false;
            assetInfo.IsError = false;
            assetInfo.ErrorMsg = string.Empty;
            assetInfo.Progress = 0f;
        }

        public async Task<IAsyncOperation<T>> LoadAssetBundle()
        {
            return await LoadAsync(null, DoLoadAssetBundleAsync());
        }

        public async Task<IAsyncOperation<T>> LoadAsset(AssetBundle assetBundle)
        {
            return await LoadAsync(assetBundle, DoLoadAssetAsync(assetBundle));
        }

        public async Task<IAsyncOperation<T>> LoadAllAsset(AssetBundle assetBundle)
        {
            return await LoadAsync(assetBundle, DoLoadAllAssetBundle(assetBundle));
        }

        private async Task<IAsyncOperation<T>> LoadAsync(AssetBundle assetBundle, IEnumerator itor)
        {
            asc = new AsyncCompletionSource<T>();
            Chain.Start()
                .Coroutine(itor);
            return asc;
        }

        /// <summary>
        /// 子类析构
        /// </summary>
        protected void OnDispose()
        {

        }

        private async Task<AssetBundle> DoLoadAssetBundleAsync()
        {
            await TimeSpan.FromSeconds(1);
            await AssetBundle.LoadFromFileAsync(assetInfo.AssetPath);
            asc.SetRunning();
            while (!request.isDone)
            { 
                await Task.Delay(TimeSpan.FromMilliseconds(Time.deltaTime));
                assetInfo.Progress = request.progress;
                if (request.progress != 0)
                    asc.SetProgress(request.progress);
                yield return null;
            }
            assetInfo.Progress = 1f;
            assetInfo.IsCompleted = request.isDone;
            assetInfo.Asset = request.assetBundle as T;
            assetInfo.IsSuccess = assetInfo.Asset != null;
            assetInfo.IsError = assetInfo.Asset == null;
            if (assetInfo.IsError && assetInfo.Asset == null)
            {
                assetInfo.ErrorMsg = string.Format("load assetBundle {0} is null", assetInfo.Asset.name);
                asc.SetException(new AssetLoaderException(assetInfo.ErrorMsg));
            }
            else
            {
                asc.SetResult(assetInfo.Asset);
            }
        }

        private IEnumerator DoLoadAssetAsync(AssetBundle assetBundle)
        {
            var request = assetBundle.LoadAssetAsync(assetInfo.AssetPath);
            while (!request.isDone)
            {
                assetInfo.Progress = request.progress;
                asc.SetProgress(assetInfo.Progress);
                yield return null;
            }
            assetInfo.Progress = 1f;
            asc.SetProgress(assetInfo.Progress);
            assetInfo.IsCompleted = request.isDone;
            assetInfo.Asset = request.asset as T;
            assetInfo.IsSuccess = assetInfo.Asset != null;
            assetInfo.IsError = assetInfo.Asset == null;
            if (assetInfo.IsError && assetInfo.Asset == null)
            {
                assetInfo.ErrorMsg = string.Format("load asset {0} is null", assetInfo.Asset.name);
                asc.SetException(new AssetLoaderException(assetInfo.ErrorMsg));
            }
            else
            {
                asc.SetResult(assetInfo.Asset);
            }
        }

        private IEnumerator DoLoadAllAssetBundle(AssetBundle assetBundle)
        {
            var request = assetBundle.LoadAllAssetsAsync<T>();
            while (!request.isDone)
            {
                assetInfo.Progress = request.progress;
                asc.SetProgress(assetInfo.Progress);
                yield return null;
            }
            assetInfo.Progress = 1f;
            asc.SetProgress(assetInfo.Progress);
            assetInfo.IsCompleted = request.isDone;
            assetInfo.Asset = request.allAssets as T;
            assetInfo.IsSuccess = assetInfo.Asset != null;
            assetInfo.IsError = assetInfo.Asset == null;
            if (assetInfo.IsError && assetInfo.Asset == null)
            {
                assetInfo.ErrorMsg = string.Format("load all asset in {0} is null", assetInfo.AssetPath);
                asc.SetException(new AssetLoaderException(assetInfo.ErrorMsg));
            }
            else
            {
                asc.SetResult(assetInfo.Asset);
            }
        }
    }
}