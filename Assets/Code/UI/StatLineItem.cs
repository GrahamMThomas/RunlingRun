namespace RunlingRun.UI
{
    using RunlingRun.Character;
    using RunlingRun.Character.Stats;
    using RunlingRun.Managers;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class StatLineItem : MonoBehaviour
    {
        public TMP_Text StatName;
        public TMP_Text LevelText;
        public TMP_Text PointsNeeded;
        public Button UpgradeButton;

        private Stat _trackingStat;
        private CharacterLoadout _loadout;

        public void SetTracking(Stat stat)
        {
            _loadout = CharacterSelectionManager.Instance.CurrentPlayer.GetComponent<CharacterLoadout>();

            _trackingStat = stat;
            StatName.text = stat.DisplayName;
            PointsNeeded.text = stat.Cost.ToString();

            UpgradeButton.onClick.AddListener(() => UpgradeStat());
            UpdateToReflectChanges();

            _loadout.OnLevelUp += UpdateToReflectChanges;
            _loadout.OnSpendPoints += UpdateToReflectChanges;
            _trackingStat.OnStatUpgrade += UpdateToReflectChanges;
        }

        public void UpdateToReflectChanges()
        {
            LevelText.text = $"Lv.{_trackingStat.Level}";
            UpgradeButton.interactable = _loadout.AvailablePoints >= _trackingStat.Cost;
        }

        public void UpgradeStat()
        {
            // CharacterSelectionManager.Instance.CurrentPlayer.GetComponent<CharacterLoadout>()
            _loadout.SpendPoints(_trackingStat.Cost);
            _trackingStat.Upgrade();
        }
    }
}