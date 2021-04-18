namespace RunlingRun.Character.Stats
{
    using UnityEngine;

    class BlinkDistanceStat : Stat
    {
        public const string Name = "Blink Distance";

        public override string DisplayName { get { return Name; } }

        public BlinkDistanceStat(int level) : base(level)
        {
            _level = level;
        }

        public override void Apply(GameObject player)
        {
            //pass
        }

        public float GetMaxBlinkDistance()
        {
            return (Mathf.Pow(_level, 0.5f) * 2) + 2;
        }
    }
}