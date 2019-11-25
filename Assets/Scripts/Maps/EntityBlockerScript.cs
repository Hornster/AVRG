using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.Maps
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Changes color of blocking objects when the blocked by them object gets nearby.
    /// </summary>
    public class EntityBlockerScript : MonoBehaviour
    {
        /// <summary>
        /// Renderer of the gameobject. Used to change color.
        /// </summary>
        [SerializeField] private Renderer _renderer;
        /// <summary>
        /// Transform of blocked entity.
        /// </summary>
        [SerializeField] private Transform _blockedEntityTransform;
        /// <summary>
        /// The border distance of blocked entity from the blocker. When entity gets closer than this, the
        /// blocker starts reacting. 
        /// </summary>
        [SerializeField] private float _reactionDistance = 3.0f;
        /// <summary>
        /// Normal vector used for checking the distance of the blocked object. Is rotated
        /// with the blocker to allow usage of any angle.
        /// </summary>
        [SerializeField] private Vector3 _normalVector = Vector3.forward;
        /// <summary>
        /// Color intensity the blocker will take when the blocked object is further than the reaction distance.
        /// </summary>
        [SerializeField] private float _smallestColorIntensity = 0.1f;
        /// <summary>
        /// Color intensity the blocker will take when the blocked object is touching the blocker.
        /// </summary>
        [SerializeField] private float _biggestColorIntensity = 0.4f;
        /// <summary>
        /// The reference color of the blocker.
        /// </summary>
        [SerializeField] private Color _blockerColor = Color.red;
        /// <summary>
        /// Allows for usage of multiplication instead of division. Basically 1/reactionFactor.
        /// </summary>
        private float _reactionDistanceFactor;

        void Start()
        {
            if (_reactionDistance <= 0.0f)
            {
                throw new Exception("Error: _reaction distance cannot be lesser or equal to 0!");
            }

            _reactionDistanceFactor = 1 / _reactionDistance;
            _normalVector = VectorManipulator.RotateVector(gameObject.transform.rotation, _normalVector);

            if (_renderer == null)
            {
                throw new Exception("Error: EntityBlockerScript has no renderer attached!");
            }

            if (_blockedEntityTransform == null)
            {
                throw new Exception(
                    "Error: EntityBlockerScript has no blocked entity transform attached. How is it supposed to check for distance?");
            }

            _renderer.material.color = _blockerColor * _smallestColorIntensity;
        }

        void Update()
        {
            Vector3 distanceVector = transform.position - _blockedEntityTransform.position;
            float distance = VectorManipulator.CastVectorOntoVector(distanceVector, _normalVector).magnitude;
            distance = Math.Abs(distance);

            if (distance <= _reactionDistance)
            {
                float colorScale = 1 - distance * _reactionDistanceFactor;
                colorScale *= _biggestColorIntensity;

                if (colorScale < _smallestColorIntensity)
                {
                    colorScale = _smallestColorIntensity;
                }
                Debug.DrawRay(transform.position, _normalVector, Color.magenta);
                _renderer.material.color = _blockerColor * colorScale;
                
            }
        }
    }
}
