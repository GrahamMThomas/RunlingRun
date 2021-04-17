namespace RunlingRun.UI
{
    using RunlingRun.Managers.Persistence;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterInfo : MonoBehaviour
    {
        public TMP_Text UsernameText;
        public TMP_Text PlayerNameText;
        public TMP_Text LevelText;
        public Image ExpBar;

        // --- Singleton Pattern
        private static CharacterInfo _instance = null;
        public static CharacterInfo Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
        }
        // --- End Singleton Pattern

        public void SetCharInfo(CharacterData data)
        {

        }
    }
}