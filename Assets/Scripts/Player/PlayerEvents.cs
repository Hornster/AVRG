using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Author: Karol Kozuch
    /// </summary>
    public class PlayerEvents:MonoBehaviour
    {
        #region Events
        //VR Controller
        private static UnityAction<OVRInput.Controller, GameObject> _onControllerChanged = null;
        private static UnityAction _onTriggerUp = null;
        private static UnityAction _onTriggerPressed = null;
        private static UnityAction _onTriggerDown = null;
        private static UnityAction _onTouchUp = null;
        private static UnityAction _onTouchPressed = null;
        private static UnityAction _onTouchDown = null;

        //Mouse, Keyboard
        /// <summary>
        /// Used mainly when in editor.
        /// </summary>
        private static UnityAction<Vector3> _onDefaultAxesChange = null;
        /// <summary>
        /// Used mainly when in editor. Called upon detection of mouse movement.
        /// </summary>
        private static UnityAction<Vector2> _onMouseAxesChange = null;
        /// <summary>
        /// Used mainly when in editor. Called when the left mouse button has been pressed (once).
        /// </summary>
        private static UnityAction _onMouseLeftPressed = null;
        /// <summary>
        /// Used mainly when in editor. Called when the left mouse button has been released.
        /// </summary>
        private static UnityAction _onMouseLeftUp = null;
        /// <summary>
        /// Used mainly when in editor. Called when the left mouse button is constantly being pressed.
        /// </summary>
        private static UnityAction _onMouseLeftDown = null;
        /// <summary>
        /// Used mainly when in editor. Called when the right mouse button has been pressed (once).
        /// </summary>
        private static UnityAction _onMouseRightPressed = null;
        /// <summary>
        /// Used mainly when in editor. Called when the escape key has been pressed.
        /// </summary>
        private static UnityAction _onEscKeyPressed = null;
        #endregion
        #region Anchors

        [SerializeField] private GameObject _leftHandAnchor = null;
        [SerializeField] private GameObject _rightHandAnchor = null;
        [SerializeField] private GameObject _headCenterAnchor = null;
        #endregion

        #region Input

        private Dictionary<OVRInput.Controller, GameObject> _controllers = null;
        private OVRInput.Controller _lastInputSource = OVRInput.Controller.None;
        private OVRInput.Controller _activeControllerState = OVRInput.Controller.None;
        public bool InputAllowed { get; private set; } = true;


        /// <summary>
        /// Vector presenting where did the click occur on the touchpad.
        /// </summary>
        public Vector2 TouchPadAxis { get; private set; } = new Vector2();
        /// <summary>
        /// Value presenting how deeply is the primary trigger pressed.
        /// </summary>
        public float TriggerAxis { get; private set; } = 0.0f;
        /// <summary>
        /// Stores values read from default, raw axes (that is, the horizontal and vertical). X = horizontal, z = vertical. Used in editor.
        /// </summary>
        public Vector3 DefaultAxesValues { get; private set; } = new Vector3();
        /// <summary>
        /// Stores values read from mouse axes.
        /// </summary>
        public Vector2 MouseAxesValues { get; private set; } = new Vector2();
        #endregion

        private void Awake()
        {
            OVRManager.HMDMounted += UserActivated;
            OVRManager.HMDUnmounted += UserDeactivated;

            _controllers = CreateControllers();
        }

        private void Update()
        {
            //Check for active input
            if (InputAllowed == false)
            {
                return;
            }
            //Check if controller present
            CheckForController();
            //Check input source
            CheckInputSource();
            //Check input
            Input();
            //InputController (CheckControllerPresence)
        }
        private void OnDestroy()
        {
            OVRManager.HMDMounted -= UserActivated;
            OVRManager.HMDUnmounted -= UserDeactivated;
        }
        /// <summary>
        /// Checks if checked controller differs from currently stored. If yes - remember new controller
        /// type and notify subscribes for this change.
        /// </summary>
        /// <param name="current">Freshly checked controller type.</param>
        /// <param name="previous">Recently used controller type.</param>
        /// <returns></returns>
        private OVRInput.Controller UpdateController(OVRInput.Controller current, OVRInput.Controller previous)
        {
            if (current == previous)
            {
                return previous;
            }

            GameObject newController = null;
            _controllers.TryGetValue(current, out newController);

            if (newController == null)
            {
                newController = _headCenterAnchor;
            }

            _onControllerChanged?.Invoke(current, newController);

            return current;
        }
        /// <summary>
        /// Checks for the presence of controllers and for what types is the one connected.
        /// </summary>
        private void CheckForController()
        {
            var controllerCheck = _activeControllerState;

            //Left remote,
            if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            {
                controllerCheck = OVRInput.Controller.LTrackedRemote;
            }
            //Right remote,
            if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            {
                controllerCheck = OVRInput.Controller.RTrackedRemote;
            }
            //If no controllers - headset
            if (!OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote) &&
                !OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            {
                controllerCheck = OVRInput.Controller.Touchpad;
            }

            //Notify about controller change
            _activeControllerState = UpdateController(controllerCheck, _activeControllerState);
        }

        private void CheckInputSource()
        {
            _lastInputSource = UpdateController(OVRInput.GetActiveController(), _lastInputSource);
        }

        private void Input()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                _onTriggerPressed?.Invoke();
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
            {
                _onTriggerUp?.Invoke();
            }

            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                _onTriggerDown?.Invoke();
            }

            if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
            {
                _onTouchPressed?.Invoke();
            }

            if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
            {
                _onTouchUp?.Invoke();
            }

            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
            {
                _onTouchDown?.Invoke();
            }

            TouchPadAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            TriggerAxis = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
        }

        private Dictionary<OVRInput.Controller, GameObject> CreateControllers()
        {
            var newControllers = new Dictionary<OVRInput.Controller, GameObject>()
            {
                { OVRInput.Controller.LTrackedRemote, _leftHandAnchor },
                { OVRInput.Controller.RTrackedRemote, _rightHandAnchor },
                { OVRInput.Controller.Touchpad, _headCenterAnchor },

                { OVRInput.Controller.None, _headCenterAnchor }
            };

            return newControllers;
        }


        #region EventRegistering
        public static void RegisterOnControllerChanged(UnityAction<OVRInput.Controller, GameObject> action)
        {
            _onControllerChanged += action;
        }
        /// <summary>
        /// Register handler - Touch Pad was pressed last frame (non-continuous).
        /// </summary>
        /// <param name="action"></param>
        public static void RegisterOnTouchPressed(UnityAction action)
        {
            _onTouchPressed += action;
        }

        public static void RegisterOnDefaultAxesChange(UnityAction<Vector3> action)
        {
            _onDefaultAxesChange += action;
        }
        #endregion EventRegistering


        /// <summary>
        /// Called when user puts on the headset.
        /// </summary>
        public void UserActivated()
        {
            InputAllowed = true;
        }

        /// <summary>
        /// Called when the user takes off the headset.
        /// </summary>
        public void UserDeactivated()
        {
            InputAllowed = false;
        }
    }
}
