using System;

namespace ZXC
{
    public interface IRecycleObject : IDisposable
    {
        void Create();
        void Recycle();
    }
}