using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.Res
{
    public interface IAssetInfo<T> : IAsyncObject, IDisposable
    {
        string AssetPath { get; }
        T Asset { get; }
    }
}