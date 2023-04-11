/*------------------------------
//Copyright (C) moshi
//Author:zdy
//FileCreateDate:2022-08-02 星期二 17:15
//Desc: 
------------------------------*/
using TMPro;
using UnityEngine;

namespace TocClient
{
    public class UITimer : MonoBehaviour
    {
        public TMP_Text TextView;

        private float _curTimer = -1;
        private int lastUpdateTimer = -1;

        private void OnEnable()
        {
            _curTimer = 0;
            lastUpdateTimer = -1;
            GameLaunch.UpdateLoop += updateTimer;
        }

        void updateTimer()
        {
            _curTimer += Time.deltaTime;
            if ((int)_curTimer > lastUpdateTimer)
            {
                lastUpdateTimer = (int)_curTimer;
                var min = (int)_curTimer / 59;
                var sec = (int)_curTimer - min * 59;
                
                TextView.text = $"{min.ToString().PadLeft(2,'0')}:{sec.ToString().PadLeft(2,'0')}";
            }
        }

        private void OnDisable()
        {
            GameLaunch.UpdateLoop -= updateTimer;
        }
    }
}