namespace RunlingRun.Character.Abilities
{
    using System.Collections;
    using Photon.Pun;
    using RunlingRun.Managers;
    using RunlingRun.Player.Controllers;
    using RunlingRun.Utilities;
    using Stats;
    using UnityEngine;
    using UnityEngine.AI;

    public class BlinkAbility : Ability
    {
        public const string Name = "Blink Ability";
        public override string DisplayName { get { return Name; } }
        public float BlinkDistance;
        private readonly MonoBehaviour _playerMono;
        private readonly CharacterBehaviour _character;

        private const float MaxBlinkAheadDistance = 20f;

        public BlinkAbility(GameObject player, Stat[] attributes) : base(player, attributes)
        {
            BlinkDistanceStat distanceStat = (BlinkDistanceStat)Attributes[0];
            BlinkChargesStat chargeStat = (BlinkChargesStat)Attributes[1];

            chargeStat.Apply(this);
            distanceStat.Apply(this);
            Cooldown = 5f;

            _character = player.GetComponent<CharacterBehaviour>();
            _playerMono = player.GetComponent<MonoBehaviour>();
        }

        public override IEnumerator Activate()
        {
            if (!CanCast()) { yield break; }

            IsActive = true;

            Vector3? blinkLocation = null;
            MouseController mouseController = GameObject.Find("GameManager").GetComponent<MouseController>();
            yield return _playerMono.StartCoroutine(mouseController.WaitForMouseClickLocation((_blinkLocation) => blinkLocation = _blinkLocation));

            if (blinkLocation.HasValue)
            {
                Debug.Log("Activate Blink!");
                CharacterBehaviour behav = _player.GetComponent<CharacterBehaviour>();
                Vector3 oldPos = _player.transform.position;
                Vector3 direction = blinkLocation.Value - oldPos;
                Vector3 relativeMovement = Vector3.ClampMagnitude(direction, BlinkDistance);
                Vector3 targetPos = _player.transform.position + relativeMovement;
                if (IsBlinkCheating(targetPos))
                {
                    Debug.Log("Tried to cheat or Path Invalid");
                }
                else
                {
                    behav.Teleport(targetPos);
                    GameObject fromEffect = PhotonNetwork.Instantiate("ParticleSystems/Blink", oldPos, Quaternion.Euler(0, 0, 0));
                    GameObject toEffect = PhotonNetwork.Instantiate("ParticleSystems/Blink", targetPos, Quaternion.Euler(0, 0, 0));
                    SpendAbilityCharge();
                }
            }
            else
            {
                Debug.Log("Cancelled Blink");
            }

            IsActive = false;
        }

        private bool IsBlinkCheating(Vector3 targetPos)
        {

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(targetPos, GameManager.Instance.EndofMapPos, NavMesh.AllAreas, path);
            float distanceToEnd = NavAgentHelpers.GetPathLength(path);
            return distanceToEnd < (_character.maxPlayerDistanceToEnd - MaxBlinkAheadDistance);
        }
    }
}