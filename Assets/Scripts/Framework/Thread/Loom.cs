using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace ZXC
{
    public class Loom : ZMonoSingleton<Loom>
    {
        private class DelayItem
        {
            public float time;
            public Action action;
        }

        private Queue<Action> actionList;
        private Queue<DelayItem> delayActionList;
        private int curThreadNum = 0;
        private int maxThreadNum = 8;

        protected override void AfterAwake()
        {
            actionList = new Queue<Action>();
            delayActionList = new Queue<DelayItem>();
        }

        public void QueueInMainThread(Action action)
        {
            QueueInMainThread(action, 0f);
        }

        public void QueueInMainThread(Action action, float time)
        {
            if (time != 0)
            {
                lock (delayActionList)
                {
                    delayActionList.Enqueue(new DelayItem()
                    {
                        time = time,
                        action = action
                    });
                }
            }
            else
            {
                lock (actionList)
                {
                    actionList.Enqueue(action);
                }
            }
        }

        public void RunThread(Action action)
        {
            while (curThreadNum > maxThreadNum)
            {
                Thread.Sleep(1);
            }
            Interlocked.Increment(ref curThreadNum);
        }

        private void RunOnThread(object action)
        {
            try
            {
                (action as Action)();
            }
            catch (Exception e)
            {
                ZLog.Error(e.StackTrace);
            }
            finally
            {
                Interlocked.Decrement(ref curThreadNum);
            }
        }
    }
}