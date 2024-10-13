using System.Collections.Generic;
using DG.Tweening;
using TestCase.Gameplay.Data;
using UnityEngine;

namespace TestCase.Gameplay.UI
{
    public class WinLosePopup : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _scrollView, _bombImage;
        [SerializeField] private Reward _finalRewardPrefab;

        public void ShowWinPopup(List<WheelRewards.RewardData> rewardDataList)
        {
            _scrollView.SetActive(true);
            _bombImage.SetActive(false);
            _canvasGroup.DOFade(1, .35f).SetEase(Ease.InSine);
            ShowFinalRewards(rewardDataList);
        }

        public void ShowLosePopup()
        {
            _scrollView.SetActive(false);
            _bombImage.SetActive(true);
            _canvasGroup.DOFade(1, .35f).SetEase(Ease.InSine);
        }
        
        public void HidePopup()
        {
            _scrollView.SetActive(false);
            _bombImage.SetActive(false);
            _canvasGroup.DOFade(0, .35f).SetEase(Ease.InSine);
        }

        private void ShowFinalRewards(List<WheelRewards.RewardData> rewardDataList)
        {
            if (rewardDataList == null)
                return;
            
            foreach (var rewardData in rewardDataList)
            {
                var reward = Instantiate(_finalRewardPrefab, _content);
                reward.SetFinalRewardData(rewardData);
            }
        }
    }
}