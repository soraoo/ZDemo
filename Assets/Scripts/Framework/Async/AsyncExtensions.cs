using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using System;


public static class AsyncExtensions
{
    public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan)
    {
        return Task.Delay(timeSpan).GetAwaiter();
    }

    public static TaskAwaiter<AssetBundle> GetAwaiter(this AssetBundleCreateRequest request)
    {
        while(request.isDone)
        {
            return TimeSpan.FromSeconds(Time.deltaTime).GetAwaiter();
        }
        return Task<AssetBundle>.CompletedTask.GetAwaiter();
    }
}