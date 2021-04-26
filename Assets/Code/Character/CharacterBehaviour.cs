namespace RunlingRun.Character
{
    using System;
    using System.Collections;
    using Photon.Pun;
    using RunlingRun.Managers;
    using RunlingRun.UI;
    using RunlingRun.Utilities;
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(CharacterLoadout))]
    public class CharacterBehaviour : MonoBehaviourPun, IPunObservable
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

        // Events
        public event Action OnRevive;

        // References
        public GameObject CharacterModel;
        private NavMeshAgent _agent;
        private Material _shader;

        // Network Related
        private Vector3 _networkLocation;
        private Quaternion _networkRotation;

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
            if (!photonView.IsMine)
            {
                if ((transform.position - _networkLocation).magnitude > 2f)
                {
                    transform.position = _networkLocation;
                    transform.rotation = _networkRotation;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, _networkLocation, Time.deltaTime * _agent.speed);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, _networkRotation, Time.deltaTime * _agent.angularSpeed);
                }
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
            if (photonView.IsMine) { CharacterPersistenceManager.Instance.SaveCharacter(gameObject); }

        }

        private void OnApplicationQuit()
        {
            if (photonView.IsMine) { CharacterPersistenceManager.Instance.SaveCharacter(gameObject); }
        }

        // Character Actions --------------------

        public void GetRekted()
        {
            photonView.RPC("RPCGetRekted", RpcTarget.All);
        }

        [PunRPC]
        private void RPCGetRekted()
        {
            if (isDowned) { return; }
            isDowned = true;
            _agent.ResetPath();
            _agent.isStopped = true;
            GetComponent<Collider>().isTrigger = true;
            StartCoroutine(PlayDeathEffect());
            if (photonView.IsMine)
            {
                GameObject deathMarker = PhotonNetwork.Instantiate("DeathMarker", gameObject.transform.position, Quaternion.identity);
                deathMarker.GetComponent<DeathMarker.DeathMarker>().SetCharacter(this);
            }
        }

        public void Revive()
        {
            photonView.RPC("RPCRevive", RpcTarget.All);
        }

        [PunRPC]
        private void RPCRevive()
        {
            _agent.isStopped = false;
            GetComponent<Collider>().isTrigger = false;
            isDowned = false;
            _shader.SetFloat("PercentDisintegrated", 0f);
            OnRevive?.Invoke();
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

        // Network Sync

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            else
            {
                _networkLocation = (Vector3)stream.ReceiveNext();
                _networkRotation = (Quaternion)stream.ReceiveNext();
            }
        }

        public void SetSpeed(float speed)
        {
            photonView.RPC("RPCSetSpeed", RpcTarget.All, speed);
        }

        [PunRPC]
        private void RPCSetSpeed(float speed)
        {
            _agent.speed = speed;
        }

        public void Teleport(Vector3 pos)
        {
            photonView.RPC("RPCTeleported", RpcTarget.All, pos);
        }

        [PunRPC]
        private void RPCTeleported(Vector3 pos)
        {
            _agent.Warp(pos);
            transform.position = pos;
        }


        // Helper Methods

        private IEnumerator PlayDeathEffect()
        {
            float deadValue = 0f;
            while (deadValue < 1f)
            {
                if (!isDowned)
                {
                    _shader.SetFloat("PercentDisintegrated", 0f);
                    yield break;
                }
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