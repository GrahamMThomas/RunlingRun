namespace RunlingRun.ReviveProjectile
{
    using UnityEngine;

    public class ReviveProjectile : MonoBehaviour
    {
        private float _speed = 10f;

        private void FixedUpdate()
        {
            transform.position += transform.forward * Time.deltaTime * _speed;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Wall/EnemyOnly"))
            {
                Destroy(gameObject);
            }
        }
    }
}