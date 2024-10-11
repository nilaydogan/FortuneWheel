using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TestCase.Gameplay.Data;
using TestCase.Gameplay.UI;
using UnityEngine;
using UnityEngine.UI;

namespace TestCase.Gameplay
{
    public class FortuneWheelManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private WheelRewards _wheelRewards;
        [SerializeField] private Wheel _wheel;
        [SerializeField] private ZoneManager _zoneManager;
        [SerializeField] private EarnedRewards _earnedRewards;
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private Sprite _wheelSilverSkin, _wheelGoldSkin, _wheelRegularSkin;
        [SerializeField] private Sprite _indicatorSilverSkin, _indicatorGoldSkin, _indicatorRegularSkin;
        [SerializeField] private Image _indicatorImage, _wheelImage;
        
        private Button _spinButton;
        private float _spinDuration = 5f;
        private List<WheelRewards.RewardData> _currentRewards;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _zoneManager.Initialize();
            _wheel.Initialize();
            _currentRewards = _wheelRewards.GetRewardList(_zoneManager.GetZoneType(), false, _zoneManager.CurrentZone);
            _wheel.SetUpRewards(_currentRewards);
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
            var rewardCount = _wheel.GetRewardCount();
            
            //select a random reward to stop at
            var targetRewardIndex = Random.Range(0, rewardCount);
            var anglePerReward = 360f / rewardCount;
            var targetAngle = targetRewardIndex * anglePerReward;
            
            //add random extra rotations to make it look natural
            var extraRotations = Random.Range(3, 6);
            var totalRotation = -(extraRotations * 360f - targetAngle);

            var sequence = DOTween.Sequence()
                .Join(_wheelTransform
                .DORotate(new Vector3(0, 0, totalRotation), _spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad))
                .AppendInterval(.5f);

            sequence.OnComplete(() =>
            {
                var hasWon = GrantReward(targetRewardIndex, _currentRewards[targetRewardIndex].RewardValue);
                _zoneManager.UpdateCurrentZone(hasWon);
                UpdateWheelRewards(hasWon);
                UpdateWheelSkin();
                ToggleSpinButton(true);
            });
        }

        private void UpdateWheelRewards(bool hasWon)
        {
            _currentRewards.Clear();
            _currentRewards = _wheelRewards.GetRewardList(_zoneManager.GetZoneType(), hasWon, _zoneManager.CurrentZone);
            _wheel.SetUpRewards(_currentRewards);
        }
        
        private void UpdateWheelSkin()
        {
            var zoneType = _zoneManager.GetZoneType();

            switch (zoneType)
            {
                case ZoneManager.ZoneType.Super:
                    _wheelImage.sprite = _wheelGoldSkin;
                    _indicatorImage.sprite = _indicatorGoldSkin;
                    break;
                case ZoneManager.ZoneType.Safe:
                    _wheelImage.sprite = _wheelSilverSkin;
                    _indicatorImage.sprite = _indicatorSilverSkin;
                    break;
                case ZoneManager.ZoneType.Regular:
                    _wheelImage.sprite = _wheelRegularSkin;
                    _indicatorImage.sprite = _indicatorRegularSkin;
                    break;
            }
        }
        
        private void OnSpinClicked()
        {
            ToggleSpinButton(false);
            SpinWheel();
        }

        private void ToggleSpinButton(bool isEnabled)
        {
            _spinButton.interactable = isEnabled;
        }
        
        private bool GrantReward(int rewardIndex, int value)
        {
            //todo: show the reward with a popup
            //todo: check if its a bomb
            var earnedReward = _wheel.GetReward(rewardIndex);

            if (earnedReward.RewardData.RewardType == WheelRewards.RewardType.Bomb)
            {
                _earnedRewards.ClearRewards();
                return false;
            }
            
            _earnedRewards.AddReward(earnedReward, value);
            return true;
        }

        #endregion
    }
}