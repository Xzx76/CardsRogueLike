using System;
using System.Collections;
using System.Collections.Generic;
using Asset;
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
        private Dictionary<int, Sprite> cardQuilitySprites;
        private Dictionary<int, CardInfo> cardsInfo;
        public CardAsset[] Cards;
        public override void Init()
        {
            _language = new Dictionary<string, string>();
            cardQuilitySprites = new();
            cardsInfo = new();
            Cards = new CardAsset[3];
            LoadCardAsset();
            LoadCardsInfo();
            LoadCardQuilitySprite();
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
        public Sprite GetCardQuility(int quility)
        {
            if (cardQuilitySprites.TryGetValue(quility,out Sprite img))
                return img;
            return null;
        }
        private void LoadCardAsset()
        {
            AssetManager.Instance.LoadAssetAsync<CardAsset>("Hit",asset=>
            {
                Cards[0] = asset;
            });            
            AssetManager.Instance.LoadAssetAsync<CardAsset>("AexHit", asset=>
            {
                Cards[1] = asset;
            });
            AssetManager.Instance.LoadAssetAsync<CardAsset>("Stronger", asset =>
            {
                Cards[2] = asset;
            });
            AssetManager.Instance.CreatePoolAsync("CardPrefab");
        }
        private void LoadCardQuilitySprite()
        {
            for (int i = 0; i < 3; i++)
            {
                int idx = i;
                AssetManager.Instance.LoadAssetAsync<Sprite>("CardQuility" + idx, sprite=>
                   {
                       cardQuilitySprites.Add(idx, sprite);
                   });
            }
            
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
        private void LoadCardsInfo()
        {
            LoadCheck check = new LoadCheck("卡牌信息");
            GameLaunch.AddloadCheck(check);

            AssetManager.Instance.LoadAssetAsync<TextAsset>(Constants.Cfg_CardInfo, asset =>
            {
                List<Cfg_Cs_CardInfo> result = JsonMapper.ToObject<List<Cfg_Cs_CardInfo>>(asset.text);
                int count = result.Count;
                for (int i = 0; i < count; i++)
                {
                    CardInfo item = new()
                    {
                        CardId = result[i].CardId,
                        CardType = (CardType)result[i].CardType,
                        CardQuality = (CardQuality)result[i].CardQuility,
                        CardSprite = result[i].CardSprite,
                        BaseExpend = result[i].BaseExpend,
                        BuffValue = result[i].BuffValue,
                        BuffRound = result[i].BuffRound,
                        CardName = result[i].CardName,
                        Target = result[i].Target,
                        UpgradeCardId = result[i].UpgradeCardId,
                        AdditionTypes = result[i].AdditionTypes,
                        BaseAddition = result[i].BaseAddition,
                        CurrencyExpend = result[i].BaseExpend,
                        CurrencyAddition = result[i].BaseAddition,
                        EffectNames = result[i].EffectNames
                    };
                    cardsInfo.Add(item.CardId, item);
                }
                //加载完成反馈
                check.SetReady();
            });
        }
    }
}
