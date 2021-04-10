namespace RunlingRun.Characters.Stats
{
    public class MoveSpeedStat : Stat
    {
        public MoveSpeedStat()
        {
            level = 15;
        }
        public void Apply(Character character)
        {
            character.MoveSpeed = level;
        }
    }
}