namespace RunlingRun.Enemy
{
    using System.Collections;
    using Characters;
    using UnityEngine;

    public class Enemy : MonoBehaviour
    {
        public float MoveSpeed = 5f;

        public virtual void Move()
        {
            // Implement.
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        public void Kill(Character character)
        {
            character.GetRekted();
        }
    }
}