namespace RunlingRun.Managers
{
    using UnityEngine;

    public class CameraManager : MonoBehaviour
    {
        public float Zoom = 20f;
        public bool IsTracking = false;
        private GameObject targetPlayer;

        private Vector3 Offset;
        private readonly float SmoothTime = 0.3f;
        private Vector3 velocity = Vector3.zero;


        // --- Singleton Pattern
        private static CameraManager _instance = null;
        public static CameraManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
            else if (_instance != this) { Destroy(gameObject); }
        }
        // --- End Singleton Pattern

        void Start()
        {
            Offset = new Vector3(0, 1, 1);
        }

        public void StartTracking()
        {
            targetPlayer = CharacterSelectionManager.Instance.CurrentPlayer;

            transform.position = GetTargetPosition();
            transform.LookAt(targetPlayer.transform);
            IsTracking = true;
        }

        public void StopTracking()
        {
            IsTracking = false;
        }


        private void LateUpdate()
        {
            if (!IsTracking)
            {
                return;
            }

            // update position
            Vector3 targetPosition = GetTargetPosition();
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
        }

        private Vector3 GetTargetPosition()
        {
            return targetPlayer.transform.position + Offset * Zoom;
        }
    }
}
