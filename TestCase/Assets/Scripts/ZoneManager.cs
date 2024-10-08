using UnityEngine;

namespace TestCase.Gameplay
{
    public class ZoneManager : MonoBehaviour
    {
        #region Properties

        public int CurrentZone => _currentZone;

        #endregion
        
        #region Fields

        private int _currentZone;

        #endregion
        
        public void Initialize()
        {
            _currentZone = PlayerPrefs.GetInt("CurrentZone", 0);
        }
    }
}