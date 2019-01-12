using System;
using System.Threading.Tasks;
using UnityEngine;
using ZXC.Async;

namespace ZXC
{
    public sealed class AsyncWaitForSeconds : IZAwaiter
    {
        private readonly float curTime;
        private readonly float waitSeconds;

        public AsyncWaitForSeconds(float time)
        {
            waitSeconds = time;
            curTime = Time.time;
        }

        public IZAwaiter GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted => Time.time - curTime >= waitSeconds;

        public void GetResult()
        {
        }

        public async void OnCompleted(Action continuation)
        {
            while(!IsCompleted)
            {
                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
            }
            continuation?.Invoke();
        }
    }
}