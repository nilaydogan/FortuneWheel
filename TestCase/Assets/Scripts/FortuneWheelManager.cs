using TestCase.Gameplay.Data;
using UnityEngine;

namespace TestCase.Gameplay
{
    public class FortuneWheelManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private WheelRewards _wheelRewards;
        [SerializeField] private RewardsManager _rewardsManager;
        [SerializeField] private ZoneManager _zoneManager;
        
        private int _currentZone;

        #endregion

        #region Unity Methods

        private void Start()
        {
            _zoneManager.Initialize();
            _rewardsManager.Initialize();
            _rewardsManager.SetUpRewards(_wheelRewards.GetRewardDataList(_zoneManager.CurrentZone));
        }

        #endregion

        #region Private Methods


        #endregion
    }
}