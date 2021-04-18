namespace RunlingRun.Character.Stats
{
    using System;
    using Character.Abilities;
    using UnityEngine;

    public abstract class Stat
    {
        public abstract string DisplayName { get; }
        public int Level { get { return _level; } }
        protected int _level;
        public int Cost = 1;
        public event Action OnStatUpgrade;
        protected GameObject _trackedPlayer;
        protected Ability _trackedAbility;

        public void Upgrade()
        {
            _level += 1;
            OnStatUpgrade?.Invoke();
            Apply(_trackedAbility);
            Apply(_trackedPlayer);
        }

        public Stat(int level)
        {
            _level = level;
        }

        public abstract void Apply(GameObject player);
        public abstract void Apply(Ability ability);

        // Serialization Stuff -----------------------------------

        [System.Serializable]
        public class SerializableStat
        {
            public string DisplayName;
            public int Level;

            public Stat Deserialize()
            {
                switch (DisplayName)
                {
                    case MoveSpeedStat.Name:
                        return new MoveSpeedStat(Level);
                    case BlinkDistanceStat.Name:
                        return new BlinkDistanceStat(Level);
                    case BlinkChargesStat.Name:
                        return new BlinkChargesStat(Level);
                    default:
                        throw new UnityException($"Can't find stat with name: {DisplayName}");
                }
            }
        }

        public SerializableStat ToSerializeable()
        {
            SerializableStat sStat = new SerializableStat();
            sStat.DisplayName = DisplayName;
            sStat.Level = _level;
            return sStat;
        }
    }
}