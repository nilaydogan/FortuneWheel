using UnityEngine;

namespace TestCase.Gameplay
{
    public class ZoneManager : MonoBehaviour
    {
        #region Properties

        public int CurrentZone { get; private set; }

        #endregion
        
        #region Fields

        #endregion
        
        public void Initialize()
        {
            CurrentZone = PlayerPrefs.GetInt("CurrentZone", 0);
        }
    }
}