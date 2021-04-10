namespace RunlingRun.Player
{
    using Controllers;
    using Photon.Pun;
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviourPun
    {
        // Event Management
        private MouseController _mouseController;
        private NavMeshAgent _navAgent;

        private Animator _animator;

        void Awake()
        {
            _mouseController = GameObject.Find("GameManager").GetComponent<MouseController>();
            _navAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_navAgent.velocity.magnitude < 0.1f)
            {
                _animator.SetBool("isMoving", false);
            }
            else
            {
                _animator.SetBool("isMoving", true);
            }
        }

        void OnEnable()
        {
            if (photonView.IsMine)
            {
                _mouseController.ClickedOnMap += SetTargetLocation;
            }
        }

        void OnDisable()
        {
            if (photonView.IsMine)
            {
                _mouseController.ClickedOnMap -= SetTargetLocation;
            }
        }

        void SetTargetLocation(Vector3 pos)
        {
            _navAgent.SetDestination(pos);
        }
    }
}