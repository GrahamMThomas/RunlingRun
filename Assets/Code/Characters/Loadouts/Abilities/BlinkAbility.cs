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

        private const float MaxBlinkAheadDistance = 20f;

        public BlinkAbility(GameObject player, int level) : base(player, level)
        {
            _blinkDistance = level;
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
            Character character = _player.GetComponent<Character>();

            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(targetPos, character.MapGameManager.EndofMapPos, NavMesh.AllAreas, path);
            float distanceToEnd = NavAgentHelpers.GetPathLength(path);
            return distanceToEnd < (character.maxPlayerDistanceToEnd - MaxBlinkAheadDistance);
        }


    }
}