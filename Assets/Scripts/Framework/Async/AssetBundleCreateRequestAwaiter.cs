using ZXC.Async;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace ZXC
{
    public class AssetBundleCreateRequestAwaiter : IZAwaiter<AssetBundle>
    {
        private readonly AssetBundleCreateRequest request;

        public AssetBundleCreateRequestAwaiter(AssetBundleCreateRequest request)
        {
            this.request = request;
        }

        public bool IsCompleted => request.isDone;
        
        public AssetBundle GetResult()
        {
            return request.assetBundle;
        }

        public async void OnCompleted(Action continuation)
        {
            while(!IsCompleted)
            {
                await Task.Delay(TimeSpan.FromSeconds(Time.unscaledDeltaTime));
            }
            continuation?.Invoke();
        }
    }
}