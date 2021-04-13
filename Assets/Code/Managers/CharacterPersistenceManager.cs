namespace RunlingRun.Managers
{
    using Persistence;
    using RunlingRun.Character.Stats;
    using UnityEngine;

    public class CharacterPersistenceManager : MonoBehaviour
    {
        private string CharacterSaveLocation;
        // --- Singleton Pattern
        private static CharacterPersistenceManager _instance = null;
        public static CharacterPersistenceManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
            CharacterSaveLocation = Application.persistentDataPath + "/characters/";
        }
        // --- End Singleton Pattern

        // public CharacterData[] GetAllSavedCharacters()
        // {

        // }

        public void LoadCharacter()
        {

        }

        public void SaveCharacter()
        {

        }
    }

    namespace Persistence
    {
        [System.Serializable]
        public class CharacterData
        {
            // Character Attributes
            public string CharacterName;
            public string CharacterId;
            public int CharacterLevel;

            // Loadout Stuff
            public int AvailablePoints;

            // Stats
            public Stat MoveSpeedStat;

            // Abilities
            public string Ability1Type;
            public Stat[] Ability1Stats;

            public string Ability2Type;
            public Stat[] Ability2Stats;
        }
    }
}