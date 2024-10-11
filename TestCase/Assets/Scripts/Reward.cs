using TestCase.Gameplay.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestCase.Gameplay
{
    public class Reward : MonoBehaviour
    {
        public WheelRewards.RewardData RewardData => _data;
        public RectTransform RectTransform => _rectTransform;
        
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private RectTransform _rectTransform, _iconRectTransform;
        
        private int _count;
        private WheelRewards.RewardData _data;
        
        public void SetRewardData(WheelRewards.RewardData rewardData, bool hasEarned)
        {
            _data = rewardData;
            _icon.sprite = _data.RewardSprite;
            _count = rewardData.RewardValue;
            _valueText.text = _count > 1 ? (!hasEarned ? "x" : "") + _count : "";

            if (!hasEarned)
            {
                _iconRectTransform.sizeDelta = _data.RewardSize != Vector2.zero ? _data.RewardSize : new Vector2(80, 80);
                
                _iconRectTransform.anchoredPosition = _count <= 1 ? new Vector2(0, -2.5f) : Vector2.zero;
            }
        }
        public void UpdateEarnRewardCount(int count)
        {
            _valueText.text =  (_count + count).ToString();
        }

        public bool CompareRewards(Reward reward1, Reward reward2)
        {
            return reward1._icon.sprite == reward2._icon.sprite;
        }
    }
}