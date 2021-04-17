namespace RunlingRun.Map
{
    using System.Collections;
    using System.Collections.Generic;
    using RunlingRun.Character;
    using UnityEngine;

    public class ExperienceZone : MonoBehaviour
    {
        public int ExpAwarded = 1;
        private readonly List<string> gotPoints = new List<string>();
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PlayerCharacter"))
            {
                if (!gotPoints.Contains(other.gameObject.name))
                {
                    other.gameObject.GetComponent<CharacterBehaviour>().AwardExp(ExpAwarded);
                    gotPoints.Add(other.gameObject.name);
                }
            }
        }
    }
}

