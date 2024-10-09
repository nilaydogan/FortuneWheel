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
        private Queue<Zone> _levelPool;

        private float _minSpacing = 0, _maxSpacing = 2f;
        private float _levelMinWidth = 80f, _levelMaxWidth = 103f;
        
        #endregion

        public void Initialize()
        {
            _activeLevels = new List<Zone>();
            _levelPool = new Queue<Zone>();
            
            InitializeLevels();
            AdjustLevelSize();

            CurrentZone = PlayerPrefs.GetInt("CurrentZone", 0);
        }

        #region Private Methods
        
        private void InitializeLevels()
        {
            var viewportWidth = _zoneScrollView.viewport.rect.width;
            var levelsInView = Mathf.CeilToInt(viewportWidth / 80f) + 2;  // Create a few extra for preloading

            for (var i = 0; i < levelsInView; i++)
            {
                CreateLevel(i);
            }
        }

        private void CreateLevel(int index)
        {
            var level = GetLevelFromPool();
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
            
            Debug.Log("Level Width: " + levelWidth);

            _levelPrefab.ZoneRectTransform.sizeDelta = new Vector2(levelWidth, _levelPrefab.ZoneRectTransform.sizeDelta.y);
            
            foreach (var level in _activeLevels)
            {
                level.ZoneRectTransform.sizeDelta = new Vector2(levelWidth, level.ZoneRectTransform.sizeDelta.y);
            }

            _horizontalLayout.spacing = spacing;
            //_horizontalLayout.padding.left = Mathf.FloorToInt(spacing / 2);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_zoneScrollView.content);
        }
        
        private Zone GetLevelFromPool()
        {
            if (_levelPool.Count > 0)
            {
                return _levelPool.Dequeue();
            }
            return Instantiate(_levelPrefab, _zoneScrollView.content).GetComponent<Zone>();
        }

        private void PoolLevel(Zone level)
        {
            level.gameObject.SetActive(false);
            _levelPool.Enqueue(level);
        }
        
        private void UpdateZoneIndicator()
        {
            _indicator.anchoredPosition = new Vector2(CurrentZone * (_levelPrefab.ZoneRectTransform.rect.width + _horizontalLayout.spacing), 0);
        }
        
        private void UpdateSafeZonesTexts()
        {
            var safeZone = (CurrentZone / 5) * 5;
            var superZone = (CurrentZone / 30) * 30;
            _safeZoneText.text = safeZone.ToString();
            _superZoneText.text = superZone.ToString();
        }
        
        #endregion
    }
}