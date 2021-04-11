namespace RunlingRun.Characters.Loadouts.Abilities
{
    using System.Collections;
    using UnityEngine;
    public abstract class Ability
    {
        public string DisplayName = "Ability";
        public bool isActive = false;
        protected int _level = 0;
        protected GameObject _player;
        public abstract IEnumerator Activate();

        public Ability(GameObject player, int level)
        {
            _level = level;
            _player = player;
        }

        public bool IsUnlocked()
        {
            return _level > 0;
        }
    }
}