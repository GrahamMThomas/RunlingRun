namespace RunlingRun.UI
{
    using UnityEngine;

    public class LookAtCamera : MonoBehaviour
    {
        // Update is called once per frame
        void LateUpdate()
        {
            Transform cam = Camera.main.transform;
            transform.LookAt(transform.position + cam.rotation * Vector3.forward, cam.rotation * Vector3.up);
        }
    }
}

