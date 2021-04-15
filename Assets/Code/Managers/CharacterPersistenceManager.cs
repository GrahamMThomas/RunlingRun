namespace RunlingRun.Managers
{
    using System.Collections.Generic;
    using System.IO;
    using Character;
    using Character.Abilities;
    using Character.Stats;
    using Persistence;
    using UnityEngine;
    using UnityEngine.AI;

    public class CharacterPersistenceManager : MonoBehaviour
    {
        private const string fileExtension = ".runling";
        private string CharacterSaveLocation;
        // --- Singleton Pattern
        private static CharacterPersistenceManager _instance = null;
        public static CharacterPersistenceManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
            CharacterSaveLocation = Application.persistentDataPath + "/characters/";
            Directory.CreateDirectory(CharacterSaveLocation);
            Debug.Log(CharacterSaveLocation);
        }
        // --- End Singleton Pattern

        public void ApplyNewCharacterStats(GameObject character, string name, CharacterTypes type)
        {
            CharacterBehaviour behaviour = character.GetComponent<CharacterBehaviour>();
            CharacterLoadout loadout = character.GetComponent<CharacterLoadout>();
            if (type == CharacterTypes.Munch)
            {
                behaviour.Name = name;
                behaviour.Id = System.Guid.NewGuid().ToString();
                loadout.moveSpeedStat = new MoveSpeedStat(1);
                BlinkDistanceStat blinkDistanceStat = new BlinkDistanceStat(1);
                BlinkChargesStat blinkChargesStat = new BlinkChargesStat(1);
                loadout.Ability1 = new BlinkAbility(character, new Stat[] { blinkDistanceStat, blinkChargesStat });
                loadout.Ability2 = new BlinkAbility(character, new Stat[] { blinkDistanceStat, blinkChargesStat });
            }
            else
            {
                throw new UnityException($"Unknown type of character {type}");
            }
            loadout.Init();
        }

        public List<CharacterData> GetAllSavedCharacters()
        {
            List<CharacterData> datas = new List<CharacterData>();
            foreach (string fileName in Directory.GetFiles(CharacterSaveLocation))
            {
                datas.Add(JsonUtility.FromJson<CharacterData>(File.ReadAllText(fileName)));
            }
            return datas;
        }

        // LoadCharacter in place
        public void ApplyCharacterDataToObject(GameObject character, CharacterData data)
        {
            CharacterBehaviour behaviour = character.GetComponent<CharacterBehaviour>();
            CharacterLoadout loadout = character.GetComponent<CharacterLoadout>();

            // Update Character
            behaviour.Name = data.CharacterName;
            behaviour.Id = data.CharacterId;

            // Update common stats
            loadout.moveSpeedStat = (MoveSpeedStat)data.MoveSpeedStat.Deserialize();
            loadout.moveSpeedStat.Apply(character);

            // Add abilities to loadout
            loadout.Ability1 = Ability.Deserialize(character, data.Abilities[data.Ability1Index]);
            loadout.Ability2 = Ability.Deserialize(character, data.Abilities[data.Ability2Index]);

            loadout.Init();
        }

        public void SaveCharacter(GameObject character)
        {
            CharacterData saveData = new CharacterData();
            CharacterBehaviour behaviour = character.GetComponent<CharacterBehaviour>();
            CharacterLoadout loadout = character.GetComponent<CharacterLoadout>();

            // Save Character
            saveData.CharacterName = behaviour.Name;
            saveData.CharacterId = behaviour.Id;

            // Save common stats
            saveData.MoveSpeedStat = loadout.moveSpeedStat.ToSerializeable();

            // To eventually support multiple ability selections be character
            Ability.SerializableAbility bob = loadout.Ability1.ToSerializeable();
            saveData.Abilities = new Ability.SerializableAbility[] { loadout.Ability1.ToSerializeable(), loadout.Ability2.ToSerializeable() };
            saveData.Ability1Index = 0;
            saveData.Ability2Index = 1;

            string jsonCharacterBlob = JsonUtility.ToJson(saveData);

            string characterFilePath = CharacterSaveLocation + saveData.CharacterId + fileExtension;
            File.WriteAllText(characterFilePath, jsonCharacterBlob);
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
            public CharacterTypes CharacterType;

            // Loadout Stuff
            public int AvailablePoints;

            // Stats
            public Stat.SerializableStat MoveSpeedStat;

            // Abilities
            public Ability.SerializableAbility[] Abilities; // Must be >= 2
            public int Ability1Index;
            public int Ability2Index;
        }
    }
}