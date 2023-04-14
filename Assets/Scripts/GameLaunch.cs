using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TocClient
{
    public class GameLaunch : MonoSingleton<GameLaunch>
    {
        public static Action UpdateLoop;
        public static Action FixedUpdateLoop;
        public static Action LateUpdateLoop;

        private bool _assetReady = false;
        private bool _initReady = false;
        private static LoadCheck[] _loadComplete = null;
        private static int _checkCount = 0;
        private bool _isGameStart;
        public Transform BattleRoot;
        public void SetInitReady()
        {
            _initReady = true;
        }

        /// <summary>
        /// support float read
        /// </summary>
        void registerFloat()
        {
            float Importer(double obj)
            {
                return (float)obj;
            }
            JsonMapper.RegisterImporter((ImporterFunc<double, float>)Importer);
        }

        public override void Awake()
        {
            base.Awake();
            //LitJson float
            registerFloat();
            DontDestroyOnLoad(gameObject);
            _loadComplete = new LoadCheck[10];
            _checkCount = 0;
            //初始化游戏框架：资源管理、音效管理、特效管理、UI管理
            gameObject.AddComponent<AssetManager>();
            gameObject.AddComponent<UIManager>();
            gameObject.AddComponent<MsgSystem>();
            gameObject.AddComponent<PairMgr>();
            //gameObject.AddComponent<AudioManager>();
            StartCoroutine(nameof(CheckHotUpdate));

        }
        public void DoCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        IEnumerator CheckHotUpdate()
        {
            //版本控制，检查资源更新
            //TODO

            //加载UI管理器
            AsyncOperationHandle handle = AssetManager.Instance.InstantiateWithOpr("Prefab_UI_UIRoot");
            yield return handle;
            GameObject uiRoot = handle.Result as GameObject;
            DontDestroyOnLoad(uiRoot);
            Transform root = uiRoot.transform;
            AsyncOperationHandle handle1 = AssetManager.Instance.InstantiateWithOpr("Battle_Root");
            yield return handle1;
            GameObject battleRoot = handle1.Result as GameObject;
            BattleRoot = battleRoot.transform;
            DontDestroyOnLoad(battleRoot);
            //AssetManager.Instance.Add(TransName.UIRoot, root);
            //AssetManager.Instance.Add(TransName.UICamera, root.Find("UICamera"));
            //UIFormManager.Instance.Init(new FormFactory(), root, root.Find("NormalRoot"), root.Find("UnderRoot"), root.Find("PopupRoot"));

            //资源加载
            GameInit();
            while (!_initReady)
            {
                yield return 0;
            }
            //资源加载完成，开始进入游戏
            EnterGame();
            yield break;
        }

        public void GameInit()
        {
            //类型管理器初始化
            PairMgr.Instance.LoadAsync();
            StartCoroutine("PerLoad");

        }

        IEnumerator PerLoad()
        {
            yield return new WaitUntil(CheckPreLoad);
            //初始化完成
            _initReady = true;
            yield break;
        }

        //检查前置加载是否已经完成
        private bool CheckPreLoad()
        {
            for (int i = 0; i < _checkCount; i++)
            {
                if (_loadComplete[i] != null && !_loadComplete[i].State)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 添加前置加载检测
        /// </summary>
        /// <param name="check"></param>
        public static void AddloadCheck(LoadCheck check)
        {
            //存储数组不够时增加10
            if (_loadComplete.Length <= _checkCount)
            {
                LoadCheck[] temp = new LoadCheck[_checkCount + 10];
                for (int i = 0; i < _checkCount; i++)
                {
                    temp[i] = _loadComplete[i];
                }
                _loadComplete = temp;
            }
            _loadComplete[_checkCount] = check;
            _checkCount++;
        }
        public void EnterGame()
        {
            _isGameStart = true;
            _loadComplete = null;
            _checkCount = 0;
            //进入主菜单
            UIManager.Instance.PushPanel(Constants.Form_MainMenu);
        }
        private void Update()
        {
            if (_isGameStart)
                UpdateLoop?.Invoke();
        }
        private void FixedUpdate()
        {
            if (_isGameStart)
                FixedUpdateLoop?.Invoke();
        }

        private void LateUpdate()
        {
            if (_isGameStart)
                LateUpdateLoop?.Invoke();
        }
    }
}
