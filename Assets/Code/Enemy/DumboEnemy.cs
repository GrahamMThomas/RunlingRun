namespace RunlingRun.Enemy
{
    using System.Collections;
    using Character;
    using UnityEngine;

    public class DumboEnemy : Enemy
    {
        private Vector3 _moveDirection;

        private void Awake()
        {
            Vector3 randomDirection = new Vector3(Random.value, 0, Random.value);
            _moveDirection = randomDirection.normalized;
        }

        public override void Move()
        {
            transform.Translate(_moveDirection * MoveSpeed * Time.deltaTime, Space.World);
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
    }
}

