using System.Collections.Generic;
using TestCase.Gameplay;
using TestCase.Gameplay.Data;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private RectTransform _rewardPrefab;

    private List<RectTransform> _rewardImageRectTransforms;
    private float _radius = 145f;

    #endregion

    public void Initialize()
    {
        CreateRewards();
        PlaceImagesOnWheel();
    }

    public void SetUpRewards(List<WheelRewards.RewardData> rewardDataList)
    {
        for (var i = 0; i < rewardDataList.Count; i++)
        {
            _rewardImageRectTransforms[i].GetComponent<Reward>().SetRewardData(rewardDataList[i]);
        }
    }
    
    private void CreateRewards()
    {
        _rewardImageRectTransforms = new List<RectTransform>();
        for (var i = 0; i < 8; i++)
        {
            var rewardImage = Instantiate(_rewardPrefab, transform);
            _rewardImageRectTransforms.Add(rewardImage);
        }
    }
    private void PlaceImagesOnWheel()
    {
        var angleStep = 360f / _rewardImageRectTransforms.Count;
        for (var i = 0; i < _rewardImageRectTransforms.Count; i++)
        {
            var angle = i * angleStep;
            var x = Mathf.Sin(angle * Mathf.Deg2Rad) * _radius;
            var y = Mathf.Cos(angle * Mathf.Deg2Rad) * _radius;
            _rewardImageRectTransforms[i].anchoredPosition = new Vector2(x, y);
        }
    }
}