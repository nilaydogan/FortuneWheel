using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace TestCase.Gameplay.Data
{
    [CreateAssetMenu(menuName = "TestCase/Data/WheelRewards")]
    public class WheelRewards : ScriptableObject
    {
        #region Fields
        
        [SerializeField] private ZoneRewardData _zoneRewardDataList;

        #endregion
        
        #region Public Methods
        
        public List<RewardData> GetRewardList(ZoneManager.ZoneType zoneType)
        {
            var random = new Random();
            List<RewardData> randomizedList = new List<RewardData>();

            switch (zoneType)
            {
                case ZoneManager.ZoneType.Regular:
                    // Get 7 random regular rewards and insert a bomb
                    randomizedList = GetRandomRewards(RewardType.Regular, 7, _zoneRewardDataList.RewardList, random);
                    InsertBombReward(randomizedList, random, _zoneRewardDataList.RewardList);
                    break;
            
                case ZoneManager.ZoneType.Safe:
                    // Combine 4 regular and 4 safe zone rewards
                    randomizedList = GetRandomRewards(RewardType.Regular, 4, _zoneRewardDataList.RewardList, random);
                    randomizedList.AddRange(GetRandomRewards(RewardType.SafeZoneReward, 4, _zoneRewardDataList.RewardList, random));
                    break;
            
                case ZoneManager.ZoneType.Super:
                    // Combine 3 regular and 5 super zone rewards
                    randomizedList = GetRandomRewards(RewardType.Regular, 3, _zoneRewardDataList.RewardList, random);
                    randomizedList.AddRange(GetRandomRewards(RewardType.SuperZoneReward, 5, _zoneRewardDataList.RewardList, random));
                    break;
            }
            
            return randomizedList;
        }

        public List<RewardData> GetRewardList(ZoneManager.ZoneType zoneType, bool hasWon, int currentZone)
        {
            var rewards = GetRewardList(zoneType);

            if (hasWon)
            {
                foreach (var rewardData in rewards)
                {
                    var newAmount = rewardData.InitialValue + rewardData.IncreaseAmount * currentZone;
                    rewardData.RewardValue = newAmount;
                }
            }
            else
            {
                rewards.ForEach(x => x.RewardValue = x.InitialValue);
            }
            
            return rewards;
        }
        
        private List<RewardData> GetRandomRewards(RewardType rewardType, int count, List<RewardData> rewardList, Random random)
        {
            return rewardList
                .FindAll(x => x.RewardType == rewardType)
                .OrderBy(x => random.Next())
                .Take(count)
                .ToList();
        }
        
        private void InsertBombReward(List<RewardData> rewardList, Random random, List<RewardData> fullRewardList)
        {
            var bombReward = fullRewardList.Find(x => x.RewardType == RewardType.Bomb);
            var randomIndex = random.Next(0, rewardList.Count + 1);
            rewardList.Insert(randomIndex, bombReward);
        }

        #endregion

        #region Additional Classes/Structs
        
        [Serializable]
        public class ZoneRewardData
        {
            public List<RewardData> RewardList;
        }

        [Serializable]
        public class RewardData
        {
            public RewardType RewardType;
            public Sprite RewardSprite;
            public int InitialValue;
            public int RewardValue;
            public int IncreaseAmount;
            public Vector2 RewardSize;
            //maybe one-time rewards?
            //maybe percentage of the reward to be on the wheel?
        }
        
        public enum RewardType
        {
            Bomb,
            Regular,
            SuperZoneReward,
            SafeZoneReward
        }

        #endregion
    }
}