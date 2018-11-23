
using System;
using System.Runtime.CompilerServices;

namespace ZXC.Async
{
    /// <summary>
    /// 能够await等待
    /// </summary>
    public interface IZAwaiter<TResult> : INotifyCompletion
    {
        bool IsCompleted { get; }
        TResult GetResult();
    }

    public interface IZAwaiter : INotifyCompletion
    {
        bool IsCompleted { get; }
        void GetResult();
    }
}
