using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXC
{
    public class ZMonoSingleton<T> : MonoBehaviour where T : ZMonoSingleton<T>
    {
        protected static T instance = null;
        
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        ZLog.Warning(typeof(T).Name + " has more than 1 in scene!");
                        return instance;
                    }

                    if (instance == null)
                    {
                        string instanceName = typeof(T).Name;
                        GameObject instanceObj = GameObject.Find(instanceName) ?? new GameObject(instanceName);
                        instance = instanceObj.AddComponent<T>();
                        DontDestroyOnLoad(instanceObj);
                    }
                }
                return instance;
            }
        }

        void Awake()
        {
            AfterAwake();
        }

        protected virtual void AfterAwake()
        {
        }

        protected virtual void OnDestroy()
        {
            instance = null;
        }

        public virtual void Init(OnFinishedDelegate onFinished = null)
        {
        }

        public virtual IEnumerator Init()
        {
            yield return null;
        }
    }
}