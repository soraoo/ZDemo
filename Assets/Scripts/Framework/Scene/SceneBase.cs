using Task = System.Threading.Tasks.Task;

namespace ZXC
{
    public class SceneBase
    {
        /// <summary>
        /// 场景ID
        /// </summary>
        public int SceneId { get; }

        public SceneBase(int sceneId)
        {
            SceneId = sceneId;
        }
        
        public void Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }
    }
}