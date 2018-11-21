using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using System;
using ZXC.Async;

namespace ZXC
{
    public static class AsyncExtensions
    {
        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
        {
            return Task.Delay(timeSpan).GetAwaiter();
        }

        public static IZAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest request)
        {
            return new AssetBundleCreateRequestAwaiter(request);
        }

        public static IZAwaiter<UnityEngine.Object> GetAwaiter(this AssetBundleRequest request)
        {
            return new AssetBundleRequestAwaiter(request);
        }
    }
}