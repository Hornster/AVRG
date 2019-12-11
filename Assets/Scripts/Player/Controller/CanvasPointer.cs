using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Player.Controller
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Pointer class for use with UI.
    /// </summary>
    public class CanvasPointer : MonoBehaviour
    {
        /// <summary>
        /// The default, max length of the pointer when aiming at canvases.
        /// </summary>
        [SerializeField]
        private float _defaultLength = 3.0f;
        /// <summary>
        /// Renders the beam of the pointer.
        /// </summary>
        [SerializeField]
        private LineRenderer _beamRenderer = null;
        [SerializeField]
        private EventSystem _eventSystem = null;

        [SerializeField] private StandaloneInputModule _inputModule = null;

        private void Update()
        {
            UpdateLength();
        }

        private void UpdateLength()
        {
            _beamRenderer.SetPosition(0, transform.position);
            _beamRenderer.SetPosition(1, GetEnd());
        }

        private Vector3 GetEnd()
        {
            float canvasDistance = GetCanvasDistance();
            Vector3 endPosition = CalculateEnd(_defaultLength);

            if (canvasDistance != 0.0f)
            {
                endPosition = CalculateEnd(canvasDistance);
            }

            return endPosition;
        }

        private float GetCanvasDistance()
        {
            var eventData = new PointerEventData(_eventSystem);
            eventData.position = _inputModule.inputOverride.mousePosition;

            var raycastResults = new List<RaycastResult>();
            _eventSystem.RaycastAll(eventData, raycastResults);

            RaycastResult closestResult = FindFirstRaycast(raycastResults);
            float distance = closestResult.distance;

            distance = Mathf.Clamp(distance, 0.0f, _defaultLength);
            return distance;
        }

        private RaycastResult FindFirstRaycast(List<RaycastResult> raycastResults)
        {
            //The list of raycasts results is already sorted by distance. The first result
            //with gameobject present is the closest one.
            foreach (var raycast in raycastResults)
            {
                if (raycast.gameObject != null)
                {
                    return raycast;
                }
            }

            return new RaycastResult();
        }

        private Vector3 CalculateEnd(float length)
        {
            return transform.position + (transform.forward * length);
        }
    }
}
