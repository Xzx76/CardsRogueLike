/*------------------------------
//Copyright (C) moshi
//Author:liyk
//Date:2022/6/29 17:16:37
//Desc: 
------------------------------*/

namespace TocClient
{
    public class LoadCheck
    {
        private bool _isReady=false;

        public LoadCheck()
        {
        }
        public LoadCheck(string name)
        {
            this.Name = name;
        }
        public void SetReady()
        {
            _isReady = true;
        }
        public string Name { get; set; }
        public bool State { get => _isReady; }
    }
}
