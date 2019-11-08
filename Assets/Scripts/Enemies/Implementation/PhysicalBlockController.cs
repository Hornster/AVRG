using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Factories;
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
        /// Renderer of the object, stores the material.
        /// </summary>
        private Renderer _renderer;
        /// <summary>
        /// Rigidbody component of the object.
        /// </summary>
        private Rigidbody _rigidBody;
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

            _renderer = GetComponent<Renderer>();
            if (_renderer == null)
            {
                throw new Exception("Error - renderer (and material) not found for this physical block.");
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
            _renderer.material.color = ColorHelper.GetHighlightColor(team);
        }
        /// <summary>
        /// Resets the color of the material, removing the highlight.
        /// </summary>
        public void DeselectObject()
        {
            _renderer.material.color = Color.white;
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
        /// <summary>
        /// Retrieves position of the gameobject.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }
        /// <summary>
        /// Returns the velocity of the block.
        /// </summary>
        /// <returns></returns>
        public Vector3 GetVelocity()
        {
            return _rigidBody.velocity;
        }
        /// <summary>
        /// Applies new data to obstacle.
        /// </summary>
        /// <param name="newData">Data to be applied to obstacle.</param>
        public void Mutate(ObstacleIniData newData)
        {
            gameObject.transform.position = newData.Position;
            gameObject.transform.localScale = newData.Scale;
            gameObject.transform.rotation = newData.Rotation;
            if (newData.ParentTransform != null)
            {
                gameObject.transform.SetParent(newData.ParentTransform);
            }
        }
        /// <summary>
        /// Enables the gameobject of the obstacle.
        /// </summary>
        public void Enable()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// Disables the gameobject of the obstacle.
        /// </summary>
        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
