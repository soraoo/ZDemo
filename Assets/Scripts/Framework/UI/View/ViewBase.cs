using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXC.UI;

namespace ZXC
{
    public class ViewBase : MonoBehaviour, IZView
    {
        public bool IsHide { get; set; }

        void Awake()
        {
            Init();
        }

        void OnDestroy()
        {

        }

        public void Init()
        {
            
        }

        public void Open()
        {
            
        }

        public void Hide()
        {
            
        }

        public void Close()
        {
            
        }

        protected virtual void OnInit()
        {

        }

        protected virtual void OnOpenAnimationStart()
        {

        }

        protected virtual void OnOpenAnimationEnd()
        {

        }

        protected virtual void OnHideAnimationStart()
        {

        }

        protected virtual void OnHideAnimationEnd()
        {

        }

        protected virtual void OnClose()
        {

        }

        protected virtual void BindProperty<T>(string propName, OnValueChangedHandler<T> handler)
        {
            ViewMgr.Instance.BindProperty<T>(this, propName, handler);
        }

        protected virtual void UnBindProperty<T>(string propName, OnValueChangedHandler<T> handler)
        {
            ViewMgr.Instance.UnBindProperty<T>(this, propName, handler);
        }
    }
}