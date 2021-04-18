namespace RunlingRun.Character.Stats
{
    using RunlingRun.Character.Abilities;
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
            // pass
        }

        public override void Apply(Ability ability)
        {
            _trackedAbility = ability;
            BlinkAbility bAbility = (BlinkAbility)ability;
            bAbility.BlinkDistance = (Mathf.Pow(_level, 0.5f) * 2) + 2;
        }
    }
}