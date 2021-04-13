namespace RunlingRun.Map
{
    using System.Collections;
    using System.Collections.Generic;
    using RunlingRun.Character;
    using UnityEngine;

    public class LevelUpZone : MonoBehaviour
    {
        private readonly List<string> gotPoints = new List<string>();
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerCharacter"))
            {
                if (!gotPoints.Contains(other.gameObject.name))
                {
                    other.gameObject.GetComponent<CharacterLoadout>().LevelUp();
                    gotPoints.Add(other.gameObject.name);
                }
            }
        }
    }
}

