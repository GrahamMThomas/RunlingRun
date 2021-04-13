namespace RunlingRun.Character.Stats
{
    using UnityEngine;

    class BlinkDistanceStat : Stat
    {
        public new string DisplayName = "Blink Distance";

        public BlinkDistanceStat(int level) : base(level)
        {
            _level = level;
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