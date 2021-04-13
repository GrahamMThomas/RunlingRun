namespace RunlingRun.Character.Stats
{
    using UnityEngine;
    using UnityEngine.AI;


    public class MoveSpeedStat : Stat
    {
        public MoveSpeedStat(int level) : base(level)
        {
            _level = level;
        }

        public override void Apply(GameObject player)
        {
            player.GetComponent<NavMeshAgent>().speed = _level;
        }
    }
}