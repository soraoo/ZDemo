using UnityEngine;
using System;
using System.Threading.Tasks;
using ZXC.Async;


namespace ZXC
{
    public sealed class AsyncWaitFrame : IZAwaiter
    {
        private int curFrame;
        private readonly int waitFrames;

        public AsyncWaitFrame(int frame)
        {
            waitFrames = frame;
            curFrame = 0;
        }

        public IZAwaiter GetAwaiter()
        {
            return this;
        }

        public bool IsCompleted => curFrame >= waitFrames;

        public void GetResult()
        {
        }

        public async void OnCompleted(Action continuation)
        {
            while (!IsCompleted)
            {
                await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
                curFrame++;
            }
            continuation?.Invoke();
        }
    }
}