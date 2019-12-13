using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Shared.Enums;
using UnityEngine;

namespace Assets.Scripts.Player.Controller
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Toggles between 3D and canvas pointers, enables them and disables.
    /// </summary>
    public class PointerToggler : MonoBehaviour
    {
        /// <summary>
        /// What pointer shall be activated upon application start, by default?
        /// </summary>
        [SerializeField] private PointerType _startingPointer;
        /// <summary>
        /// Reference to pointer used with 3D objects on the scene.
        /// </summary>
        [SerializeField]
        private Pointer _sceneObjectsPointer = null;
        /// <summary>
        /// Reference to pointer used with canvases.
        /// </summary>
        [SerializeField]
        private CanvasPointer _canvasPointer = null;
        

        private void Start()
        {
            SwitchToPointer(_startingPointer);
        }
        /// <summary>
        /// Switches from current pointer to the one of provided type.
        /// </summary>
        /// <param name="type">Type of pointer that will be switched to.</param>
        public void SwitchToPointer(PointerType type)
        {
            switch (type)
            {
                case PointerType.ThreeDScenePointer:
                    _sceneObjectsPointer.enabled = true;
                    _canvasPointer.enabled = false;
                    break;
                case PointerType.CanvasPointer:
                    _sceneObjectsPointer.enabled = false;
                    _canvasPointer.enabled = true;
                    break;
            }
        }
        /// <summary>
        /// Disables all active pointers.
        /// </summary>
        public void DisableAllPointers()
        {
            _sceneObjectsPointer.enabled = false;
            _canvasPointer.enabled = false;
        }
        /// <summary>
        /// Prompts the 3D pointer to change its color, accordingly to new glove equipped by the player.
        /// </summary>
        /// <param name="newSetGlove"></param>
        public void Switch3DPointerColor(ProjectileTypeEnum newSetGlove)
        {
            _sceneObjectsPointer.SwitchColor(newSetGlove);
        }
    }
}
