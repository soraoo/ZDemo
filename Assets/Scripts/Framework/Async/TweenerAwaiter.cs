using ZXC.Async;
using UnityEngine;
using System;
using System.Threading.Tasks;
using DG.Tweening;

namespace ZXC
{
    public class TweenerAwaiter : IZAwaiter<Tweener>
    {
        private Tweener tweener;

        public TweenerAwaiter(Tweener tweener)
        {
            this.tweener = tweener;
        }
        public bool IsCompleted
        {
            get
            {
                return tweener.IsComplete();
            }
        }

        public Tweener GetResult()
        {
            return tweener;
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