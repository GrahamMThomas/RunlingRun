namespace RunlingRun.Managers
{
    using System.Collections;
    using System.Collections.Generic;
    using RunlingRun.Characters;
    using RunlingRun.Characters.Loadouts;
    using TMPro;
    using UnityEngine;

    public class HudManager : MonoBehaviour
    {
        public GameManager gameManager;
        public TMP_Text statPointText;
        public TMP_Text moveSpeedStat;

        void Update()
        {
            CharacterLoadout stats = gameManager.CurrentPlayer.GetComponent<Character>().Stats;
            statPointText.text = $"Available Points: {stats.GetAvailablePoints()}";
            moveSpeedStat.text = stats.moveSpeedStat.GetLevel().ToString();
        }

        public void UpdateMoveSpeed()
        {
            CharacterLoadout stats = gameManager.CurrentPlayer.GetComponent<Character>().Stats;
            if (stats.SpendPoint())
            {
                stats.moveSpeedStat.Upgrade();
            }
        }
    }
}

