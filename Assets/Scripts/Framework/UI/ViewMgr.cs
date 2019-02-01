using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZXC.UI;


namespace ZXC
{
    public class ViewMgr : ZMonoSingleton<ViewMgr>
    {
        private Dictionary<IZController, List<IZView>> controllerDic;
        private Dictionary<Type, IPresenter<IView>> viewDic;
        private Queue<IZView> closeViewQueue;
        
        private const float CHECK_CLOSE_TIME = 10f;
        
        protected override void AfterAwake()
        {
            controllerDic = new Dictionary<IZController, List<IZView>>();
            viewDic = new Dictionary<Type, IPresenter<IView>>();
            closeViewQueue = new Queue<IZView>();
        }

        private void Start()
        {
            //StartCoroutine(CheckCloseView());
        }

        public async Task OpenView<V, P>(string viewName)
            where V : class, IView
            where P : class, IPresenter<V>
        {
            ViewBase<V, P> view = null;
            if (!viewDic.ContainsKey(typeof(V)))
            {
                var viewObj = await ResMgr.instance.LoadAsset<GameObject>(viewName);
                var presenter = ObjectFactory.GetFactory(FactoryType.Temp).CreateObject<IPresenter<IView>>();
                view = viewObj.GetComponent<ViewBase<V, P>>() ?? viewObj.AddComponent<ViewBase<V, P>>();
                presenter.BindView(view);
                viewDic.Add(typeof(V), presenter);
            }
            view.Init();
        }

        public void HideView<TView>(TView view) where TView : IZView
        {
//            if (!viewDic.ContainsKey(view)) return;
//            view.Hide();
        }

        public void BindProperty<T>(IZView view, string propName, OnValueChangedHandler<T> handler)
        {
//            if (!viewDic.ContainsKey(view)) return;
//            var prop = viewDic[view].ViewModel.GetType().GetProperty(propName) as PropertyBase<T>;
//            prop.ValueChangedChangeEvent += handler;
        }

        public void UnBindProperty<T>(IZView view, string propName, OnValueChangedHandler<T> handler)
        {
//            if (!viewDic.ContainsKey(view)) return;
//            var prop = viewDic[view].ViewModel.GetType().GetProperty(propName) as PropertyBase<T>;
//            prop.ValueChangedChangeEvent -= handler;
        }

        public void ExcuteCommand(IZView view, int command, params object[] param)
        {
//            if (!viewDic.ContainsKey(view)) return;
//            var controller = viewDic[view];
//            controller.ExecuteCommand(command, param);
        }

//        private IEnumerator CheckCloseView()
//        {
//            if (viewDic.Count == 0) yield return null;
//            yield return new WaitForSeconds(CHECK_CLOSE_TIME);
//            foreach (var view in viewDic.Keys)
//            {
//                if(view.IsHide)
//                {
//                    closeViewQueue.Enqueue(view);
//                }
//            }
//        }

        private void Update()
        {
//            while (closeViewQueue.Count != 0)
//            {
//                var view = closeViewQueue.Dequeue();
//                var controller = viewDic[view];
//                view.Close();
//                controller.Disabled();
//                viewDic.Remove(view);
//                controllerDic[controller].Remove(view);
//                if (controllerDic.Count <= 0)
//                {
//                    controllerDic.Remove(controller);
//                }
//            }
        }
    }
}