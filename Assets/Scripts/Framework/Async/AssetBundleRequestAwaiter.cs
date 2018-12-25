using ZXC.Async;
using UnityEngine;
using System;
using System.Threading.Tasks;

namespace ZXC
{
    public class AssetBundleRequestAwaiter : IZAwaiter<UnityEngine.Object>
    {
        private AssetBundleRequest request;

        public AssetBundleRequestAwaiter(AssetBundleRequest request)
        {
            this.request = request;
        }
        public bool IsCompleted
        {
            get
            {
                return request.isDone;
            }
        }

        public UnityEngine.Object GetResult()
        {
            return request.asset;
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