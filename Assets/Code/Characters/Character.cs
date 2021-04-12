namespace RunlingRun.Characters
{
    using System;
    using System.Collections;
    using Loadouts;
    using Photon.Pun;
    using RunlingRun.Managers;
    using RunlingRun.Player.Controllers;
    using RunlingRun.Utilities;
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : MonoBehaviourPun
    {
        [HideInInspector]
        public float MoveSpeed;
        public GameObject model;
        public bool isDowned = false;
        [HideInInspector]
        public CharacterLoadout Loadout;
        [HideInInspector]
        public KeyboardController _keyboardController;
        [HideInInspector]
        public GameManager MapGameManager;
        public float maxPlayerDistanceToEnd = 10000;
        private NavMeshAgent _agent;
        private Material _shader;

        // Event Management - End

        private void Awake()
        {
            GameObject gameManager = GameObject.Find("GameManager");
            MapGameManager = gameManager.GetComponent<GameManager>();
            _keyboardController = gameManager.GetComponent<KeyboardController>();
            _agent = GetComponent<NavMeshAgent>();
            // TODO: Change out for network id of player
            Loadout = new CharacterLoadout(gameObject, "Player1");
            _shader = model.GetComponentInChildren<SkinnedMeshRenderer>().material;

            ApplyLoadout();
        }

        void OnEnable()
        {
            if (photonView.IsMine)
            {
                _keyboardController.UseAbility1 += Loadout.Ability1.Activate;
                _keyboardController.UseAbility2 += Loadout.Ability2.Activate;
            }
        }

        void OnDisable()
        {
            if (photonView.IsMine)
            {
                _keyboardController.UseAbility1 -= Loadout.Ability1.Activate;
                _keyboardController.UseAbility2 -= Loadout.Ability2.Activate;
            }
        }

        private void ApplyLoadout()
        {
            Loadout.moveSpeedStat.Apply(this);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && photonView.IsMine)
            {
                Revive();
            }
        }

        private void LateUpdate()
        {
            NavMeshPath path = new NavMeshPath();
            _agent.CalculatePath(MapGameManager.EndofMapPos, path);
            float distanceToEnd = NavAgentHelpers.GetPathLength(path);
            maxPlayerDistanceToEnd = Math.Min(maxPlayerDistanceToEnd, distanceToEnd);
        }

        public void GetRekted()
        {
            StartCoroutine(PlayDeathEffect());
        }

        public IEnumerator PlayDeathEffect()
        {
            float deadValue = 0f;
            while (deadValue < 1f)
            {
                deadValue += 0.02f;
                _shader.SetFloat("PercentDisintegrated", deadValue);
                yield return new WaitForSeconds(0.05f);
            }
        }

        public void Revive()
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            GetComponent<Collider>().enabled = true;
            isDowned = false;
        }
    }
}