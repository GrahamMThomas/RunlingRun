namespace RunlingRun.Character.Abilities
{
    using System.Collections;
    using RunlingRun.Managers;
    using RunlingRun.Player.Controllers;
    using RunlingRun.Utilities;
    using Stats;
    using UnityEngine;
    using UnityEngine.AI;

    public class BlinkAbility : Ability
    {
        private readonly float _blinkDistance;

        private readonly MonoBehaviour _playerMono;
        private readonly CharacterBehaviour _character;
        private readonly GameObject _blinkEffect;

        private const float MaxBlinkAheadDistance = 20f;

        public BlinkAbility(GameObject player, Stat[] attributes) : base(player, attributes)
        {
            BlinkDistanceStat distanceStat = (BlinkDistanceStat)attributes[0];
            BlinkChargesStat chargeStat = (BlinkChargesStat)attributes[0];

            _blinkDistance = distanceStat.GetMaxBlinkDistance();
            MaxCharges = chargeStat.GetMaxBlinkCharges();

            _character = player.GetComponent<CharacterBehaviour>();
            _playerMono = player.GetComponent<MonoBehaviour>();

            _blinkEffect = (GameObject)Resources.Load("ParticleSystems/Blink");
        }

        public override IEnumerator Activate()
        {
            if (!IsUnlocked)
            {
                Debug.Log("Ability is not Unlocked");
                yield break;
            }
            if (IsActive)
            {
                Debug.Log("Already using blink...");
                yield break;
            }

            IsActive = true;

            Vector3? blinkLocation = null;
            MouseController mouseController = GameObject.Find("GameManager").GetComponent<MouseController>();
            yield return _playerMono.StartCoroutine(mouseController.WaitForMouseClickLocation((_blinkLocation) => blinkLocation = _blinkLocation));

            if (blinkLocation.HasValue)
            {
                Debug.Log("Activate Blink!");
                NavMeshAgent navAgent = _player.GetComponent<NavMeshAgent>();
                Vector3 oldPos = _player.transform.position;
                Vector3 direction = blinkLocation.Value - oldPos;
                Vector3 relativeMovement = Vector3.ClampMagnitude(direction, _blinkDistance);
                Vector3 targetPos = _player.transform.position + relativeMovement;
                if (IsBlinkCheating(targetPos))
                {
                    Debug.Log("Tried to cheat or Path Invalid");
                }
                else
                {
                    navAgent.Warp(targetPos);
                    GameObject fromEffect = Object.Instantiate(_blinkEffect, oldPos, Quaternion.Euler(90, 0, 0));
                    Object.Destroy(fromEffect, 2f);
                    GameObject toEffect = Object.Instantiate(_blinkEffect, targetPos, Quaternion.Euler(90, 0, 0));
                    Object.Destroy(toEffect, 2f);
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