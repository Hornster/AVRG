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
        /// <summary>
        /// Parent gameobject for whole VR camera structure.
        /// </summary>
        [SerializeField]
        private GameObject _vrCameraHierarchy;
        /// <summary>
        /// Reference to canvas showing the GUI seen by the player.
        /// </summary>
        [SerializeField] private Canvas _playerCanvas;
        /// <summary>
        /// Camera component which shall be used to see the player GUI.
        /// </summary>
        [SerializeField] private Camera _vrCameraComponent;
        /// <summary>
        /// Called at initialization of the gameobject.
        /// </summary>
        void Awake()
        {
            _playerCanvas.worldCamera = _vrCameraComponent;
        }

    }
}
