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
    public class CharacterBehaviour : MonoBehaviourPun
    {
        // Static
        public string Name;
        public string Id;
        public int Level;

        // Dynamic 
        public bool isDowned = false;
        public float maxPlayerDistanceToEnd = 10000;

        // References
        public GameObject CharacterModel;
        private NavMeshAgent _agent;
        private Material _shader;

        // Unity Hooks --------------------

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _shader = CharacterModel.GetComponentInChildren<SkinnedMeshRenderer>().material;
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
    }
}