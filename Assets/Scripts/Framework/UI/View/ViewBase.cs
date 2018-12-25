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

    public class ViewBase : MonoBehaviour, IZView
    {
        public bool IsHide { get; set; }

        protected GameObject myObj;
        protected Transform myTrans;
        protected CanvasGroup canvasGroup;
        protected ViewAnimType animType;

        private Dictionary<string, Selectable> componentDic;
        public void Init()
        {
            myObj = gameObject;
            myTrans = transform;
            canvasGroup = myObj.GetComponent<CanvasGroup>() ?? myObj.AddComponent<CanvasGroup>();
            myObj.SetActive(false);
            canvasGroup.alpha = 0;
            GetAllComponent();
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

        protected void BindButtonEvent(string evtName, UnityAction action)
        {
            Selectable component = null;
            if(componentDic.TryGetValue(evtName, out component))
            {
                (component as Button).onClick.AddListener(action);
            }
        }

        protected void BindToggleEvent(string evtName, UnityAction<bool> action)
        {
            Selectable component = null;
            if(componentDic.TryGetValue(evtName, out component))
            {
                (component as Toggle).onValueChanged.AddListener(action);
            }
        }

        protected virtual void BindProperty<T>(string propName, OnValueChangedHandler<T> handler)
        {
            ViewMgr.Instance.BindProperty<T>(this, propName, handler);
        }

        protected virtual void UnBindProperty<T>(string propName, OnValueChangedHandler<T> handler)
        {
            ViewMgr.Instance.UnBindProperty<T>(this, propName, handler);
        }

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

        private void GetAllComponent()
        {
            var buttons = myTrans.GetComponentsInChildren<Button>();
            foreach (var button in buttons)
            {
                string name = $"On{button.gameObject.name}Click";
                componentDic.Add(name, button);
            }

            var toggles = myTrans.GetComponentsInChildren<Toggle>();
            foreach (var toggle in toggles)
            {
                string name = $"On{toggle.gameObject.name}ValueChanged";
                componentDic.Add(name, toggle);
            }
        }
    }
}