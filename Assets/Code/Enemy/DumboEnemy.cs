namespace RunlingRun.Enemy
{
    using System.Collections;
    using Character;
    using Photon.Pun;
    using UnityEngine;

    public class DumboEnemy : Enemy, IPunObservable
    {
        private Vector3 _moveDirection;
        private new Rigidbody rigidbody;
        private Vector3 _networkLocation;
        private float _networkLag;

        private void Awake()
        {
            Vector3 randomDirection = new Vector3(Random.value, 0, Random.value);
            _moveDirection = randomDirection.normalized;
            rigidbody = GetComponent<Rigidbody>();
        }

        public override void Move()
        {
            if (photonView.IsMine)
            {
                transform.Translate(_moveDirection * MoveSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, _networkLocation, Time.deltaTime * MoveSpeed);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Contains("Wall"))
            {
                Vector3 bounceDir = Vector3.Reflect(_moveDirection, other.contacts[0].normal);
                bounceDir.Set(bounceDir.x, 0, bounceDir.z); // Don't allow movement on Y axis
                _moveDirection = bounceDir;
                transform.transform.rotation = Quaternion.LookRotation(bounceDir, Vector3.up);
                // gameObject.transform.eulerAngles = bounceDir;
            }
            else if (other.gameObject.CompareTag("PlayerCharacter"))
            {
                Kill(other.gameObject.GetComponent<CharacterBehaviour>());
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(rigidbody.position);
                stream.SendNext(rigidbody.rotation);
            }
            else
            {
                _networkLocation = (Vector3)stream.ReceiveNext();

                rigidbody.rotation = (Quaternion)stream.ReceiveNext();

                _networkLag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            }
        }
    }
}

