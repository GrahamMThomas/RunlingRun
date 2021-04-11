namespace RunlingRun.Enemy
{
    using System.Collections;
    using Characters;
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
            transform.Translate(_moveDirection * MoveSpeed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag.Contains("Wall"))
            {
                Vector3 bounceDir = Vector3.Reflect(_moveDirection, other.contacts[0].normal);
                bounceDir.Set(bounceDir.x, 0, bounceDir.z); // Don't allow movement on Y axis
                _moveDirection = bounceDir;
            }
            else if (other.gameObject.CompareTag("PlayerCharacter"))
            {
                Kill(other.gameObject.GetComponent<Character>());
                transform.localScale = new Vector3(2f, 2f, 2f);
            }
        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}

