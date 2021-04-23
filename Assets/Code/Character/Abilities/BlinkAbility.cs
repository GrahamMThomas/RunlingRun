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
        public const string Name = "Blink Ability";
        public override string DisplayName { get { return Name; } }
        public float BlinkDistance;
        private readonly MonoBehaviour _playerMono;
        private readonly CharacterBehaviour _character;
        private readonly GameObject _blinkEffect;

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
                yield break;
            }
            if (CurrentCharges <= 0)
            {
                yield break;
            }

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
                    GameObject fromEffect = Object.Instantiate(_blinkEffect, oldPos, Quaternion.Euler(90, 0, 0));
                    Object.Destroy(fromEffect, 2f);
                    GameObject toEffect = Object.Instantiate(_blinkEffect, targetPos, Quaternion.Euler(90, 0, 0));
                    Object.Destroy(toEffect, 2f);
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