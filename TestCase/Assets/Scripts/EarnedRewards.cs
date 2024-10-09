using System.Collections.Generic;
using UnityEngine;

namespace TestCase.Gameplay.UI
{
    public class EarnedRewards : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private Reward _rewardPrefab;
        
        //private Dictionary<Reward,Reward> _rewardCountDictionary;
        private List<Reward> _rewardList;

        public void AddReward(Reward rewardToBeAdded)
        {
            _rewardList ??= new List<Reward>();
            
            if (_rewardList.Contains(rewardToBeAdded))
            {
                rewardToBeAdded.UpdateEarnRewardCount(rewardToBeAdded.RewardData.RewardValue);
            }
            else
            {
                var reward = Instantiate(_rewardPrefab, _content);
                reward.SetRewardData(rewardToBeAdded.RewardData, true);
                _rewardList.Add(reward);
            }
        }
        
        public void ClearRewards()
        {
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
            _rewardList.Clear();
        }
        
        //todo: add a method to update the count of a reward
    }
}