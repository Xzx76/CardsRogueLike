using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TocClient
{
    public class ObjectPoolManager : MonoBehaviour
    {
        private static ObjectPoolManager _Instance;
        public static ObjectPoolManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<ObjectPoolManager>();
                }
                return _Instance;
            }
        }


        /// <summary>
        /// 对象池，需要的时候可以直接取
        /// </summary>
        private Dictionary<string, List<GameObject>> pool;


        /// <summary>
        /// 预制体
        /// </summary>
        private Dictionary<string, GameObject> prefabs;


        /// <summary>
        /// 初始化
        /// </summary>
        public void Awake()
        {
            if (pool == null)
                pool = new Dictionary<string, List<GameObject>>();
            if (prefabs == null)
                prefabs = new Dictionary<string, GameObject>();

            RegisterAll();
        }


        /// <summary>
        /// 注册可能需要的预制体
        /// </summary>
        /// <param name="key">path</param>
        public void Register(string key)
        {
            Addressables.LoadAssetAsync<GameObject>(key).Completed += (res) =>
            {
                //设定快速获取对象
                prefabs[key] = res.Result;
            };
        }

        /// <summary>
        /// 在初始化时，注册所有可能需要的预制体path，当然也可以在游戏中动态注册
        /// </summary>
        public void RegisterAll()
        {
            
            Register("Assets/Prefab/Card"); 
        }

        /// <summary>
        /// 增加预制体
        /// </summary>
        /// <param name="key">预制体的key值,也就是addressable的path值</param>
        /// <param name="num">预制体的数量</param>
        public void Add(string key, int num)
        {
            //如果已经拥有这个对象池
            if (pool.ContainsKey(key))
            {
                GameObject go;
                for (int i = 0; i < num; i++)
                {
                    go = Instantiate(prefabs[key]);
                    go.SetActive(false);
                    pool[key].Add(go);
                }
            }
            else
            {
                //新建一个对象池
                List<GameObject> goList = new List<GameObject>();
                pool.Add(key, goList);
                //创建对象
                GameObject go;

                //如果已经注册过
                if (prefabs.TryGetValue(key, out GameObject re))
                {
                    for (int i = 0; i < num; i++)
                    {
                        go = Instantiate(prefabs[key]);
                        go.SetActive(false);
                        pool[key].Add(go);
                    }
                }
                else
                {
                    Addressables.LoadAssetAsync<GameObject>(key).Completed += (res) =>
                    {
                        //设定快速获取对象
                        prefabs[key] = res.Result;

                        for (int i = 0; i < num; i++)
                        {
                            go = Instantiate(prefabs[key]);
                            go.SetActive(false);
                            pool[key].Add(go);
                        }
                    };
                }

            }
        }

        /// <summary>
        /// 找到第一个可用预制体
        /// </summary>
        /// <param name="key">预制体的key值,也就是addressable的path值</param>
        /// <returns></returns>
        public GameObject FindUsable(string key)
        {
            if (pool.ContainsKey(key))
            {
                foreach (GameObject item in pool[key])
                {
                    if (!item.activeSelf)
                        return item;
                }
                Debug.Log($"{key}的对象池数量不够");
            }
            else
            {
                Debug.Log($"{key}未添加对象池");
            }
            return null;
        }

        /// <summary>
        /// 创建一个对象
        /// </summary>
        /// <param name="key">预制体的key值,也就是addressable的path值</param>
        /// <param name="pos">位置</param>
        /// <param name="rot">旋转</param>
        /// <param name="parent">父类</param>
        /// <returns></returns>
        public GameObject Creat(string key, Vector3 pos, Vector3 rot, Transform parent = null)
        {
            GameObject tempGo = FindUsable(key);
            if (tempGo == null)
            {
                Debug.Log($"{key}的对象池数量不够");
                Add(key, 10);
                tempGo = FindUsable(key);
            }
            tempGo.transform.position = pos;
            tempGo.transform.rotation = Quaternion.Euler(rot);
            if (parent)
                tempGo.transform.SetParent(parent);
            tempGo.SetActive(true);
            return tempGo;
        }

        /// <summary>
        /// 创就一个对象，重载
        /// </summary>
        /// <param name="key">预制体的key值,也就是addressable的path值</param>
        /// <returns></returns>
        public GameObject Creat(string key)
        {
            GameObject tempGo = FindUsable(key);
            if (tempGo == null)
            {
                Debug.Log($"{key}的对象池数量不够");
                Add(key, 10);
                tempGo = FindUsable(key);
            }

            tempGo.SetActive(true);
            return tempGo;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        /// <param name="destoryGo"></param>
        public void Destory(GameObject destoryGo)
        {
            destoryGo.SetActive(false);
        }

        /// <summary>
        /// 将对象归入池中，进行延时销毁
        /// </summary>
        /// <param name="tempGo"></param>
        /// <param name="delay">时间</param>
        public void Destory(GameObject tempGo, float delay)
        {
            StartCoroutine(DelayDestory(tempGo, delay));
        }

        /// <summary>
        /// 延迟销毁
        /// </summary>
        /// <param name="destoryGO"></param>
        /// <param name="delay">时间</param>
        /// <returns></returns>
        private IEnumerator DelayDestory(GameObject destoryGO, float delay)
        {
            yield return new WaitForSeconds(delay);
            Destory(destoryGO);
        }

        /// <summary>
        /// 清空某类游戏对象
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            pool.Remove(key);
        }

        /// <summary>
        /// 清空池中所有游戏对象
        /// </summary>
        public void ClearAll()
        {
            pool.Clear();
        }

    }
}



