namespace RunlingRun.Managers
{
    using System;
    using Character;
    using Persistence;
    using Photon.Pun;
    using TMPro;
    using UI;
    using UnityEngine;

    public class CharacterSelectionManager : MonoBehaviourPun
    {
        public GameObject CurrentPlayer;
        public GameObject CharListPanel;
        public TMP_InputField CharacterNameInput;
        public GameObject NewCharacterPanel;
        public GameObject CharListContainer;
        public GameObject MunchCharPanelPrefab;
        public GameObject SpawnArea;
        public GameObject CharInfoPanel;

        // --- Singleton Pattern
        private static CharacterSelectionManager _instance = null;
        public static CharacterSelectionManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
        }
        // --- End Singleton Pattern

        private void Start()
        {
            ActivateCharacterSelectionScreen();
            PopulateCharacterSelectionScreen();
        }

        private void PopulateCharacterSelectionScreen()
        {
            foreach (CharacterData data in CharacterPersistenceManager.Instance.GetAllSavedCharacters())
            {
                GameObject panelType = data.CharacterType switch
                {
                    CharacterTypes.Munch => MunchCharPanelPrefab,
                    _ => throw new ArgumentException($"Can't find prefab with name {data.CharacterType}"),
                };
                GameObject charPanel = Instantiate(panelType, CharListContainer.transform);
                charPanel.transform.localScale = new Vector3(0.8f, 0.8f, 1);
                CharacterSelectionPanel panel = charPanel.GetComponent<CharacterSelectionPanel>();
                panel.NameText.text = data.CharacterName;
                panel.LevelText.text = "Lv. " + data.CharacterLevel;
                panel.PlayButton.onClick.AddListener(() => SpawnPlayer(data));
            }
        }

        public void SpawnPlayer(CharacterData data)
        {
            string charType = Enum.GetName(typeof(CharacterTypes), data.CharacterType);
            CurrentPlayer = PhotonNetwork.Instantiate(charType, GetSpawnPoint(), Quaternion.identity);
            CharacterPersistenceManager.Instance.ApplyCharacterDataToObject(CurrentPlayer, data);
            CharacterSpawnedActions();
        }

        public void ActivateCharacterSelectionScreen()
        {
            CharListPanel.SetActive(true);
        }

        public void ActivateNewCharacterScreen()
        {
            NewCharacterPanel.SetActive(true);
        }

        public void CreateNewCharacter(int charType)
        {
            CurrentPlayer = PhotonNetwork.Instantiate(((CharacterTypes)charType).ToString(), GetSpawnPoint(), Quaternion.identity);

            CharacterPersistenceManager.Instance.ApplyNewCharacterStats(CurrentPlayer, CharacterNameInput.text, (CharacterTypes)charType);
            CharacterPersistenceManager.Instance.SaveCharacter(CurrentPlayer);
            CharacterSpawnedActions();
        }

        public void CharacterSpawnedActions()
        {
            CameraManager.Instance.StartTracking();
            CharInfoPanel.SetActive(true);
            UI.CharacterInfo.Instance.SetCharInfo(CurrentPlayer);
            CloseCharacterScreens();
        }

        private void CloseCharacterScreens()
        {
            CharListPanel.SetActive(false);
            NewCharacterPanel.SetActive(false);
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