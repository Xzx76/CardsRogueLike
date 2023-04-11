using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using TocClient;
using UnityEngine;

namespace TocClient
{
    public class PairMgr : MonoSingleton<PairMgr>
    {
        private bool _hasInit;
        private LoadCheck[] _loadComplete = new LoadCheck[10];
        private int _checkCount = 0;
        private Dictionary<string, string> _language;
        private Dictionary<string, MapInfo> _mapInfo;

        public override void Init()
        {
            _language = new Dictionary<string, string>();
            //初始化状态
            _hasInit = true;
        }
        public void LoadAsync()
        {
            if (!_hasInit)
                Init();
            //加载配置文件(需要提前加载语言包)
            //LoadLanguage(() =>
            //{

            //    LoadHeroType();
            //    LoadHeroQuality();
            //    LoadHeroVocation();
            //    LoadHeroRace();
            //    LoadAckType();
            //    LoadHeroTarget();
            //    LoadItemQuality();
            //});
        }
        /// <summary>
        /// 获取语言字符串
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetLanguageString(string key)
        {
            if (!_language.TryGetValue(key, out string val))
                return key;
            return val;
        }
        /// <summary>
        /// 获取英雄类型显示名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public string HeroTypeName(int typeId)
        //{
        //    if (!_heroType.TryGetValue(typeId, out HeroType val))
        //        return null;
        //    return GetLanguageString(val.Name);
        //}

        ////语言包
        //private void LoadLanguage(Action action)
        //{
        //    AssetManager.Instance.LoadAssetAsync<TextAsset>(Constants.Cfg_Language, asset =>
        //    {
        //        List<Cfg_Language> result = JsonMapper.ToObject<List<Cfg_Language>>(asset.text);
        //        int count = result.Count;
        //        if (_language == null)
        //            _language = new Dictionary<string, string>();
        //        for (int i = 0; i < count; i++)
        //        {
        //            Cfg_Language item = result[i];
        //            _language[item.Id] = item.Langcn;
        //        }
        //        //加载完成反馈
        //        action?.Invoke();
        //    });
        //}
/*        private void LoadHeroVocation()
        {
            LoadCheck check = new LoadCheck("地图信息");
            GameLaunch.AddloadCheck(check);

            AssetManager.Instance.LoadAssetAsync<TextAsset>(Constants.Cfg_MapInfo, asset =>
            {
                List<MapInfo> result = JsonMapper.ToObject<List<MapInfo>>(asset.text);
                int count = result.Count;
                if (_mapInfo == null)
                    _mapInfo = new Dictionary<string, MapInfo>();
                for (int i = 0; i < count; i++)
                {
                    MapInfo item = result[i];
                    _mapInfo[item.MapName] = item;
                }
                //加载完成反馈
                check.SetReady();
            });
        }*/
    }
}
