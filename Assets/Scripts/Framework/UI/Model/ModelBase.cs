using ZXC.UI;

namespace ZXC
{
    public class ModelBase : IZModel
    {
        public void Init()
        {
            OnInit();
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnInit()
        {

        }

        protected virtual void OnDispose()
        {

        }
    }
}