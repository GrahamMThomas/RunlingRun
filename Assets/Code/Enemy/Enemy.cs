namespace RunlingRun.Enemy
{
    using Character;
    using UnityEngine;

    public abstract class Enemy : MonoBehaviour
    {
        public float MoveSpeed = 5f;

        public abstract void Move();

        public void Die()
        {
            Destroy(gameObject);
        }

        public void Kill(CharacterBehaviour character)
        {
            character.GetRekted();
        }
    }
}