using TMPro;
using UnityEngine;

namespace TestCase.Gameplay.UI
{
    public class Zone : MonoBehaviour
    {
        public int ZoneIndex => _zoneIndex;
        public RectTransform ZoneRectTransform => _rectTransform;

        #region Fields

        [SerializeField] private TextMeshProUGUI _zoneNumberText;
        [SerializeField] private GameObject _bg;
        [SerializeField] private RectTransform _rectTransform;
        
        private int _zoneIndex;

        #endregion

        public void SetZoneNumber(int zoneNumber, bool isCurrentZone)
        {
            _zoneIndex = zoneNumber;
            _zoneNumberText.text = zoneNumber.ToString();
            //_bg.SetActive(isCurrentZone);
        }
        
        public void UpdateZoneNumber(int zoneNumber)
        {
            _zoneIndex = zoneNumber;
            _zoneNumberText.text = zoneNumber.ToString();
        }
    }
}