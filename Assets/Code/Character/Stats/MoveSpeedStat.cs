namespace RunlingRun.Character.Stats
{
    using UnityEngine;
    using UnityEngine.AI;


    public class MoveSpeedStat : Stat
    {
        public const string Name = "Move Speed";
        public override string DisplayName { get { return Name; } }

        public MoveSpeedStat(int level) : base(level)
        {
            _level = 7 + level;
        }

        public override void Apply(GameObject player)
        {
            player.GetComponent<NavMeshAgent>().speed = _level;
        }
    }
}