namespace RunlingRun.Managers
{
    using System;
    using Character;
    using Persistence;
    using Photon.Pun;
    using UI;
    using UnityEngine;

    public class CharacterSelectionManager : MonoBehaviourPun
    {
        public GameObject CurrentPlayer;
        public GameObject CharWindow;
        public GameObject CharListPanel;
        public GameObject NewCharacterPanel;
        public GameObject CharPanelContainer;
        public GameObject CharPanelPrefab;
        public GameObject SpawnArea;
        private CharacterData _selectCharData;

        // --- Singleton Pattern
        private static CharacterSelectionManager _instance = null;
        public static CharacterSelectionManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
            PopulateCharacterSelectionScreen();
        }
        // --- End Singleton Pattern

        private void PopulateCharacterSelectionScreen()
        {
            foreach (CharacterData data in CharacterPersistenceManager.Instance.GetAllSavedCharacters())
            {
                GameObject charPanel = (GameObject)Instantiate(CharPanelPrefab, CharPanelContainer.transform);
                CharacterSelectionPanel panel = charPanel.GetComponent<CharacterSelectionPanel>();
                panel.NameText.text = data.CharacterName;
                panel.LevelText.text = "Level " + data.CharacterLevel;
                panel.TypeText.text = data.CharacterType.ToString();
                panel.SelectButton.onClick.AddListener(() => SetSelectedCharacter(data));
            }
        }

        private void SetSelectedCharacter(CharacterData data)
        {
            Debug.Log("Selected character!");
            _selectCharData = data;
        }

        public void SpawnPlayer()
        {
            // Create Networked Object
            string charType = Enum.GetName(typeof(CharacterTypes), _selectCharData.CharacterType);
            CurrentPlayer = PhotonNetwork.Instantiate(charType, GetSpawnPoint(), Quaternion.identity);
            CharacterPersistenceManager.Instance.ApplyCharacterDataToObject(CurrentPlayer, _selectCharData);
            CharWindow.SetActive(false);
            CameraManager.Instance.StartTracking();
        }

        public void SwitchToNewCharacterScreen()
        {
            CharListPanel.SetActive(false);
            NewCharacterPanel.SetActive(true);
        }

        public void CreateNewCharacter(int charType)
        {

            CurrentPlayer = PhotonNetwork.Instantiate(((CharacterTypes)charType).ToString(), GetSpawnPoint(), Quaternion.identity);

            CharacterPersistenceManager.Instance.ApplyNewCharacterStats(CurrentPlayer, "Bob", (CharacterTypes)charType);
            NewCharacterPanel.SetActive(false);
            CharListPanel.SetActive(false);
            CharWindow.SetActive(false);
            CameraManager.Instance.StartTracking();
            CharacterPersistenceManager.Instance.SaveCharacter(CurrentPlayer);
        }

        private Vector3 GetSpawnPoint()
        {
            Vector3 spawnPoint = SpawnArea.transform.position;
            spawnPoint.x += UnityEngine.Random.Range(-3, 3);
            spawnPoint.z += UnityEngine.Random.Range(-3, 3);
            return spawnPoint;
        }
    }
}