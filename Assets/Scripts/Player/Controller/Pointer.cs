using Assets.Scripts.Shared.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player.Controller
{/// <summary>
/// Author: Karol Kozuch
/// Pointer for the controller in game.
/// </summary>
    public class Pointer : MonoBehaviour
    {
        private static UnityAction<Vector3, GameObject> _onPointerUpdate = null;
        /// <summary>
        /// Used to turn on and off the reticle of the pointer.
        /// </summary>
        private static UnityAction<bool> _setReticleEnabled = null;


        [SerializeField]
        private float _rayLength = 10.0f;
        [SerializeField]
        private LineRenderer _beamRenderer = null;
        [SerializeField]
        private LayerMask _everythingMask = 0;
        [SerializeField]
        private LayerMask _interactableMask = 0;
        /// <summary>
        /// The color of the beam at the base (origin) - energy glove version.
        /// </summary>
        [SerializeField] private Color _beamStartColorEnergy = Color.cyan;
        /// <summary>
        /// The color of the beam at the base (origin) - physics glove version.
        /// </summary>
        [SerializeField] private Color _beamStartColorPhysic = Color.grey;
        /// <summary>
        /// The color of the beam at its end.
        /// </summary>
        [SerializeField] private Color _beamEndColor = new Color(0.0f,0.0f,0.0f,0.0f);

        private Color _currentSelectedColor;

        private GameObject _currentlyPointedObject = null;
        private Transform _origin = null;

        private void Awake()
        {
            PlayerEvents.RegisterOnControllerChanged(UpdateOrigin);
        }

        private void OnDisable()
        {
            _setReticleEnabled.Invoke(false);
            _beamRenderer.enabled = false;
        }
        private void OnEnable()
        {
            SetLineColor();
            _setReticleEnabled.Invoke(true);
            _beamRenderer.enabled = true;
        }
        private void Start()
        {
            _currentSelectedColor = _beamStartColorPhysic;
            SetLineColor();
        }

        private void LateUpdate()
        {
            if (_origin == null)
            {
                //Seems like the origin has not yet been assigned.
                Debug.Log("The origin of the pointer hasn't yet been assigned. If you see this message more than once - something's wrong.");
                return;
            }
            Vector3 hitPoint = UpdateLine();
            _currentlyPointedObject = UpdatePointerStatus();

            _onPointerUpdate?.Invoke(hitPoint, _currentlyPointedObject);
        }
        /// <summary>
        /// Checks if it collided with anything.
        /// If yes, returns collision point. If no - returns origin translated by ray max length.
        /// </summary>
        /// <returns></returns>
        private Vector3 UpdateLine()
        {
            RaycastHit hit = CastRay(ref _everythingMask);

            Vector3 endPosition = _origin.position + (_origin.forward * _rayLength);
            if (hit.collider != null)
            {
                endPosition = hit.point;
            }

            _beamRenderer.SetPosition(0, _origin.position);
            _beamRenderer.SetPosition(1, endPosition);

            return endPosition;
        }
        /// <summary>
        /// Casts the ray using distance and masks as parameters.
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        private RaycastHit CastRay(ref LayerMask layer)
        {
            Ray ray = new Ray(_origin.position, _origin.forward);
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit, _rayLength, layer);

            return hit;
        }
        /// <summary>
        /// Sets the color of the pointer line.
        /// </summary>
        private void SetLineColor()
        {
            if (_beamRenderer == null)
            {
                return;
            }

            _beamRenderer.startColor = _currentSelectedColor;
            _beamRenderer.endColor = _beamEndColor;
        }
        /// <summary>
        /// Checks if the pointer has hit anything interactable.
        /// </summary>
        /// <returns>Game object of the interactable entity or null if none hit.</returns>
        private GameObject UpdatePointerStatus()
        {
            RaycastHit hit = CastRay(ref _interactableMask);

            return hit.collider != null ? hit.collider.gameObject : null;
        }

        private void OnDestroy()
        {

        }
        /// <summary>
        /// Updates the origin of the pointer.
        /// </summary>
        /// <param name="controllerType">Type of new controller.</param>
        /// <param name="controllerObject">Object which data shall be used from.</param>
        private void UpdateOrigin(OVRInput.Controller controllerType, GameObject controllerObject)
        {
            _origin = controllerObject.transform;

            _beamRenderer.enabled = controllerType != OVRInput.Controller.Touchpad;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public static void RegisterOnPointerUpdate(UnityAction<Vector3, GameObject> action)
        {
            _onPointerUpdate += action;
        }
        /// <summary>
        /// Registers event handler for toggling reticle. For example if pointer is disabled, this will be called to disable the reticle as well.
        /// </summary>
        /// <param name="action"></param>
        public static void RegisterSetReticleEnabled(UnityAction<bool> action)
        {
            _setReticleEnabled += action;
        }
        /// <summary>
        /// Changes color of the beam, accordingly to new equipped glove.
        /// </summary>
        /// <param name="newSetGlove"></param>
        public void SwitchColor(ProjectileTypeEnum newSetGlove)
        {
            switch (newSetGlove)
            {
                case ProjectileTypeEnum.Physical:
                    _beamRenderer.startColor = _beamStartColorPhysic;
                    break;
                case ProjectileTypeEnum.Energy:
                    _beamRenderer.startColor = _beamStartColorEnergy;
                    break;
            }

            _currentSelectedColor = _beamRenderer.startColor;
        }
    }
}
