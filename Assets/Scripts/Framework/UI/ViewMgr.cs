using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using ZXC.UI;


namespace ZXC
{
    public class ViewMgr : ZMonoSingleton<ViewMgr>
    {
        private Dictionary<IZController, List<IZView>> controllerDic;
        private Dictionary<IZView, IZController> viewDic;
        private Queue<IZView> closeViewQueue;
        
        private const float CHECK_CLOSE_TIME = 10f;
        
        protected override void AfterAwake()
        {
            controllerDic = new Dictionary<IZController, List<IZView>>();
            viewDic = new Dictionary<IZView, IZController>();
            closeViewQueue = new Queue<IZView>();
        }

        private void Start()
        {
            StartCoroutine(CheckCloseView());
        }

        public IZView OpenView<TView, TController>()
            where TView : class, IZView
            where TController : class, IZController
        {
            var view = ObjectFactory.GetFactory(FactoryType.Temp).CreateObject<TView>() as IZView;
            var controller = ObjectFactory.GetFactory(FactoryType.Temp).CreateObject<IZController>() as IZController;

            if (!viewDic.ContainsKey(view))
            {
                //加载View资源
                //初始化
                view.Init();
                controller.Enabled();
            }
            view.Show();
            return view;
        }

        public void HideView<TView>(TView view) where TView : IZView
        {
            if (!viewDic.ContainsKey(view)) return;
            view.Hide();
        }

        public void BindProperty<T>(IZView view, string propName, OnValueChangedHandler<T> handler)
        {
            if (!viewDic.ContainsKey(view)) return;
            var prop = viewDic[view].ViewModel.GetType().GetProperty(propName) as PropertyBase<T>;
            prop.ValueChangedChangeEvent += handler;
        }

        public void UnBindProperty<T>(IZView view, string propName, OnValueChangedHandler<T> handler)
        {
            if (!viewDic.ContainsKey(view)) return;
            var prop = viewDic[view].ViewModel.GetType().GetProperty(propName) as PropertyBase<T>;
            prop.ValueChangedChangeEvent -= handler;
        }

        public void ExcuteCommand(IZView view, int command, params object[] param)
        {
            if (!viewDic.ContainsKey(view)) return;
            var controller = viewDic[view];
            controller.ExecuteCommand(command, param);
        }

        private IEnumerator CheckCloseView()
        {
            if (viewDic.Count == 0) yield return null;
            yield return new WaitForSeconds(CHECK_CLOSE_TIME);
            foreach (var view in viewDic.Keys)
            {
                if(view.IsHide)
                {
                    closeViewQueue.Enqueue(view);
                }
            }
        }

        private void Update()
        {
            while (closeViewQueue.Count != 0)
            {
                var view = closeViewQueue.Dequeue();
                var controller = viewDic[view];
                view.Close();
                controller.Disabled();
                viewDic.Remove(view);
                controllerDic[controller].Remove(view);
                if (controllerDic.Count <= 0)
                {
                    controllerDic.Remove(controller);
                }
            }
        }
    }
}