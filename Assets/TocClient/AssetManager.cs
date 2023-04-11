using PathologicalGames;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace TocClient
{
    public class AssetManager : MonoSingleton<AssetManager>
    {
        private SpawnPool _spawnPool;
        private Transform _poolRoot;
        private Dictionary<string, string> _keyName;
        private Dictionary<string, Transform> _pools;

        public override void Awake()
        {
            base.Awake();
            _keyName = new Dictionary<string, string>();
            DontDestroyOnLoad(gameObject);
            _poolRoot = new GameObject("ItemPools").transform;
            _poolRoot.SetParent(transform);
            _spawnPool = _poolRoot.gameObject.AddComponent<SpawnPool>();
            _pools = new Dictionary<string, Transform>();
        }
        public void Add(string key, Transform trans)
        {
            if (_pools == null)
                _pools = new Dictionary<string, Transform>();
            _pools[key] = trans;
        }

        public bool Remove(string key)
        {
            if (_pools == null)
                _pools = new Dictionary<string, Transform>();
            return _pools.Remove(key);
        }

        public Transform GetTransform(string key)
        {
            if (_pools.TryGetValue(key, out Transform transform))
            {
                return transform;
            }
            return null;
        }
        /// <summary>
        /// 引用类资源加载（异步）
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="key"></param>
        /// <param name="action">回调</param>
        public void LoadAssetAsync<TObject>(string key, Action<TObject> action)
        {
            AsyncOperationHandle<TObject> handle = Addressables.LoadAssetAsync<TObject>(key);
            handle.Completed += (result) =>
            {
                action?.Invoke(result.Result);
                Addressables.Release(handle);
            };
        }
        public TObject LoadAsset<TObject>(string key)
        {
            return Addressables.LoadAssetAsync<TObject>(key).WaitForCompletion();
        }

        /// <summary>
        /// 引用类多资源加载（异步）
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="keys"></param>
        /// <param name="action"></param>
        public void LoadAssetsAsync<TObject>(IEnumerable keys, Action<IList<TObject>> action)
        {
            AsyncOperationHandle<IList<TObject>> handle = Addressables.LoadAssetsAsync<TObject>(keys, null, Addressables.MergeMode.Union);
            handle.Completed += result => {
                action?.Invoke(result.Result);
            };
        }
        public AsyncOperationHandle LoadSceneAsync(object key)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(key);
            return handle;
        }

        /// <summary>
        /// GameObject加载并实例化（异步）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action">回调</param>
        /// <param name="parent">父级</param>
        public void InstantiateAsync(string key, Action<GameObject> action, Transform parent = null)
        {
            Addressables.InstantiateAsync(key, parent).Completed += (handle) =>
            {
                action?.Invoke(handle.Result);
            };
        }

        /// <summary>
        /// 实例化加载并返回异步操作
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public AsyncOperationHandle InstantiateWithOpr(string key, Transform parent = null)
        {
            return Addressables.InstantiateAsync(key, parent);
        }
        //----Config-----
        public void GetConfig(string key, Action<byte[]> handle)
        {
            LoadAssetAsync<TextAsset>(key, asset => {
                handle?.Invoke(asset.bytes);
            });
        }

        //-----Pool------
        //--使用前需要先集中调用CreatePoolAsync添加子缓存池
        public void CreatePoolAsync(string key, int amount = 1, Action<string> callback = null)
        {
            if (_keyName.TryGetValue(key, out string name))
                callback?.Invoke(name);
            else
                AddPrefabPoolAsync(key, amount, callback);
        }
        public void CreatePool(string key, GameObject obj, int amount = 1)
        {
            if (!obj)
                return;
            Transform trans = obj.transform;
            if (!_keyName.ContainsKey(key))
            {
                //创建缓存
                _keyName[key] = trans.name;
                CreatPrefabPool(trans, amount);
            }
        }
        /// <summary>
        /// 创建缓存池
        /// </summary>
        /// <param name="keys">资源地址</param>
        /// <param name="amount">初始数量</param>
        /// <param name="callback">完成回调</param>
        public void CreatePoolsAsync(string[] keys, int amount = 1, Action<int> callback = null)
        {
            if (keys == null || keys.Length <= 0)
            {
                callback?.Invoke(0);
                return;
            }

            int len = keys.Length;
            List<string> tmpKeys = new List<string>(len);
            for (int i = 0; i < len; i++)
            {
                if (!_keyName.ContainsKey(keys[i]))
                {
                    tmpKeys.Add(keys[i]);
                }
            }

            if (tmpKeys.Count == 0)
            {
                callback?.Invoke(keys.Length);
                return;
            }
            LoadAssetsAsync<GameObject>(tmpKeys, result =>
            {
                int count = result.Count;
                for (int i = 0; i < count; i++)
                {
                    Transform trans = result[i].transform;
                    if (trans)
                    {
                        string key = tmpKeys[i];
                        string name = trans.name;
                        //创建缓存
                        _keyName[key] = name;
                        CreatPrefabPool(trans, amount);
                    }
                }
                callback?.Invoke(count);
            });
        }

        public Transform Spawn(string key, Transform parent = null)
        {
            if (_keyName.TryGetValue(key, out string name))
                return SpawnPool(name, parent);
            else
            {
                string transName = CreatePool(key, 1);
                return SpawnPool(transName, parent);
            }
        }
        /// <summary>
        /// 实体释放到缓存池
        /// </summary>
        /// <param name="trans">实体</param>
        /// <param name="isBack">释放回缓冲池Root节点</param>
        public void Despawn(Transform trans, bool isBack = false)
        {
            if (isBack)
                trans.SetParent(_poolRoot);
            _spawnPool?.Despawn(trans);
        }
        /// <summary>
        /// 实体延时释放
        /// </summary>
        /// <param name="trans">实体</param>
        /// <param name="delay">秒</param>
        public void Despawn(Transform trans, float delay, bool isBack = false)
        {
            _spawnPool.Despawn(trans, delay);
            if (isBack)
                trans.SetParent(_poolRoot);
        }
        private string CreatePool(string key, int amount = 1)
        {
            GameObject obj = Addressables.LoadAssetAsync<GameObject>(key).WaitForCompletion();
            Transform trans = obj.transform;
            string name = trans.name;
            //创建缓存
            _keyName[key] = name;
            CreatPrefabPool(trans, amount);
            return name;
        }
        private Transform SpawnPool(string name, Transform parent = null)
        {
            if (parent)
                return _spawnPool.Spawn(name, parent);
            else
                return _spawnPool.Spawn(name);
        }
        private void AddPrefabPoolAsync(string key, int amount, Action<string> callback = null)
        {
            //加载资源
            Addressables.LoadAssetAsync<GameObject>(key).Completed += handle => {
                Transform trans = handle.Result.transform;
                string name = trans.name;
                //创建缓存
                _keyName[key] = name;
                CreatPrefabPool(trans, amount);
                callback?.Invoke(name);
            };
        }
        private PrefabPool CreatPrefabPool(Transform prefab, int preloadAmount)
        {
            PrefabPool prefabPool = new PrefabPool(prefab);
            if (!_spawnPool._perPrefabPoolOptions.Contains(prefabPool))
            {
                //默认初始化Prefab数量
                prefabPool.preloadAmount = preloadAmount;
                //开启限制
                prefabPool.limitInstances = false;
                //无限取Prefab
                prefabPool.limitFIFO = true;
                //限制池子里最大的Prefab数量
                prefabPool.limitAmount = 20;
                //开启自动清理池子
                prefabPool.cullDespawned = true;
                //最终保留
                prefabPool.cullAbove = 10;
                //多久清理一次
                prefabPool.cullDelay = 30;
                //每次清理几个
                prefabPool.cullMaxPerPass = 5;
                //初始化内存池
                _spawnPool._perPrefabPoolOptions.Add(prefabPool);
                _spawnPool.CreatePrefabPool(prefabPool);
            }
            return prefabPool;
        }
    }
}
