namespace ZXC.UI
{
    /// <summary>
    /// Presenter接口
    /// </summary>
    /// <typeparam name="V">View</typeparam>
    public interface IPresenter<in V> where V : IView
    {
        /// <summary>
        /// 绑定View
        /// </summary>
        /// <param name="view">View</param>
        void BindView(V view);
        
        /// <summary>
        /// 解绑View
        /// </summary>
        void UnBindView();
    }
}