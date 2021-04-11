namespace RunlingRun.Utilities
{
    using UnityEngine;
    using UnityEngine.AI;

    class NavAgentHelpers
    {
        public static float GetPathLength(NavMeshPath path)
        {
            float lng = 0.0f;

            if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
            {
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }
            }

            return lng;
        }
    }
}