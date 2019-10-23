using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Author: Karol Kozuch
///
/// Communicates with the controller, checks the inputs.
/// </summary>
public class InputController : MonoBehaviour
{
    #region VR Controller
    private static UnityAction<bool> _onHasController = null;

    private static UnityAction _onTriggerUp = null;
    private static UnityAction _onTriggerDown = null;
    private static UnityAction _onTouchUp = null;
    private static UnityAction _onTouchDown = null;
    #endregion VR Controller

    #region Keyboard and Mouse
    /// <summary>
    /// Used mainly when in editor.
    /// </summary>
    private static UnityAction _onDefaultAxesChange = null;

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

    void Awake()
    {
        OVRManager.HMDMounted += UserActivated;
        OVRManager.HMDUnmounted += UserDeactivated;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputAllowed == false)
        {
            return;
        }

        CheckVRInput();

        ControllerDetected = CheckControllerPresence(ControllerDetected);
    }

    void OnDestroy()
    {
        OVRManager.HMDMounted -= UserActivated;
        OVRManager.HMDUnmounted -= UserDeactivated;
    }

    private void CheckVRInput()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            _onTriggerDown?.Invoke();
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            _onTriggerUp?.Invoke();
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad))
        {
            _onTouchDown?.Invoke();
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        {
            _onTouchUp?.Invoke();
        }

        TouchPadAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
        TriggerAxis = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger);
    }

    private void CheckDefaultInput()
    {
        DefaultAxesValues = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if(DefaultAxesValues.x != 0.0f || DefaultAxesValues.y != 0.0f)
        {
            _onDefaultAxesChange?.Invoke();
        }
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

    public static void RegisterOnTouchUp(UnityAction action)
    {
        _onTouchUp += action;
    }

    public static void RegisterOnTouchDown(UnityAction action)
    {
        _onTouchDown += action;
    }

    public static void RegisterOnDefaultAxesChange(UnityAction action)
    {
        _onDefaultAxesChange = action;
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
