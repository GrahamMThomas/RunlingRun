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
            Vector3? blinkLocation = null;
            MouseController mouseController = GameObject.Find("GameManager").GetComponent<MouseController>();
            yield return _playerMono.StartCoroutine(mouseController.WaitForMouseClickLocation((_blinkLocation) => blinkLocation = _blinkLocation));

            if (blinkLocation.HasValue)
            {
                GameObject projectile = PhotonNetwork.InstantiateRoomObject("ReviveProjectile", _player.transform.position, _player.transform.rotation);
                projectile.transform.LookAt(blinkLocation.Value);
            }
            yield return null;
        }
    }
}