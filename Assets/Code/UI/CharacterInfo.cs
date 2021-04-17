namespace RunlingRun.UI
{
    using Character;
    using Photon.Pun;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterInfo : MonoBehaviourPun
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

        public void SetCharInfo(GameObject player)
        {
            gameObject.SetActive(true);
            CharacterBehaviour behav = player.GetComponent<CharacterBehaviour>();
            UsernameText.text = PhotonNetwork.NickName;
            PlayerNameText.text = behav.Name;
            LevelText.text = $"Lv.{behav.Level}";
            UpdateExpBar(player);
        }

        public void UpdateExpBar(GameObject player)
        {
            CharacterBehaviour behav = player.GetComponent<CharacterBehaviour>();
            LevelText.text = $"Lv.{behav.Level}";
            ExpBar.transform.localScale = new Vector3(behav.Experience / (float)behav.ExpNeededForLevel, 1, 1);
        }
    }
}