using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Asset
{
    /// <summary>
    /// 卡牌类型
    /// </summary>
    public enum CardType
    {
        Attack,
        Skill,
    }
    public enum CardQuality
    {
        Write,
        Blue,
        Yellow
    }

    [Serializable]
    public class DictData
    {
        public char key;
        public int data;

        public DictData(char key, int data)
        {
            this.key = key;
            this.data = data;
        }
    }

    [CreateAssetMenu(fileName = "NewCard", menuName = "MyAsset/CardAsset", order = 0)]
    public class CardAsset : ScriptableObject
    {
        [BoxGroup("材质")] public CardType cardType;
        [BoxGroup("材质")] public CardQuality cardQuality;
        [BoxGroup("材质")] [ShowAssetPreview()] public Sprite cardSprite;

        [BoxGroup("数据")]
        [Tooltip("消耗")]
        public int Expend;

        [BoxGroup("数据")] public string cardName;

        [BoxGroup("数据")] [ResizableTextArea] public string express;
        [BoxGroup("数据")] [ResizableTextArea] public string desc;

        [BoxGroup("数据"), Label("基础数据")] [ReorderableList] public DictData[] dictData;
        [BoxGroup("数据"), Label("玩家加成数据")] [ReorderableList] public List<DictData> dictAddData;
        [BoxGroup("数据"), Label("升级卡牌")] public CardAsset Upgrade;
        [BoxGroup("数据"), Label("卡牌效果")] public List<CardEffect> Effects;
        [BoxGroup("数据"), Label("卡牌效果文字")] public List<string> EffectNames;
        [BoxGroup("数据"), Label("目标")] public string Target;

        Action _cardAttrChangeCb;
        /// <summary>
        /// 写入配置
        /// </summary>
        [Button("写入配置")]
        public void InitCardAsset()
        {
            desc = string.Empty;
            //字典方式
            foreach (var i in express)
            {
                var currChar = i + "";
                foreach (var t in dictData)
                {
                    if (!t.key.Equals(i)) continue;

                    var addValue = 0;    //加成值
                    //判断该数据是否需要被加成
                    if (t.key >= 'A' && t.key <= 'Z')
                    {
                        addValue = dictAddData.FirstOrDefault(m => m.key == t.key).data;
                    }
                    currChar = t.data + addValue + "";
                    break;
                }

                desc += currChar;
            }
            _cardAttrChangeCb?.Invoke();
        }
        /// <summary>
        /// 更新卡牌
        /// </summary>
        [Button("写入玩家加成数据")]
        public void UpdateCard()
        {
            dictAddData.Clear();
            dictAddData.Add(new DictData('X', PlayerAsset.Instance.strength));
            dictAddData.Add(new DictData('Y', PlayerAsset.Instance.agility));
            //X-玩家力量 Y-玩家敏捷
            //Debug.Log(PlayerAsset.Instance.maxHp + "\t" + PlayerAsset.Instance.currentHp + "\t" + PlayerAsset.Instance.strength);
            //重写
            InitCardAsset();
        }
        public void SetChangeEvent(System.Action cb)
        {
            _cardAttrChangeCb = cb;
        }
        [Button("写入卡牌效果")]
        public void SetEffect()
        {
            for (int i = 0; i < EffectNames.Count ; i++)
            {
                var effect = ScriptableObject.CreateInstance(Type.GetType(EffectNames[i])) as CardEffect;
                if (effect != null)
                {
                    effect.hideFlags = HideFlags.HideInHierarchy;
                    effect.Card = this;
                    Effects.Add(effect);
                    AssetDatabase.AddObjectToAsset(effect, this);
                }
            }

        }
    }
}
