namespace RunlingRun.Character.Stats
{
    using UnityEngine;
    using UnityEngine.AI;


    public class MoveSpeedStat : Stat
    {
        public const string Name = "Move Speed";
        public override string DisplayName { get { return Name; } }

        private readonly float _baseMoveSpeed = 4;
        private readonly float _moveSpeedPerLevel = 0.8f;

        public MoveSpeedStat(int level) : base(level)
        {
            _level = level;
        }

        public override void Apply(GameObject player)
        {
            player.GetComponent<NavMeshAgent>().speed = _baseMoveSpeed + (_moveSpeedPerLevel * _level);
        }
    }
}