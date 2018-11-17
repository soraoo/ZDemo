using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.UI
{
    public interface IZProperty<T>
    {
        T Value { get; set; }
    }
}