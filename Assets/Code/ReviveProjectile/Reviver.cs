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
                if (_player.GetComponent<CharacterBehaviour>().isDowned)
                {
                    Debug.Log("Reviving!");
                }
            }
        }
    }
}