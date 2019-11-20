using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Enemies.Implementation;
using Assets.Scripts.Factories;
using Assets.Scripts.Match.Entities;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Enemies.Interface
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Focuses shared methods and properties between some of the obstacles.
    /// </summary>
    public abstract class Block : MonoBehaviour, IObstacle
    {
        /// <summary>
        /// Rigidbody component of the object.
        /// </summary>
        protected Rigidbody _rigidBody;
        /// <summary>
        /// Script that constantly applies constant force.
        /// </summary>
        protected SimpleMover _simpleMover;
        /// <summary>
        /// Renderer of the object, stores the material.
        /// </summary>
        protected Renderer _renderer;
        /// <summary>
        /// Is the current obstacle used?
        /// </summary>
        protected bool _isActive;

        /// <summary>
        /// Index assigned to poolable obstacle upon activation. Used by the owner pool to recognize the object in collection.
        /// </summary>
        public uint ActivationIndex { get; private set; }
        /// <summary>
        /// Reference to callback that can be used by the obstacle to call its pool when the obstacle has to be
        /// deactivated.
        /// </summary>
        public UnityAction<uint> DeactivationCallback { get; set; }
        /// <summary>
        /// Retrieve components from gameobject.
        /// </summary>
        protected void Start()
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

            ResetObstacle();
        }
        /// <summary>
        /// Resets the obstacle.
        /// </summary>
        protected void ResetObstacle()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }


        /// <summary>
        /// Resets the color of the material, removing the highlight.
        /// </summary>
        public void DeselectObject()
        {
            _renderer.material.color = Color.white;
        }

        public abstract bool ChkGloveType(ProjectileTypeEnum projectileType);

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
            _isActive = true;
            ActivationIndex = activationIndex;
        }
        /// <summary>
        /// Disables the gameobject of the obstacle.
        /// </summary>
        public void Deactivate()
        {
            ResetObstacle();
            //There's a a chance that the obstacle will collide with two disabling entities
            //at once. If one of them has deactivated the obstacle already, do not attempt to
            //inform the obstacle pool again.
            if (_isActive)
            {
                this.DeactivationCallback(ActivationIndex);
                _isActive = false;
            }
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
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Normalized force vector.</param>
        /// <param name="value">Value by which the vector shall be multiplied.</param>

        public abstract void ApplyForce(Vector3 direction, float value);
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Direction of force, including its value.</param>
        public abstract void ApplyForce(Vector3 direction);
        /// <summary>
        /// Applies highlight to the object.
        /// </summary>
        /// <param name="team">What team color should the highlight have?</param>
        public abstract void SelectObject(TeamEnum team);
    }
}
