namespace RunlingRun.Character
{
    using System;
    using System.Collections;
    using Photon.Pun;
    using RunlingRun.Managers;
    using RunlingRun.Utilities;
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterLoadout))]
    public class CharacterBehaviour : MonoBehaviourPun
    {
        // Static
        public string Name;
        public string Id;
        public int Level;
        public int Experience;
        public int ExpNeededForLevel;

        // Dynamic 
        public bool isDowned = false;
        public float maxPlayerDistanceToEnd = 10000;

        // References
        public GameObject CharacterModel;
        private NavMeshAgent _agent;
        private Material _shader;

        public void SetProgress(int level, int experience)
        {
            Level = level;
            Experience = experience;
            SetExpNeededForNextLevel();
        }

        // Unity Hooks --------------------

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _shader = CharacterModel.GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        private void Start()
        {
            // Ignore non-trigger colliders on all Safezones
            foreach (GameObject safeZone in GameObject.FindGameObjectsWithTag("Wall/EnemyOnly"))
            {
                Physics.IgnoreCollision(safeZone.GetComponent<Collider>(), GetComponent<Collider>());
            }
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
            _agent.CalculatePath(GameManager.Instance.EndofMapPos, path);
            float distanceToEnd = NavAgentHelpers.GetPathLength(path);
            maxPlayerDistanceToEnd = Math.Min(maxPlayerDistanceToEnd, distanceToEnd);
        }

        private void OnDestroy()
        {
            CharacterPersistenceManager.Instance.SaveCharacter(gameObject);
        }

        private void OnApplicationQuit()
        {
            CharacterPersistenceManager.Instance.SaveCharacter(gameObject);
        }

        // Character Actions --------------------

        public void GetRekted()
        {
            if (isDowned) { return; }
            isDowned = true;
            _agent.ResetPath();
            _agent.isStopped = true;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(PlayDeathEffect());
        }

        public void Revive()
        {
            _agent.isStopped = false;
            _shader.SetFloat("PercentDisintegrated", 0f);
            GetComponent<Collider>().enabled = true;
            isDowned = false;
        }

        public void AwardExp(int amount)
        {
            Experience += amount;
            while (Experience >= ExpNeededForLevel)
            {
                Experience -= ExpNeededForLevel;
                Level += 1;
                SetExpNeededForNextLevel();
                GetComponent<CharacterLoadout>().LevelUp();
            }
        }

        // Helper Methods

        private IEnumerator PlayDeathEffect()
        {
            float deadValue = 0f;
            while (deadValue < 1f)
            {
                deadValue += 0.02f;
                _shader.SetFloat("PercentDisintegrated", deadValue);
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void SetExpNeededForNextLevel()
        {
            ExpNeededForLevel = (int)(Mathf.Pow(Level, 1f / 1.6f) * 4 + 4);
        }
    }
}