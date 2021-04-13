namespace RunlingRun.Character.Stats
{
    using UnityEngine;

    class BlinkChargesStat : Stat
    {
        public new string DisplayName = "Blink Charges";

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