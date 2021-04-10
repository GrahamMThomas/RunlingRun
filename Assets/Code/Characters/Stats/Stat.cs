namespace RunlingRun.Characters.Stats
{
    public class Stat
    {
        protected int level;

        public void Upgrade()
        {
            level += 1;
        }

        public int GetLevel()
        {
            return level;
        }

        public virtual void Apply()
        {
            // pass
        }
    }
}