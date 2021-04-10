namespace RunlingRun.Player.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class MouseController : MonoBehaviour
    {
        public delegate void SetTargetPosition(Vector3 pos);
        public ParticleSystem ClickOnMapEffect;
        public event SetTargetPosition ClickedOnMap;
        private int fingerID = -1;
        private void Awake()
        {
#if !UNITY_EDITOR
            fingerID = 0; 
#endif
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3? clickLocation = GetMouseClickLocation();
                if (clickLocation.HasValue)
                {
                    ClickedOnMap?.Invoke(clickLocation.Value);
                }
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

        private void PlayClickedOnMapEffect(Vector3 location)
        {
            Vector3 hover = location;
            hover.y += 0.1f;
            ParticleSystem bob = Instantiate(ClickOnMapEffect, hover, Quaternion.Euler(90, 0, 0));
            Destroy(bob.gameObject, 2f);
        }
    }
}

