using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.UI
{
    public interface IZViewModel
    {
        void Init();
        void Enable();
        void Disable();
        void Dispose();
    }
}