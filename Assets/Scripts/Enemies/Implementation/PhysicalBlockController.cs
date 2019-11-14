using System;
using System.Collections;
using System.Collections.Generic;
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
    public class PhysicalBlockController : MonoBehaviour, IObstacle, IDamageDealer
    {
        /// <summary>
        /// All layers that trigger destruction of this block.
        /// </summary>
        [SerializeField]
        private LayerMask _destroyingLayers;
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
        /// Script that constantly applies constant force.
        /// </summary>
        private SimpleMover _simpleMover;
        /// <summary>
        /// Type of glove the object will react to.
        /// </summary>
        private const ProjectileTypeEnum ObjectType = ProjectileTypeEnum.Physical;
        /// <summary>
        /// Index assigned to poolable obstacle upon activation. Used by the owner pool to recognize the object in collection.
        /// </summary>
        public uint ActivationIndex { get; private set; }

        /// <summary>
        /// Reference to callback that can be used by the obstacle to call its pool when the obstacle has to be
        /// deactivated.
        /// </summary>
        public UnityAction<uint> DeactivationCallback { get; set; }
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

            _simpleMover = GetComponent<SimpleMover>();

            Deactivate();
        }
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
                this.DeactivationCallback(ActivationIndex);
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
            _rigidBody.mass = newData.Mass;

            if (_simpleMover != null)
            {
                _simpleMover.ConstantForce = newData.ConstantForce;
            }

            if (newData.ParentTransform != null)
            {
                gameObject.transform.SetParent(newData.ParentTransform);
            }
        }
        /// <summary>
        /// Enables the gameobject of the obstacle.
        /// </summary>
        public void Activate(uint activationIndex)
        {
            gameObject.SetActive(true);
            ActivationIndex = activationIndex;
        }
        /// <summary>
        /// Disables the gameobject of the obstacle.
        /// </summary>
        public void Deactivate()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Deals damage to the receiver.
        /// </summary>
        /// <param name="receiver">Damage receiving entity.</param>
        public void DealDamage(IDamageReceiver receiver)
        {
            var calculations = Calculations.GetInstance();
            var receiverKineticData = receiver.GetKineticData();

            float thisKineticEnergy = calculations.CalcKineticEnergy(_rigidBody.velocity, _rigidBody.mass);
            float receiverKineticEnergy =
                calculations.CalcKineticEnergy(receiverKineticData.Velocity, receiverKineticData.Mass);
            float velocityAngleFactor = VectorManipulator.CalcAngleBetweenVectors(_rigidBody.velocity, receiverKineticData.Velocity);
            //Head on collision means 0°. Perfect amortization means 180°. -cosa describes this relation, we get cosa.
            velocityAngleFactor = -velocityAngleFactor;
            
            receiverKineticEnergy *= velocityAngleFactor;
            float totalEnergy = thisKineticEnergy + receiverKineticEnergy;

            receiver.ReceiveDamage(totalEnergy);
        }
    }
}
//TODO:
// - remove collider from spawner, rigidbody too if any present. DUN
// - rotate the offset vector by spawner rotation. DUN
// - Use the Destroyer layer in colliders of obstacles and test it. DUN
// - add constant force to the obstacles that increases with overall  playtime. Caps at some point, of course. DUN
// - change the player - they shall move basing on force, too.
// - Implement the match manager. INPR