using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.Enemies.Implementation
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Controller for obstacles in form of physical blocks.
    /// </summary>
    public class PhysicalBlockController : MonoBehaviour, IObstacle
    {
        /// <summary>
        /// Highlight color of the selected obstacle.
        /// </summary>
        [SerializeField]
        private Color _selectedColor;
        /// <summary>
        /// Rigidbody component of the object.
        /// </summary>
        private Rigidbody _rigidBody;
        /// <summary>
        /// Material of the object.
        /// </summary>
        private Material _material;
        /// <summary>
        /// Type of glove the object will react to.
        /// </summary>
        private const ProjectileTypeEnum ObjectType = ProjectileTypeEnum.Physical;
        // Start is called before the first frame update
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            if (_rigidBody == null)
            {
                throw new Exception("Error - rigid body not found in physical block.");
            }

            _material = GetComponent<Material>();
            if (_material == null)
            {
                throw new Exception("Error - material not found for this physical block.");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Normalized force vector.</param>
        /// <param name="value">Value by which the vector shall be multiplied.</param>
        public void ApplyForce(Vector3 direction, float value)
        {
            direction *= value;
            ApplyForce(direction);
        }
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Direction of force, including its value.</param>
        public void ApplyForce(Vector3 direction)
        {
            _rigidBody.AddForce(direction);
        }

        /// <summary>
        /// Applies highlight to the object.
        /// </summary>
        /// <param name="team">What team color should the highlight have?</param>
        public void SelectObject(TeamEnum team)
        {
            _material.color = ColorHelper.GetHighlightColor(team);
        }
        /// <summary>
        /// Resets the color of the material, removing the highlight.
        /// </summary>
        public void DeselectObject()
        {
            _material.color = Color.white;
        }
        /// <summary>
        /// Checks if type of object is the same as type of the glove that tried to hook it.
        /// </summary>
        /// <param name="projectileType">Type of hooking glove.</param>
        /// <returns>True if types match, false otherwise.</returns>
        public bool ChkGloveType(ProjectileTypeEnum projectileType)
        {
            return projectileType == ObjectType;
        }
    }
}
