namespace RunlingRun.ReviveProjectile
{
    using UnityEngine;

    public class Rotato : MonoBehaviour
    {
        private void FixedUpdate()
        {
            transform.Rotate(new Vector3(0f, 10f, 0f), Space.Self);
        }
    }
}
