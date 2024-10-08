using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestCase.Gameplay.Data
{
    [CreateAssetMenu(menuName = "TestCase/Data/WheelRewards")]
    public class WheelRewards : ScriptableObject
    {
        #region Fields
        
        //[SerializeField] private bool _isSafeZone;
        [SerializeField] private List<ZoneRewardData> _zoneRewardDataList;

        #endregion
        
        #region Public Methods
        public List<RewardData> GetRewardDataList(int index)
        {
            return _zoneRewardDataList[index].RewardList;
        }
        
        #endregion

        #region Additional Classes/Structs
        
        [Serializable]
        public class ZoneRewardData
        {
            public List<RewardData> RewardList;
        }

        [Serializable]
        public struct RewardData
        {
            public RewardName RewardName;
            public Sprite RewardSprite;
            public int RewardValue;
            public Vector2 RewardSize;
        }
        
        public enum RewardName
        {
            
        }

        #endregion
    }
}