using TestCase.Gameplay.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestCase.Gameplay
{
    public class Reward : MonoBehaviour
    {
        public WheelRewards.RewardData RewardData => _data;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _valueText;
        
        private int _count;
        private WheelRewards.RewardData _data;
        
        public void SetRewardData(WheelRewards.RewardData rewardData, bool hasEarned = false)
        {
            _data = rewardData;
            _icon.sprite = _data.RewardSprite;
            _count = _data.RewardValue;
            _valueText.text = _count > 1 ? (!hasEarned ? "x" : "") + _count : "";
        }
        public void UpdateEarnRewardCount(int count)
        {
            _valueText.text =  (_count + count).ToString();
        }
    }
}