namespace RunlingRun.Character.Stats
{
    using UnityEngine;

    class BlinkChargesStat : Stat
    {
        public const string Name = "Blink Charges";

        public override string DisplayName { get { return Name; } }

        public BlinkChargesStat(int level) : base(level)
        {
            _level = level;
        }

        public override void Apply(GameObject player)
        {
            //pass
        }

        public int GetMaxBlinkCharges()
        {
            return _level;
        }
    }
}