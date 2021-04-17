namespace RunlingRun.Character.Stats
{
    using UnityEngine;

    class BlinkDistanceStat : Stat
    {
        public const string Name = "Blink Distance";

        public override string DisplayName { get { return Name; } }

        public BlinkDistanceStat(int level) : base(level)
        {
            _level = 8 + level;
        }

        public override void Apply(GameObject player)
        {
            //pass
        }

        public int GetMaxBlinkDistance()
        {
            return _level;
        }
    }
}