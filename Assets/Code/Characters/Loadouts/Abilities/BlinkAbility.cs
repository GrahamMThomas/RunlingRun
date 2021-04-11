namespace RunlingRun.Characters.Loadouts.Abilities
{
    using System.Collections;
    using RunlingRun.Player.Controllers;
    using RunlingRun.Utilities;
    using UnityEngine;
    using UnityEngine.AI;

    public class BlinkAbility : Ability
    {
        private float _blinkDistance;
        private MonoBehaviour _playerMono;
        private Character _character;
        private GameObject _blinkEffect;

        private const float MaxBlinkAheadDistance = 20f;

        public BlinkAbility(GameObject player, int level) : base(player, level)
        {
            _character = player.GetComponent<Character>();
            _playerMono = player.GetComponent<MonoBehaviour>();
            _blinkDistance = level;
            _blinkEffect = (GameObject)Resources.Load("ParticleSystems/Blink");
        }

        public override IEnumerator Activate()
        {
            if (!IsUnlocked())
            {
                Debug.Log("Ability is not Unlocked");
                yield break;
            }
            if (isActive)
            {
                Debug.Log("Already using blink...");
                yield break;
            }

            isActive = true;

            Vector3? blinkLocation = null;
            MouseController mouseController = GameObject.Find("GameManager").GetComponent<MouseController>();
            yield return mouseController.StartCoroutine(mouseController.WaitForMouseClickLocation((_blinkLocation) => blinkLocation = _blinkLocation));

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

            isActive = false;
        }

        private bool IsBlinkCheating(Vector3 targetPos)
        {

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(targetPos, _character.MapGameManager.EndofMapPos, NavMesh.AllAreas, path);
            float distanceToEnd = NavAgentHelpers.GetPathLength(path);
            return distanceToEnd < (_character.maxPlayerDistanceToEnd - MaxBlinkAheadDistance);
        }


    }
}