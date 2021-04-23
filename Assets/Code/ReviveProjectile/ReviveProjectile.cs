namespace RunlingRun.ReviveProjectile
{
    using Character;
    using Photon.Pun;
    using UnityEngine;

    public class ReviveProjectile : MonoBehaviourPun
    {
        private float _speed = 10f;

        public void TurnTowardPosition(Vector3 pos)
        {
            photonView.RPC("RPCTurnTowardPosition", RpcTarget.All, pos);
        }

        [PunRPC]
        private void RPCTurnTowardPosition(Vector3 pos)
        {
            transform.LookAt(pos);
        }

        private void FixedUpdate()
        {
            transform.position += transform.forward * Time.deltaTime * _speed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Wall/All"))
            {
                Destroy(gameObject);
            }
        }
    }
}