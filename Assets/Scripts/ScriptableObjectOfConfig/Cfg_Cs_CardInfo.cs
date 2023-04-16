using System;
using System.Collections.Generic;

namespace TocClient {
    public class Cfg_Cs_CardInfo
    {
        public int CardId{get;set;}
        public int CardType{get;set;}
        public int CardQuility{get;set;}
        public string CardSprite{get;set;}
        public int BaseExpend{get;set;}
        public int[] BuffValue{get;set;}
        public int BuffRound{get;set;}
        public string CardName{get;set;}
        public string Target{get;set;}
        public int UpgradeCardId{get;set;}
        public int[] AdditionTypes{get;set;}
        public int[] BaseAddition{get;set;}
        public string[] EffectNames{get;set;}
        public string Desc{get;set;}
    }
}
