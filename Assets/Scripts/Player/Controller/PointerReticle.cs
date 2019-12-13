using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.Controller
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Reticle of the controller pointer.
    /// </summary>
    public class PointerReticle : MonoBehaviour
    {
        /// <summary>
        /// The camera that will be seeing the reticle.
        /// </summary>
        [SerializeField] private Camera _camera = null;
        [SerializeField] private SpriteRenderer _reticleRenderer = null;
        /// <summary>
        /// Sprite shown by the reticle if currently pointed object is not interactable.
        /// </summary>
        [SerializeField] private Sprite _noInteractionSprite = null;
        /// <summary>
        /// Sprite shown by the reticle if currently pointed object is  interactable.
        /// </summary>
        [SerializeField] private Sprite _interactionSprite = null;
        private void Awake()
        {
            Pointer.RegisterOnPointerUpdate(UpdateSprite);
            Pointer.RegisterSetReticleEnabled(ToggleReticle);
        }
        private void Update()
        {
            transform.LookAt(_camera.gameObject.transform);
        }

        private void OnDestroy()
        {

        }
        /// <summary>
        /// Toggles the reticle.
        /// </summary>
        /// <param name="isEnabled">Pass true to enable reticle. False to disable.</param>
        private void ToggleReticle(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }
        /// <summary>
        /// Event handler. Updates reticle sprite accordingly to type of the object currently pointed at.
        /// </summary>
        /// <param name="hitPoint">The point in world space where the currently pointed at object was hit.</param>
        /// <param name="hitGameObject">The ref to pointed at game object.</param>
        private void UpdateSprite(Vector3 hitPoint, GameObject hitGameObject)
        {
            transform.position = hitPoint;

            _reticleRenderer.sprite = hitGameObject != null ? _interactionSprite : _noInteractionSprite;
        }
    }
}
