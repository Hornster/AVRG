using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

/// <summary>
/// Author: Karol Kozuch
///
/// Communicates with the controller, checks the inputs.
/// </summary>
public class InputController : MonoBehaviour
{
    #region VR Controller
    //TODO add pause caller (back button)
    //TODO add glove switch (pad button)
    private static UnityAction<bool> _onHasController = null;

    private static UnityAction _onTriggerUp = null;
    private static UnityAction _onTriggerPressed = null;
    private static UnityAction _onTriggerDown = null;
    private static UnityAction _onTouchUp = null;
    private static UnityAction _onTouchPressed = null;
    private static UnityAction _onTouchDown = null;
    #endregion VR Controller

    #region Keyboard and Mouse
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
    #endregion Keyboard and Mouse

    public bool ControllerDetected { get; private set; } = false;
    public bool InputAllowed { get; private set; } = false;
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

    void Awake()
    {
        OVRManager.HMDMounted += UserActivated;
        OVRManager.HMDUnmounted += UserDeactivated;
    }
    // Start is called before the first frame update
    void Start()
    {
        //InputAllowed = true;
        if (Application.isEditor)
        {
            InputAllowed = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check for active input
        if (InputAllowed == false)
        {
            return;
        }
        //Check if controller present
        if (ControllerDetected == false)
        {
            CheckDefaultInput();
            CheckMouseInput();
        }
        else
        {
            CheckVRInput();

            ControllerDetected = CheckControllerPresence(ControllerDetected);
        }
    }

    void OnDestroy()
    {
        OVRManager.HMDMounted -= UserActivated;
        OVRManager.HMDUnmounted -= UserDeactivated;
    }
    /// <summary>
    /// Performs checks of the VR controller input.
    /// </summary>
    private void CheckVRInput()
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
    /// <summary>
    /// Performs checks of the default input.
    /// </summary>
    private void CheckDefaultInput()
    {
        DefaultAxesValues = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (ChkIfNonZeroVector(new Vector2(DefaultAxesValues.x, DefaultAxesValues.y)))
        {
            _onDefaultAxesChange?.Invoke(DefaultAxesValues);
        }

        if (Input.GetMouseButtonDown((int)MouseButton.LeftMouse))
        {
            _onMouseLeftPressed?.Invoke();
        }
        else if (Input.GetMouseButtonUp((int)MouseButton.LeftMouse))
        {
            _onMouseLeftUp?.Invoke();
        }
        else if (Input.GetMouseButton((int)MouseButton.LeftMouse))
        {
            _onMouseLeftDown?.Invoke();
        }

        if (Input.GetMouseButtonDown((int) MouseButton.RightMouse))
        {
            _onMouseRightPressed?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _onEscKeyPressed?.Invoke();
        }
    }
    /// <summary>
    /// Checks input from the mouse.
    /// </summary>
    private void CheckMouseInput()
    {
        MouseAxesValues = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (ChkIfNonZeroVector(MouseAxesValues))
        {
            _onMouseAxesChange?.Invoke(MouseAxesValues);
        }
    }
    /// <summary>
    /// Checks if floating point vector differs from 0.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private bool ChkIfNonZeroVector(Vector2 vector)
    {
        const float detectionThreshold = 0.00001f;
        return vector.x > detectionThreshold
                || vector.x < -detectionThreshold
                || vector.y > detectionThreshold
                || vector.y < -detectionThreshold;
    }
    /// <summary>
    /// Checks whether has the presence of a controller changed.
    /// </summary>
    /// <param name="currentControllerState"></param>
    /// <returns></returns>
    private bool CheckControllerPresence(bool currentControllerState)
    {
        bool isControllerPresent = OVRInput.IsControllerConnected(OVRInput.Controller.Remote)
                                   || OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote)
                                   || OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote);

        if (isControllerPresent == currentControllerState)
        {
            return currentControllerState;
        }

        _onHasController?.Invoke(isControllerPresent);

        return isControllerPresent;
    }

    #region EventRegistering
    public static void RegisterOnHasController(UnityAction<bool> action)
    {
        _onHasController += action;
    }

    public static void RegisterOnTriggerUp(UnityAction action)
    {
        _onTriggerUp += action;
    }

    public static void RegisterOnTriggerDown(UnityAction action)
    {
        _onTriggerDown += action;
    }
    public static void RegisterOnTriggerPressed(UnityAction action)
    {
        _onTriggerPressed += action;
    }
    /// <summary>
    /// Register handler - VR Touch pad has just been released.
    /// </summary>
    /// <param name="action"></param>
    public static void RegisterOnTouchUp(UnityAction action)
    {
        _onTouchUp += action;
    }
    /// <summary>
    /// Register handler - VR Touch pad is being constantly pressed down.
    /// </summary>
    /// <param name="action"></param>
    public static void RegisterOnTouchDown(UnityAction action)
    {
        _onTouchDown += action;
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
    /// <summary>
    /// Register handler - LMB was released last frame.
    /// </summary>
    /// <param name="action"></param>
    public static void RegisterOnMouseAxesChange(UnityAction<Vector2> action)
    {
        _onMouseAxesChange += action;
    }
    /// <summary>
    /// Register handler - LMB was pressed last frame (non-continuous).
    /// </summary>
    /// <param name="action"></param>
    public static void RegisterOnMouseLeftPressed(UnityAction action)
    {
        _onMouseLeftPressed += action;
    }
    /// <summary>
    /// Register handler - LMB is being constantly pressed down.
    /// </summary>
    /// <param name="action"></param>
    public static void RegisterOnMouseLeftDown(UnityAction action)
    {
        _onMouseLeftDown += action;
    }

    /// <summary>
    /// Register handler - RMB was pressed last frame (non-continuous).
    /// </summary>
    /// <param name="action"></param>
    public static void RegisterOnMouseRightPressed(UnityAction action)
    {
        _onMouseRightPressed += action;
    }
    public static void RegisterOnMouseLeftUp(UnityAction action)
    {
        _onMouseLeftUp += action;
    }

    public static void RegisterOnEscKeyPressed(UnityAction action)
    {
        _onEscKeyPressed += action;
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
