using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityFx.Async;
using UnityFx.Async.Promises;

namespace ZXC
{
    /// <summary>
    /// 链式调度类
    /// </summary>
    public class Chain
    {
        private delegate void WaitNextDelegate(Action next, Action kill);
        private Queue<WaitNextDelegate> chainQueue;
        public bool IsFinished { get; private set; }
        private bool canNext;

        #region API

        public static Chain Start()
        {
            
            var chain = new Chain();
            return chain;
        }

        public static Chain Start(Action callback)
        {
            var chain = new Chain();
            return chain.Then(callback);
        }

        public static Chain Start(ChainThenDelegate callback)
        {
            var chain = new Chain();
            return chain.Then(callback);
        }

        public Chain Then(Action callback)
        {
            WaitNext((next, kill) =>
            {
                callback();
                next();
            });
            return this;
        }

        public Chain Then(ChainThenDelegate callback)
        {
            WaitNext((next, kill) =>
            {
                callback(next);
            });
            return this;
        }

        public Chain Then(ChainThenDelegateWithKill callback)
        {
            WaitNext((next, kill) =>
            {
                callback(next, kill);
            });
            return this;
        }

        public Chain Coroutine(IEnumerator enumerator)
        {
            WaitNext((next, Kill) =>
            {
                ChainMgr.Instance.StartCoroutine(WaitCoroutine(enumerator, next));
            });
            return this;
        }

        public Chain Coroutine(Coroutine co)
        {
            WaitNext((next, kill) =>
            {
                ChainMgr.Instance.StartCoroutine(WaitCoroutine(co, next));
            });
            return this;
        }

        public Chain WaitForSeconds(float time)
        {
            WaitNext((next, kill) =>
            {
                ChainMgr.Instance.StartCoroutine(WaitCoroutine(time, next));
            });
            return this;
        }

        public Chain WaitForFrame(int frameCount)
        {
            WaitNext((next, kill) =>
            {
                ChainMgr.Instance.StartCoroutine(WaitCoroutine(frameCount, next));
            });
            return this;
        }

        #endregion

        private Chain()
        {
            canNext = true;
        }

        private void WaitNext(WaitNextDelegate callback)
        {
            if (!canNext)
            {
                if (chainQueue == null)
                {
                    chainQueue = new Queue<WaitNextDelegate>();
                }
                chainQueue.Enqueue(callback);
            }
            else
            {
                canNext = false;
                callback(Next, Kill);
            }
        }

        private void Next()
        {
            canNext = true;
            if (chainQueue != null && chainQueue.Count != 0)
            {
                WaitNext(chainQueue.Dequeue());
            }
            else
            {
                IsFinished = true;
                Kill();
            }
        }

        private void Kill()
        {
            canNext = true;
            if (chainQueue != null)
            {
                chainQueue.Clear();
                chainQueue = null;
            }
        }

        private IEnumerator WaitCoroutine(IEnumerator enumerator, Action next)
        {
            yield return ChainMgr.Instance.StartCoroutine(enumerator);
            next();
        }

        private IEnumerator WaitCoroutine(Coroutine co, Action next)
        {
            yield return co;
            next();
        }

        private IEnumerator WaitCoroutine(float time, Action next)
        {
            yield return new WaitForSeconds(time);
            next();
        }

        private IEnumerator WaitCoroutine(int frameCount, Action next)
        {
            while (frameCount > 0)
            {
                yield return null;
                frameCount--;
            }
            next();
        }
    }
}