namespace RunlingRun.Character.Stats
{
    using UnityEngine;

    [System.Serializable]
    public abstract class Stat
    {
        public string DisplayName { get; }
        //TODO public Image Icon;
        protected int _level;
        public void Upgrade() { _level += 1; }
        public int GetLevel() { return _level; }

        public Stat(int level)
        {
            _level = level;
        }

        public abstract void Apply(GameObject player);
    }
}