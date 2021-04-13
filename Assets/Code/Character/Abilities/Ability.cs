namespace RunlingRun.Character.Abilities
{
    using System.Collections;
    using Stats;
    using UnityEngine;
    public abstract class Ability
    {
        public string DisplayName = "Ability";
        public bool IsActive = false;
        public bool IsUnlocked = true;
        public int CurrentCharges = 1;
        public int MaxCharges = 1;
        public Stat[] attributes;
        protected GameObject _player;
        public abstract IEnumerator Activate();

        public Ability(GameObject player, Stat[] myAttributes)
        {
            attributes = myAttributes;
            _player = player;
        }
    }
}