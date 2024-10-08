using System.Collections.Generic;
using DG.Tweening;
using TestCase.Gameplay.Data;
using UnityEngine;
using UnityEngine.UI;

namespace TestCase.Gameplay
{
    public class FortuneWheelManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private WheelRewards _wheelRewards;
        [SerializeField] private RewardsManager _rewardsManager;
        [SerializeField] private ZoneManager _zoneManager;
        [SerializeField] private Transform _wheel;
        
        private Button _spinButton;
        
        private int _currentZone;
        
        private List<WheelRewards.RewardData> _currentRewardDataList;
        private float _duration = 5f;
        private float _initialSpeed = 500f; 

        #endregion

        #region Unity Methods

        private void Start()
        {
            _zoneManager.Initialize();
            _rewardsManager.Initialize();
            
            _currentRewardDataList = _wheelRewards.GetRewardDataList(_zoneManager.CurrentZone);
            _rewardsManager.SetUpRewards(_currentRewardDataList);
        }
        
        private void OnValidate()
        {
            if (_spinButton == null)
            {
                _spinButton = GetComponentInChildren<Button>();

                if (_spinButton != null)
                {
                    Debug.Log("Button reference set automatically.");
                    
                    _spinButton.onClick.RemoveAllListeners();
                    _spinButton.onClick.AddListener(OnSpinClicked);
                }
                else
                {
                    Debug.LogWarning("Button reference not found.");
                }
            }
        }

        #endregion

        #region Private Methods

        private void SpinWheel()
        {
            var targetRewardIndex = Random.Range(0, _currentRewardDataList.Count);
            var anglePerReward = 360f / _currentRewardDataList.Count;
            var targetAngle = targetRewardIndex * anglePerReward;
            var extraRotations = Random.Range(1, 5);
            var totalRotation = -(360f * extraRotations + targetAngle);

            _wheel
                .DORotate(new Vector3(0, 0, totalRotation), _duration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad);
        }

        private void UpdateWheelRewards()
        {
            
        }
        
        private void UpdateCurrentZone()
        {
            
        }
        
        private void OnSpinClicked()
        {
            SpinWheel();
        }

        #endregion
    }
}