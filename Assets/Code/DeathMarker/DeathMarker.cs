namespace RunlingRun.DeathMarker
{
    using RunlingRun.Character;
    using TMPro;
    using UnityEngine;
    public class DeathMarker : MonoBehaviour
    {
        public TMP_Text PlayerNameText;
        private CharacterBehaviour _behav;

        public void SetCharacter(CharacterBehaviour behav)
        {
            _behav = behav;
            PlayerNameText.text = _behav.Name;
            _behav.OnRevive += PlayerHasRevived;
        }

        private void PlayerHasRevived()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _behav.OnRevive -= PlayerHasRevived;
        }
    }
}

