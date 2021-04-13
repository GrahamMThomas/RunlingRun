namespace RunlingRun.Managers
{
    using TMPro;
    using UnityEngine;

    public class HudManager : MonoBehaviour
    {
        public TMP_Text statPointText;
        public TMP_Text moveSpeedStat;

        // --- Singleton Pattern
        private static HudManager _instance = null;
        public static HudManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
        }
        // --- End Singleton Pattern
    }
}

