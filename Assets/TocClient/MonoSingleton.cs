using UnityEngine;

namespace TocClient
{
    /// <summary>
    /// 单例（MonoBehaviour）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
                return instance;
            }
        }

        public T GetSelf()
        {
            return instance;
        }

        public virtual void Awake()
        {
            instance = this as T;
        }

        public virtual void Init() { }
    }

}
