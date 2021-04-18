namespace RunlingRun.Character
{
    using System;
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
        public event Action OnLevelUp;
        public MoveSpeedStat moveSpeedStat;

        public Ability Ability1;
        public Ability Ability2;

        private int _availablePoints;

        [HideInInspector]
        public KeyboardController _keyboardController;

        // Getters ----------------------------
        public int GetAvailablePoints() { return _availablePoints; }
        public void SetAvailablePoints(int points) { _availablePoints = points; }

        // Unity Hooks ------------------------

        private void Awake()
        {
            _keyboardController = GameManager.Instance.GetComponent<KeyboardController>();
        }

        private void OnDisable()
        {
            if (photonView.IsMine)
            {
                _keyboardController.UseAbility1 -= Ability1.Activate;
                _keyboardController.UseAbility2 -= Ability2.Activate;
            }
        }

        public void LevelUp()
        {
            _availablePoints += 1;
            OnLevelUp();
        }

        // Functions --------------------------

        public void Init()
        {
            moveSpeedStat.Apply(gameObject);
            if (photonView.IsMine)
            {
                _keyboardController.UseAbility1 += Ability1.Activate;
                _keyboardController.UseAbility2 += Ability2.Activate;
            }
        }
    }
}