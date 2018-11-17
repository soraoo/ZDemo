using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.UI
{
    public interface IZController
    {
        IZViewModel ViewModel { get; }
        void Enabled();
        void Disabled();
        void ExecuteCommand(int command, params object[] param);
    }
}