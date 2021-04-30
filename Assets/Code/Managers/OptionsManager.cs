namespace RunlingRun.Managers
{
    using UnityEngine;
    using UnityEngine.UI;

    public class OptionsManager : MonoBehaviour
    {
        public bool QuickCast;
        public Toggle QCToggle;


        // --- Singleton Pattern
        private static OptionsManager _instance = null;
        public static OptionsManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
        }
        // --- End Singleton Pattern

        private void Start()
        {
            QuickCast = PlayerPrefs.GetInt("QuickCast", 0) == 1;
            QCToggle.isOn = QuickCast;
        }

        public void SetQuickCast(bool value)
        {
            QuickCast = value;
            PlayerPrefs.SetInt("QuickCast", QuickCast ? 1 : 0);
        }
    }
}