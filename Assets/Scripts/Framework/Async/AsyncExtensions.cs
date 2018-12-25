using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using System;
using ZXC.Async;
using DG.Tweening;

namespace ZXC
{
    public static class AsyncExtensions
    {
        public static TaskAwaiter GetAwaiter(this WaitForSeconds waitForSeconds, float time)
        {
            return Task.Delay(TimeSpan.FromSeconds(time)).GetAwaiter();
        }

        public static IZAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest request)
        {
            return new AssetBundleCreateRequestAwaiter(request);
        }

        public static IZAwaiter<UnityEngine.Object> GetAwaiter(this AssetBundleRequest request)
        {
            return new AssetBundleRequestAwaiter(request);
        }

        public static IZAwaiter<Tweener> GetAwaiter(this Tweener tweener)
        {
            return new TweenerAwaiter(tweener);
        }
    }
}