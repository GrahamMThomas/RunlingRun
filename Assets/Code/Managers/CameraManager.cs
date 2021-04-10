namespace RunlingRun.Managers
{
    using UnityEngine;

    public class CameraManager : MonoBehaviour
    {
        public GameManager gameManager;
        public float zoom = 20f;
        private GameObject targetPlayer;

        private Vector3 Offset;
        private readonly float SmoothTime = 0.3f;
        private Vector3 velocity = Vector3.zero;

        void Start()
        {
            targetPlayer = gameManager.GetComponent<GameManager>().CurrentPlayer;
            Offset = new Vector3(0, 1, 1);

            transform.position = GetTargetPosition();
            transform.LookAt(targetPlayer.transform);
        }

        private void LateUpdate()
        {
            // update position
            Vector3 targetPosition = GetTargetPosition();
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        }

        private Vector3 GetTargetPosition()
        {
            return targetPlayer.transform.position + Offset * zoom;
        }
    }
}
