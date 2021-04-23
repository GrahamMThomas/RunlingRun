namespace RunlingRun.ReviveProjectile
{
    using Character;
    using UnityEngine;

    public class Reviver : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerCharacter"))
            {
                GameObject _player = other.gameObject;
                CharacterBehaviour behav = _player.GetComponent<CharacterBehaviour>();
                if (behav.isDowned)
                {
                    behav.Revive();
                }
            }
        }
    }
}