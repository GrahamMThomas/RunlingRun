namespace RunlingRun.UI
{
    using System.Collections.Generic;
    using Character;
    using Photon.Pun;
    using RunlingRun.Character.Abilities;
    using RunlingRun.Managers;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterInfo : MonoBehaviourPun
    {
        public TMP_Text UsernameText;
        public TMP_Text PlayerNameText;
        public TMP_Text LevelText;
        public Image ExpBar;

        public AbilityIndicator Ability1;
        public AbilityIndicator Ability2;

        // --- Singleton Pattern
        private static CharacterInfo _instance = null;
        public static CharacterInfo Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
        }
        // --- End Singleton Pattern

        private void LateUpdate()
        {
            GameObject player = CharacterSelectionManager.Instance.CurrentPlayer;

            CharacterBehaviour behav = player.GetComponent<CharacterBehaviour>();
            CharacterLoadout loadout = player.GetComponent<CharacterLoadout>();

            UpdateExpBar(behav);
            Ability1.CooldownImage.fillAmount = GetPercentCooldownLeft(loadout.Ability1);
            Ability2.CooldownImage.fillAmount = GetPercentCooldownLeft(loadout.Ability2);

            // Set Charges
            SetChargesText(Ability1, loadout.Ability1.CurrentCharges);
            SetChargesText(Ability2, loadout.Ability2.CurrentCharges);
        }

        public void SetCharInfo(GameObject player)
        {
            CharacterBehaviour behav = player.GetComponent<CharacterBehaviour>();
            UsernameText.text = PhotonNetwork.NickName;
            PlayerNameText.text = behav.Name;
            SetAbilityIcon(player.GetComponent<CharacterLoadout>().Ability1, Ability1);
            SetAbilityIcon(player.GetComponent<CharacterLoadout>().Ability2, Ability2);
        }

        public void UpdateExpBar(CharacterBehaviour behav)
        {
            LevelText.text = $"Lv.{behav.Level}";
            ExpBar.transform.localScale = new Vector3(behav.Experience / (float)behav.ExpNeededForLevel, 1, 1);
        }

        public void SetAbilityIcon(Ability ability, AbilityIndicator abilityIndicator)
        {
            // Set Image
            Dictionary<System.Type, string> abilityToImageMapping = new Dictionary<System.Type, string>() {
                {typeof(BlinkAbility), "BlinkIcon"},
                {typeof(ReviveProjectileAbility), "ReviveProjectile"}
            };

            string iconPath = "UI/Abilities/";
            System.Type abilityType = ability.GetType();
            string spritename = abilityToImageMapping[abilityType];
            abilityIndicator.AbilityImage.sprite = Resources.Load<Sprite>(iconPath + spritename);
        }

        public void SetChargesText(AbilityIndicator indicator, int chargeNum)
        {
            if (chargeNum <= 1)
            {
                indicator.ChargesText.text = "";
                return;
            }
            else
            {
                indicator.ChargesText.text = chargeNum.ToString();
            }
        }

        private float GetPercentCooldownLeft(Ability ability)
        {
            if (ability.CurrentCharges <= 0)
            {
                return ability.CooldownTimer / ability.Cooldown;
            }
            else
            {
                return 0f;
            }
        }
    }
}