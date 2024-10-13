using System.Collections.Generic;
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
        [SerializeField] private Button _spinButton, _exitButton, _quitButton, _retryButton;
        [SerializeField] private WinLosePopup _winLosePopup;
        [SerializeField] private GameObject _popupsParent;

        private float _spinDuration = 5f;
        private List<WheelRewards.RewardData> _currentRewards;
        private bool _isSpinning;
        private Sequence _spinSequence;

        #endregion
        
        private void Start()
        {
            #if !UNITY_EDITOR
            ValidateButtons();
            #endif
            _zoneManager.Initialize();
            _wheel.Initialize();

            _currentRewards = _wheelRewards.GetRewardList(_zoneManager.GetZoneType(), false, _zoneManager.CurrentZone);
            _wheel.SetUpRewards(_currentRewards);
        }

        #region Private Methods

        private void Reset()
        {
            //_zoneManager.Initialize();
            _wheel.Initialize();
            _wheelRewards.Initialize();
            _currentRewards = _wheelRewards.GetRewardList(_zoneManager.GetZoneType(), false, _zoneManager.CurrentZone);
            _wheel.SetUpRewards(_currentRewards);
            _earnedRewards.ClearRewards();
            _zoneManager.UpdateCurrentZone(false);
            UpdateWheelSkin();
            _spinButton.interactable = true;
        }
        
        private void OnValidate()
        {
            ValidateButtons();
        }
        
        private void ValidateButtons()
        {
            if (_spinButton != null)
            {
                _spinButton.onClick.RemoveAllListeners();
                _spinButton.onClick.AddListener(OnSpinClicked);
            }

            if (_exitButton != null)
            {
                _exitButton.onClick.RemoveAllListeners();
                _exitButton.onClick.AddListener(OnExitButtonClicked);
            }
            
            if(_quitButton != null)
            {
                _quitButton.onClick.RemoveAllListeners();
                _quitButton.onClick.AddListener(OnQuitButtonClicked);
            }
            
            if(_retryButton != null)
            {
                _retryButton.onClick.RemoveAllListeners();
                _retryButton.onClick.AddListener(OnRetryButtonClicked);
            }
        }

        #endregion

        #region Private Methods
        
        private void OnExitButtonClicked()
        {
            if(_isSpinning && _zoneManager.GetZoneType() == ZoneManager.ZoneType.Regular) return;
            
            _spinSequence.Kill();
            _popupsParent.SetActive(true);
            
            var finalRewards = _earnedRewards.GetRewardList();
            _winLosePopup.ShowWinPopup(finalRewards);
        }

        private void OnRetryButtonClicked()
        {
            Reset();
            _winLosePopup.HidePopup();
            _popupsParent.SetActive(false);
        }
        
        private void OnQuitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void SpinWheel()
        {
            Debug.Log("Spinning wheel");
            var rewardCount = _wheel.GetRewardCount();
            
            //select a random reward to stop at
            var targetRewardIndex = Random.Range(0, rewardCount);
            var anglePerReward = 360f / rewardCount;
            var targetAngle = targetRewardIndex * anglePerReward;
            
            //add random extra rotations to make it look natural
            var extraRotations = Random.Range(3, 6);
            var totalRotation = -(extraRotations * 360f - targetAngle);

            _spinSequence = DOTween.Sequence()
                .Join(_wheelTransform
                .DORotate(new Vector3(0, 0, totalRotation), _spinDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.OutQuad))
                .AppendInterval(.5f);

            _spinSequence.OnKill(() =>
            {
                _wheelTransform.DOKill();
                _wheelTransform.localEulerAngles = Vector3.zero;
            });

            _spinSequence.OnComplete(() =>
            {
                var hasWon = GrantReward(targetRewardIndex);
                _zoneManager.UpdateCurrentZone(hasWon);
                UpdateWheelRewards(hasWon);
                UpdateWheelSkin();
                ToggleSpinButton(true);
                _exitButton.interactable = true;
                _isSpinning = false;
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
            _isSpinning = true;
            ToggleSpinButton(false);
            _exitButton.interactable = _zoneManager.GetZoneType() != ZoneManager.ZoneType.Regular;
            SpinWheel();
        }

        private void ToggleSpinButton(bool isEnabled)
        {
            _spinButton.interactable = isEnabled;
        }
        
        private bool GrantReward(int rewardIndex)
        {
            var earnedReward = _wheel.GetReward(rewardIndex);

            if (earnedReward.RewardData.RewardType == WheelRewards.RewardType.Bomb)
            {
                _earnedRewards.ClearRewards();

                _popupsParent.SetActive(true);
                _winLosePopup.ShowLosePopup();

                return false;
            }
            
            _earnedRewards.AddReward(earnedReward);
            return true;
        }

        #endregion
    }
}