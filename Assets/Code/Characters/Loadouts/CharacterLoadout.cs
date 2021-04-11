namespace RunlingRun.Characters.Loadouts
{
    using System.Collections.Generic;
    using Abilities;
    using Stats;
    using UnityEngine;

    public class CharacterLoadout
    {
        private int availablePoints;
        public MoveSpeedStat moveSpeedStat;

        public Ability Ability1;
        public Ability Ability2;

        public CharacterLoadout(GameObject player, string playerId)
        {
            Debug.Log($"Loading Stats for {playerId}");
            moveSpeedStat = new MoveSpeedStat();
            availablePoints = 0;
            Ability1 = new BlinkAbility(player, 5);
            Ability2 = new BlinkAbility(player, 0);
        }

        public void LevelUp()
        {
            availablePoints += 1;
        }

        public bool SpendPoint()
        {
            if (availablePoints > 0)
            {
                availablePoints -= 1;
                return true;
            }
            return false;
        }

        public int GetAvailablePoints()
        {
            return availablePoints;
        }

        public virtual Dictionary<string, Stat> GetAllStats()
        {
            Dictionary<string, Stat> statMapping = new Dictionary<string, Stat>();
            statMapping.Add("MoveSpeedStat", moveSpeedStat);
            return statMapping;
        }
    }
}