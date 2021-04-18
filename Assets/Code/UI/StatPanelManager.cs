namespace RunlingRun.UI
{
    using System.Collections;
    using RunlingRun.Character;
    using RunlingRun.Character.Stats;
    using RunlingRun.Managers;
    using UnityEngine;
    using UnityEngine.UI;

    public class StatPanelManager : MonoBehaviour
    {
        public GameObject Panel;
        public GameObject CommonStats;
        public GameObject Ability1Stats;
        public GameObject Ability2Stats;

        private void Start()
        {
            CharacterSelectionManager.Instance.OnCharacterInstantiate += UpdateStatPanel;
        }

        public void UpdateStatPanel(GameObject player)
        {
            CharacterLoadout loadout = player.GetComponent<CharacterLoadout>();

            GameObject moveSpeedLine = Instantiate((GameObject)Resources.Load("UI/StatPanel/StatLineItem"), CommonStats.transform);
            moveSpeedLine.GetComponent<StatLineItem>().SetTracking(loadout.moveSpeedStat);

            foreach (Stat stat in loadout.Ability1.Attributes)
            {
                GameObject statLine = Instantiate((GameObject)Resources.Load("UI/StatPanel/StatLineItem"), Ability1Stats.transform);
                statLine.GetComponent<StatLineItem>().SetTracking(stat);
            }

            foreach (Stat stat in loadout.Ability2.Attributes)
            {
                GameObject statLine = Instantiate((GameObject)Resources.Load("UI/StatPanel/StatLineItem"), Ability2Stats.transform);
                statLine.GetComponent<StatLineItem>().SetTracking(stat);
            }
            Panel.SetActive(true);
            // Workaround because Verticle layout gorup not working
            Panel.GetComponent<VerticalLayoutGroup>().enabled = false;
            StartCoroutine(ReEnableThing());
        }

        private IEnumerator ReEnableThing()
        {
            yield return new WaitForEndOfFrame();
            Panel.GetComponent<VerticalLayoutGroup>().enabled = true;
            Panel.SetActive(false);
        }

        public void ToggleStatPanel()
        {
            Panel.SetActive(!Panel.activeSelf);
        }
    }
}