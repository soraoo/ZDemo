using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC.UI
{
    public interface IZView
    {
        bool IsHide { get; set; }
        void Init();
        void Show();
        void Hide();
        void Close();
    }
}