using ZXC.UI;

namespace ZXC.Presenter
{
    public abstract class PresenterBase<V> : IPresenter<V> where V : IView
    {
        protected V View { get; private set; }
        
        public void BindView(V view)
        {
            View = view;
        }

        public void UnBindView()
        {
            View = default(V);
        }
    }
}