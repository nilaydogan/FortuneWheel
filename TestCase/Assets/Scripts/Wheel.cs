using System.Collections.Generic;
using System.Linq;
using TestCase.Gameplay.Data;
using UnityEngine;

namespace TestCase.Gameplay.UI
{
    public class Wheel : MonoBehaviour
    {
        #region Fields

        [SerializeField] private RectTransform _rewardPrefab;

        private List<Reward> _rewardsList;
        private float _radius = 145f;

        #endregion

        #region Public Methods

        public void Initialize()
        {
            CreateRewards();
            PlaceRewardsOnWheel();
        }

        public void SetUpRewards(List<WheelRewards.RewardData> rewardDataList)
        {
            for (var i = 0; i < rewardDataList.Count; i++)
            {
                var rewardData = rewardDataList.ElementAt(i);
                _rewardsList[i].SetRewardData(rewardData, false);
            }
        }

        public int GetRewardCount()
        {
            return _rewardsList.Count;
        }
        
        public Reward GetReward(int index)
        {
            return _rewardsList[index];
        }

        #endregion

        #region Private Methods

        private void CreateRewards()
        {
            _rewardsList = new List<Reward>();
            for (var i = 0; i < 8; i++)
            {
                var rewardImage = Instantiate(_rewardPrefab, transform).GetComponent<Reward>();
                _rewardsList.Add(rewardImage);
            }
        }

        private void PlaceRewardsOnWheel()
        {
            var angleStep = 360f / _rewardsList.Count;
            for (var i = 0; i < _rewardsList.Count; i++)
            {
                var angle = i * angleStep;
                var x = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;
                var y = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
                _rewardsList[i].RectTransform.anchoredPosition = new Vector2(x, y);
                _rewardsList[i].RectTransform.localRotation = Quaternion.Euler(0, 0, -angle);
            }
        }

        #endregion
    }
}