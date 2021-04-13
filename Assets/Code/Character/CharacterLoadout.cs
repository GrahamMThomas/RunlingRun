namespace RunlingRun.Character
{
    using Abilities;
    using Photon.Pun;
    using RunlingRun.Managers;
    using RunlingRun.Player.Controllers;
    using Stats;
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterLoadout : MonoBehaviourPun
    {
        private int _availablePoints;
        public MoveSpeedStat moveSpeedStat;

        public Ability Ability1;
        public Ability Ability2;

        [HideInInspector]
        public KeyboardController _keyboardController;

        // Getters ----------------------------
        public int GetAvailablePoints() { return _availablePoints; }

        // Unity Hooks ------------------------

        private void Awake()
        {
            _keyboardController = GameManager.Instance.GetComponent<KeyboardController>();
        }

        private void OnEnable()
        {
            moveSpeedStat.Apply(gameObject);
            if (photonView.IsMine)
            {
                _keyboardController.UseAbility1 += Ability1.Activate;
                _keyboardController.UseAbility2 += Ability2.Activate;
            }
        }

        private void OnDisable()
        {
            if (photonView.IsMine)
            {
                _keyboardController.UseAbility1 -= Ability1.Activate;
                _keyboardController.UseAbility2 -= Ability2.Activate;
            }
        }

        // Functions --------------------------

        public void LevelUp()
        {
            _availablePoints += 1;
        }

        public bool SpendPoint()
        {
            if (_availablePoints > 0)
            {
                _availablePoints -= 1;
                return true;
            }
            return false;
        }
    }
}