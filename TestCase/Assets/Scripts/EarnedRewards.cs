using System.Collections.Generic;
using UnityEngine;

namespace TestCase.Gameplay.UI
{
    public class EarnedRewards : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private Reward _rewardPrefab;
        
        private List<Reward> _rewardList;

        public void AddReward(Reward rewardToBeAdded, int value)
        {
            _rewardList ??= new List<Reward>();

            foreach (var earnedReward in _rewardList)
            {
                if(rewardToBeAdded.CompareRewards(earnedReward, rewardToBeAdded))
                {
                    earnedReward.UpdateEarnRewardCount(value);
                    return;
                }
            }
            
            var reward = Instantiate(_rewardPrefab, _content);
            reward.SetRewardData(rewardToBeAdded.RewardData,true);
            _rewardList.Add(reward);
        }
        
        public void ClearRewards()
        {
            if (_rewardList == null) return;
            
            _rewardList.ForEach(x => x.RewardData.RewardValue = 0);
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
            _rewardList.Clear();
        }
    }
}