namespace RunlingRun.Characters
{
    using System.Collections;
    using Loadouts;
    using Photon.Pun;
    using UnityEngine;

    public class Character : MonoBehaviourPun
    {
        [HideInInspector]
        public float MoveSpeed;
        public bool isDowned = false;
        [HideInInspector]
        public CharacterLoadout Stats;

        // Event Management - End

        private void Awake()
        {
            // TODO: Change out for network id of player
            Stats = new CharacterLoadout("Player1");
            ApplyStats();
        }

        private void ApplyStats()
        {
            Stats.moveSpeedStat.Apply(this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F) && photonView.IsMine)
            {
                Revive();
            }
        }

        public void GetRekted()
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Collider>().enabled = false;
            isDowned = true;
        }

        public void Revive()
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            GetComponent<Collider>().enabled = true;
            isDowned = false;
        }
    }
}