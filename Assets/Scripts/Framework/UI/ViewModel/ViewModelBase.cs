using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXC.UI;

namespace ZXC
{
    public class ViewModelBase : IZViewModel
    {
        public void Init()
        {
        }

        public void Enable()
        {
        }

        public void Disable()
        {      
        }

        public void Dispose()
        {           
        }

        protected virtual void OnInit()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {      
        }

        protected virtual void OnDispose()
        {
        }
    }
}