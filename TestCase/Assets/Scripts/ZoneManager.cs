using System.Collections.Generic;
using TestCase.Gameplay.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TestCase.Gameplay
{
    public class ZoneManager : MonoBehaviour
    {
        #region Properties
        public int CurrentZone { get; private set; }

        #endregion

        #region Fields

        [SerializeField] private ScrollRect _zoneScrollView;
        [SerializeField] private RectTransform _indicator;
        [SerializeField] private Zone _levelPrefab;
        [SerializeField] private HorizontalLayoutGroup _horizontalLayout;
        [SerializeField] private TextMeshProUGUI _superZoneText, _safeZoneText;

        private List<Zone> _zoneList;
        private List<Zone> _activeLevels;

        private const float _minSpacing = 0, _maxSpacing = 2f, _levelMinWidth = 80f, _levelMaxWidth = 103f;
        private int _totalLevelsToCreate;
        private const int _everySafeZone = 5, _everySuperZone = 30;
        private int _usePadding = 2;

        #endregion

        public void Initialize()
        {
            _activeLevels = new List<Zone>();
            CenterIndicator();
            CurrentZone = 1;
            InitializeLevels();
            AdjustLevelSize();

            UpdateZoneIndicator();
            UpdateSafeZonesTexts();
        }
        
        public void UpdateCurrentZone(bool hasWon)
        {
            CurrentZone = hasWon ? CurrentZone + 1 : 1;
            if (!hasWon)
            {
                _usePadding = 2;
                ResetLevels();
            }
            
            UpdateZoneIndicator();
            UpdateSafeZonesTexts();
        }
        
        public ZoneType GetZoneType()
        {
            if (CurrentZone % _everySuperZone == 0)
            {
                return ZoneType.Super;
            }
            if (CurrentZone % _everySafeZone == 0)
            {
                return ZoneType.Safe;
            }

            return ZoneType.Regular;
        }

        #region Private Methods
        
        private void CenterIndicator()
        {
            var viewportWidth = _zoneScrollView.viewport.rect.width;
            var indicatorWidth = _indicator.rect.width;
            var indicatorX = (viewportWidth - indicatorWidth) / 2;
            _indicator.anchoredPosition = new Vector2(indicatorX + indicatorWidth / 2, _indicator.anchoredPosition.y);
        }
        
        private void InitializeLevels()
        {
            var viewportWidth = _zoneScrollView.viewport.rect.width;
            _totalLevelsToCreate = Mathf.CeilToInt(viewportWidth / 80f) + 2;  // Create a few extra for preloading

            for (var i = 0; i < _totalLevelsToCreate; i++)
            {
                CreateLevel(i);
            }
        }

        private void ResetLevels()
        {
            for (var i = 0; i < _activeLevels.Count; i++)
            {
                _activeLevels[i].SetZoneNumber(i + 1, CurrentZone == i);
            }
        }

        private void CreateLevel(int index)
        {
            var level = Instantiate(_levelPrefab, _zoneScrollView.content).GetComponent<Zone>();
            level.ZoneRectTransform.anchoredPosition = new Vector2(index * (_levelMinWidth + _minSpacing), 0);
            level.gameObject.SetActive(true);
            level.SetZoneNumber(index + 1, CurrentZone == index);

            _activeLevels.Add(level);
        }

        private void AdjustLevelSize()
        {
            var viewportWidth = _zoneScrollView.viewport.rect.width;
            var spacing = Mathf.Clamp(viewportWidth / 20, _minSpacing, _maxSpacing);
            var levelsToFit = Mathf.FloorToInt(viewportWidth / (_levelPrefab.ZoneRectTransform.rect.width + spacing));

            var levelWidth = (viewportWidth - (spacing * (levelsToFit - 1))) / levelsToFit;
            levelWidth = Mathf.Clamp(levelWidth, _levelMinWidth, _levelMaxWidth);

            _levelPrefab.ZoneRectTransform.sizeDelta = new Vector2(levelWidth, _levelPrefab.ZoneRectTransform.sizeDelta.y);
            
            foreach (var level in _activeLevels)
            {
                level.ZoneRectTransform.sizeDelta = new Vector2(levelWidth, level.ZoneRectTransform.sizeDelta.y);
            }

            _horizontalLayout.spacing = spacing;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_zoneScrollView.content);
            
            CheckVisibleLevels();
        }
        
        private void CheckVisibleLevels()
        {
            Vector3[] corners = new Vector3[4];
            _zoneScrollView.viewport.GetWorldCorners(corners);
            var viewportMinX = corners[0].x;

            var level = _activeLevels[0];
            
            Vector3[] levelCorners = new Vector3[4];
            level.ZoneRectTransform.GetWorldCorners(levelCorners);
            var levelMinX = levelCorners[0].x;
            var levelMaxX = levelCorners[3].x;
            
            if ((levelMinX - (levelMaxX - levelMinX) / 2.86f) < viewportMinX)
            {
                _usePadding--;
                // level.UpdateZoneNumber(GetNextLevelIndex());
                //
                // _activeLevels.RemoveAt(0);
                // _activeLevels.Add(level);
                // level.transform.SetAsLastSibling();
            }

            // if (level.ZoneRectTransform.anchoredPosition.x + levelWidth / 2 < viewportMinX)
            // {
            //     level.UpdateZoneNumber(GetNextLevelIndex());
            //
            //     _activeLevels.RemoveAt(0);
            //     _activeLevels.Add(level);
            //     level.transform.SetAsLastSibling();
            //     LayoutRebuilder.ForceRebuildLayoutImmediate(_zoneScrollView.content);
            // }

            if ((levelMinX - (levelMaxX - levelMinX) / 4) < viewportMinX)
            {
                if(CurrentZone == 1) return;
                if (_usePadding >= 0)
                {
                    _usePadding--;
                    return;
                }

                //if(!(_usePadding < 1))  return;
                level.UpdateZoneNumber(GetNextLevelIndex());

                // Move the level to the last sibling
                _activeLevels.RemoveAt(0);
                _activeLevels.Add(level);
                level.transform.SetAsLastSibling();

                LayoutRebuilder.ForceRebuildLayoutImmediate(_zoneScrollView.content);
            }
        }
        
        private int GetNextLevelIndex()
        {
            return (_activeLevels[^1].ZoneIndex + 1);
        }
        
        private void UpdateZoneIndicator()
        {
            var offset = Mathf.RoundToInt(_indicator.anchoredPosition.x - _levelPrefab.ZoneRectTransform.rect.width / 2);
            if (CurrentZone > 1)
            {
                if (_usePadding >= 1)
                {
                    if(_usePadding == 1) _usePadding--;
                    _horizontalLayout.padding.left = Mathf.RoundToInt((offset - (_levelPrefab.ZoneRectTransform.rect.width * (CurrentZone - 1))) - (_horizontalLayout.spacing * (CurrentZone - 1)));
                }
            }
            else
            {
                _horizontalLayout.padding.left = offset;
            }
            
            //_indicator.transform.SetAsLastSibling();
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_zoneScrollView.content);
            
            CheckVisibleLevels();
        }
        
        private void UpdateSafeZonesTexts()
        {
            var safeZone = ((CurrentZone / _everySafeZone) + 1) * _everySafeZone;
            var superZone = ((CurrentZone / _everySuperZone) + 1) * _everySuperZone;
            
            //if current zone is a safe zone, then show the next safe zone
            // if (CurrentZone % _everySafeZone == 0)
            // {
            //     safeZone += _everySafeZone;
            // }
            //if current zone is a super zone, then show the next super zone
            // if (CurrentZone % _everySuperZone == 0)
            // {
            //     superZone += _everySuperZone;
            // }

            _safeZoneText.text = safeZone.ToString();
            _superZoneText.text = superZone.ToString();
        }
        
        #endregion
        
        public enum ZoneType
        {
            Safe,
            Super,
            Regular
        }
    }
}