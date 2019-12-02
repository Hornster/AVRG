using System;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Factories;
using Assets.Scripts.Match.Entities;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Enemies.Implementation
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Controller for obstacles in form of physical blocks.
    /// </summary>
    public class PhysicalBlockController : Block, IDamageDealer
    {
        /// <summary>
        /// All layers that trigger destruction of this block.
        /// </summary>
        [SerializeField]
        private LayerMask _destroyingLayers;
        /// <summary>
        /// Defines who can receive damage upon collision with this object.
        /// </summary>
        [SerializeField] private LayerMask _dealsDamageToLayers;
        /// <summary>
        /// Highlight color of the selected obstacle.
        /// </summary>
        [SerializeField]
        private Color _selectedColor;
        /// <summary>
        /// Defines resistance of the object towards any forces that work on it.
        /// Force is multiplied by this value.
        /// </summary>
        [SerializeField] private float _forceMultiplier = 0.8f;
        
        /// <summary>
        /// Type of glove the object will react to.
        /// </summary>
        private const ProjectileTypeEnum ObjectType = ProjectileTypeEnum.Physical;
        
        /// <summary>
        /// Checks what object has this block collided with.
        /// </summary>
        /// <param name="collision">Colliding object data.</param>
        void OnCollisionEnter(Collision collision)
        {
            int layerBitValue = 1;
            layerBitValue = layerBitValue << collision.gameObject.layer;
            if ((layerBitValue & _destroyingLayers.value) != 0)
            {
                this.Deactivate();
            }
            else if ((layerBitValue & _dealsDamageToLayers.value) != 0)
            {
                var damageReceiver = collision.gameObject.GetComponent<IDamageReceiver>();

                if (damageReceiver == null)
                {
                    return;
                }

                DealDamage(damageReceiver);
                this.Deactivate();
            }
            
        }
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Normalized force vector.</param>
        /// <param name="value">Value by which the vector shall be multiplied.</param>
        public override void ApplyForce(Vector3 direction, float value)
        {
            direction *= value;
            ApplyForce(direction);
        }
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Direction of force, including its value.</param>
        public override void ApplyForce(Vector3 direction)
        {
            direction *= _forceMultiplier;
            _rigidBody.AddForce(direction);
        }

        /// <summary>
        /// Applies highlight to the object.
        /// </summary>
        /// <param name="team">What team color should the highlight have?</param>
        public override void SelectObject(TeamEnum team)
        {
            _renderer.material.color = ColorHelper.GetHighlightColor(team);
        }
        /// <summary>
        /// Checks if type of object is the same as type of the glove that tried to hook it.
        /// </summary>
        /// <param name="projectileType">Type of hooking glove.</param>
        /// <returns>True if types match, false otherwise.</returns>
        public override bool ChkGloveType(ProjectileTypeEnum projectileType)
        {
            return projectileType == ObjectType;
        }
    }
}
//TODO:
// - Add obstacle - energy siphon. Upon using the glove, generates explosion that will damage the player for quite amount of hp (for example 20%).