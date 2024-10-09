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
        [SerializeField] private Transform _wheelTransform;
        [SerializeField] private Sprite _wheelSilverSkin, _wheelGoldSkin;
        
        private Button _spinButton;
        private int _currentZone;
        private float _spinDuration = 5f;
        private float _initialSpeed = 500f; 

        #endregion

        #region Unity Methods

        private void Start()
        {
            _zoneManager.Initialize();
            
            _wheel.Initialize();
            _wheel.SetUpRewards(_wheelRewards.GetRewardDataList(_zoneManager.CurrentZone));
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
            var extraRotations = Random.Range(1, 5);
            var totalRotation = -(360f * extraRotations + targetAngle);

            _wheelTransform
                .DORotate(new Vector3(0, 0, totalRotation), _spinDuration, RotateMode.FastBeyond360)
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
            ToggleSpinButton(false);
            SpinWheel();
        }

        private void ToggleSpinButton(bool isEnabled)
        {
            _spinButton.interactable = isEnabled;
        }

        #endregion
    }
}