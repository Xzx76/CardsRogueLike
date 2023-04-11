namespace TocClient
{
    /// <summary>
    /// 单例基类
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    public class Singleton<T> where T : new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
        public virtual void Init() { }

    }
}