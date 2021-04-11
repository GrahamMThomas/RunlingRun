namespace RunlingRun.Player.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MouseController : MonoBehaviour
    {
        public delegate void SetTargetPosition(Vector3 pos);
        public ParticleSystem ClickOnMapEffect;
        public event SetTargetPosition ClickedOnMap;
        private Vector3? _abilityTarget;

        // Special management for scene view UI clicking        
#pragma warning disable IDE0044
        private int fingerID = -1;
#pragma warning restore IDE0044


#pragma warning disable UNT0001
        private void Awake()
        {
#if !UNITY_EDITOR
            fingerID = 0; 
#endif
        }
#pragma warning restore UNT0001


        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Vector3? clickLocation = GetMouseClickLocation();
                if (clickLocation.HasValue)
                {
                    ClickedOnMap?.Invoke(clickLocation.Value);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                _abilityTarget = GetMouseClickLocation();
            }
        }

        Vector3? GetMouseClickLocation()
        {
            Ray ray = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject(fingerID))
            {
                //pass if we are clicking on part of the UI
            }
            else if (Physics.Raycast(ray, out RaycastHit hit, 1000, 1 << LayerMask.NameToLayer("WalkableTerrain")))
            {
                Vector3 location = hit.point;
                PlayClickedOnMapEffect(location);
                return location;
            }
            return null;
        }

        public IEnumerator WaitForMouseClickLocation(System.Action<Vector3?> callback)
        {
            _abilityTarget = null;
            yield return new WaitUntil(() => _abilityTarget != null);
            callback(_abilityTarget);
            _abilityTarget = null;
        }

        private void PlayClickedOnMapEffect(Vector3 location)
        {
            Vector3 hover = location;
            hover.y += 0.1f;
            ParticleSystem bob = Instantiate(ClickOnMapEffect, hover, Quaternion.Euler(90, 0, 0));
            Destroy(bob.gameObject, 2f);
        }
    }
}

