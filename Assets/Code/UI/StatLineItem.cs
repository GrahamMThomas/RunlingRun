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

        private Stat trackingStat;

        public void SetTracking(Stat stat)
        {
            trackingStat = stat;
            StatName.text = stat.DisplayName;
            PointsNeeded.text = stat.PointsNeededForUpgrade.ToString();

            UpgradeButton.onClick.AddListener(() => stat.Upgrade());
            UpdateOnPointChange();
            UpdateOnUpgrade();

            CharacterSelectionManager.Instance.CurrentPlayer.GetComponent<CharacterLoadout>().OnLevelUp += UpdateOnPointChange;
            stat.OnStatUpgrade += UpdateOnUpgrade;
        }

        public void UpdateOnUpgrade()
        {
            LevelText.text = $"Lv.{trackingStat.Level}";
        }

        public void UpdateOnPointChange()
        {
            CharacterLoadout loadout = CharacterSelectionManager.Instance.CurrentPlayer.GetComponent<CharacterLoadout>();
            UpgradeButton.interactable = loadout.GetAvailablePoints() >= trackingStat.PointsNeededForUpgrade;
        }

        private void OnDisable()
        {
            trackingStat.OnStatUpgrade -= UpdateOnUpgrade;
            CharacterSelectionManager.Instance.CurrentPlayer.GetComponent<CharacterLoadout>().OnLevelUp -= UpdateOnPointChange;
        }
    }
}