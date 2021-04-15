namespace RunlingRun.Character.Abilities
{
    using System.Collections;
    using System.Collections.Generic;
    using Stats;
    using UnityEngine;
    public abstract class Ability
    {
        public abstract string DisplayName { get; }
        public bool IsActive = false;
        public bool IsUnlocked = true;
        public int CurrentCharges = 1;
        public int MaxCharges = 1;
        public Stat[] Attributes;
        protected GameObject _player;
        public abstract IEnumerator Activate();

        public Ability(GameObject player, Stat[] myAttributes)
        {
            Attributes = myAttributes;
            _player = player;
        }


        // Serialization Stuff ----------------------------------------------

        public static Ability Deserialize(GameObject player, SerializableAbility sAbility)
        {
            // Deserialize Related Stats
            List<Stat> attributes = new List<Stat>();
            for (int x = 0; x < sAbility.attributes.Length; x++)
            {
                attributes.Add(sAbility.attributes[x].Deserialize());
            }

            if (sAbility.DisplayName == BlinkAbility.Name)
            {
                return new BlinkAbility(player, attributes.ToArray());
            }

            throw new UnityException($"Can't find Ability for \"{sAbility.DisplayName}\"");
        }

        [System.Serializable]
        public class SerializableAbility
        {
            public string DisplayName;
            public Stat.SerializableStat[] attributes = new Stat.SerializableStat[10];
        }

        public SerializableAbility ToSerializeable()
        {
            SerializableAbility sAbility = new SerializableAbility();
            sAbility.DisplayName = DisplayName;
            List<Stat.SerializableStat> attributes = new List<Stat.SerializableStat>();
            for (int x = 0; x < Attributes.Length; x++)
            {
                attributes.Add(Attributes[x].ToSerializeable());
            }
            sAbility.attributes = attributes.ToArray();
            return sAbility;
        }
    }
}