using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZXC.UI;
using DG.Tweening;

namespace ZXC
{
    /// <summary>
    /// view动画类型
    /// </summary>
    public enum ViewAnimType
    {
        None,
        Fade,
        Scale,
        Custom
    }

    public abstract class ViewBase<V, P> : MonoBehaviour, IView 
        where V : IView 
        where P : IPresenter<V>
    {
        protected P presenter;
        
        public bool IsHide { get; set; }

        protected GameObject myObj;
        protected Transform myTrans;
        protected CanvasGroup canvasGroup;
        protected ViewAnimType animType;

        public void Init()
        {
            myObj = gameObject;
            myTrans = transform;
            canvasGroup = myObj.GetComponent<CanvasGroup>() ?? myObj.AddComponent<CanvasGroup>();
            myObj.SetActive(false);
            canvasGroup.alpha = 0;
            OnInit();
        }

        public async void Show()
        {
            canvasGroup.interactable = false;
            OnShowAnimationStart();
            switch (animType)
            {
                case ViewAnimType.None:
                    OnShowAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
                case ViewAnimType.Fade:
                    await ShowWithFadeAnim();
                    OnShowAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
                case ViewAnimType.Scale:
                    await ShowWithScaleAnim();
                    OnShowAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
                case ViewAnimType.Custom:
                    await ShowWithCustomAnim();
                    OnShowAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
            }
        }

        public async void Hide()
        {
            canvasGroup.interactable = false;
            OnHideAnimationStart();
            switch (animType)
            {
                case ViewAnimType.None:
                    OnHideAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
                case ViewAnimType.Fade:
                    await HideWithFadeAnim();
                    OnHideAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
                case ViewAnimType.Scale:
                    await HideWithScaleAnim();
                    OnHideAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
                case ViewAnimType.Custom:
                    await HideWithCustomAnim();
                    OnHideAnimationEnd();
                    canvasGroup.interactable = true;
                    break;
            }
        }

        public void Close()
        {
            OnClose();
        }

        protected virtual void OnInit()
        {
            animType = ViewAnimType.None;
        }

        protected virtual void OnShowAnimationStart()
        {

        }

        protected virtual void OnShowAnimationEnd()
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

        protected void BindButtonEvent(int evtName, Button btn)
        {
            
        }

        protected void BindToggleEvent(int evtName, Toggle toggle)
        {

        }

//        protected virtual void BindProperty<T>(string propName, OnValueChangedHandler<T> handler)
//        {
//            ViewMgr.Instance.BindProperty<T>(this, propName, handler);
//        }
//
//        protected virtual void UnBindProperty<T>(string propName, OnValueChangedHandler<T> handler)
//        {
//            ViewMgr.Instance.UnBindProperty<T>(this, propName, handler);
//        }

        protected virtual async Task ShowWithCustomAnim()
        {
            await Task.CompletedTask;
        }

        protected virtual async Task HideWithCustomAnim()
        {
            await Task.CompletedTask;
        }

        private async Task ShowWithFadeAnim()
        {
            myObj.SetSelfActive(true);
            await canvasGroup.DOFade(1f, 0.3f);
        }

        private async Task HideWithFadeAnim()
        {
            await canvasGroup.DOFade(0f, 0.3f);
            myObj.SetSelfActive(false);
        }

        private async Task ShowWithScaleAnim()
        {
            myTrans.localScale = new Vector3(0f, 0f, 0f);
            myObj.SetSelfActive(true);
            await myTrans.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.3f);
            await myTrans.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.3f);
            await myTrans.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
        }

        private async Task HideWithScaleAnim()
        {
            await myTrans.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.3f);
            await myTrans.DOScale(new Vector3(0f, 0f, 0f), 0.3f);
            myObj.SetSelfActive(false);
        }
    }
}