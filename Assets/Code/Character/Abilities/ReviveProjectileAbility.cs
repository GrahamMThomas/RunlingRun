namespace RunlingRun.Character.Abilities
{
    using System.Collections;
    using Photon.Pun;
    using RunlingRun.Player.Controllers;
    using Stats;
    using UnityEngine;


    public class ReviveProjectileAbility : Ability
    {
        public const string Name = "Revive Shot";
        public override string DisplayName { get { return Name; } }

        private MonoBehaviourPun _playerMono;

        public ReviveProjectileAbility(GameObject player, Stat[] attributes) : base(player, attributes)
        {
            _playerMono = player.GetComponent<MonoBehaviourPun>();
        }

        public override IEnumerator Activate()
        {
            Vector3? clickLocation = null;
            MouseController mouseController = GameObject.Find("GameManager").GetComponent<MouseController>();
            yield return _playerMono.StartCoroutine(mouseController.WaitForMouseClickLocation((_clickLocation) => clickLocation = _clickLocation));

            if (clickLocation.HasValue)
            {
                float height = 1f;
                // Spawn in front
                Vector3 spawnLocation = _player.transform.position + _player.transform.forward;
                spawnLocation.y += height;
                GameObject projectile = PhotonNetwork.Instantiate("ReviveProjectile", spawnLocation, _player.transform.rotation);
                Vector3 target = clickLocation.Value;
                // Allow projectile to go up slopes
                target.y += height;
                projectile.GetComponent<ReviveProjectile.ReviveProjectile>().TurnTowardPosition(target);
            }
            yield return null;
        }
    }
}