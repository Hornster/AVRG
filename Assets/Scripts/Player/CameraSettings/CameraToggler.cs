using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player.CameraSettings
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Toggles camera in player gameobject, depending on whether is the game played in editor or on VR set.
    /// </summary>
    public class CameraToggler : MonoBehaviour
    {
        [SerializeField]
        private InputController _inputController;
        [SerializeField]
        private GameObject _regularCamera;
        [SerializeField]
        private GameObject _vrCamera;
        /// <summary>
        /// Called at initialization of the gameobject.
        /// </summary>
        void Awake()
        {
            SelectCamera();
        }
        /// <summary>
        /// Turns off either VR or regular camera, judging by whether is the app running in the editor or not.
        /// </summary>
        private void SelectCamera()
        {
            if (_inputController.ControllerDetected==false)
            {
                _vrCamera.SetActive(false);
            }
            else
            {
                _regularCamera.SetActive(false);
            }
        }

    }
}
