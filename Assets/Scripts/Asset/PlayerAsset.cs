using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Asset
{
    [CreateAssetMenu(fileName = "NEW PlayerAsset", menuName = "MyAsset/PlayerAsset", order = 0)]
    public class PlayerAsset : ScriptableObject
    {

        public int maxHp;
        public int currentHp;
        public int strength;
        public int agility;
        private static PlayerAsset _instance;
        public static PlayerAsset Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.FindObjectsOfTypeAll<PlayerAsset>().FirstOrDefault(); ;
                }
                if (_instance) return _instance;
                _instance = CreateInstance<PlayerAsset>();
                _instance.hideFlags = HideFlags.DontSave;

                return _instance;
            }
            private set => _instance = value;
        }

    }
}

