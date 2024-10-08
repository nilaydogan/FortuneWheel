using TestCase.Gameplay.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestCase.Gameplay
{
    public class Reward : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _valueText;
        
        public void SetRewardData(WheelRewards.RewardData rewardData)
        {
            _icon.sprite = rewardData.RewardSprite;
            _valueText.text = rewardData.RewardValue > 1 ? "x" + rewardData.RewardValue : "";
        }
    }
}