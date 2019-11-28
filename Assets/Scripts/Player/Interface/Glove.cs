using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Data;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Player.Helpers;
using Assets.Scripts.Player.Weapons.Aesthetics;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.Player.Interface
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Base operations and fields for glove - tool used by the player to interact with the world.
    /// </summary>
    public abstract class Glove : MonoBehaviour, IWeapon
    {
        /// <summary>
        /// Renders line between the glove and held object.
        /// </summary>
        [SerializeField]
        protected GloveLineRenderer _lineRenderer;
        /// <summary>
        /// Defines the value of force that this glove can apply to hooked obstacles.
        /// </summary>
        public float Strength { get; protected set; }

        /// <summary>
        /// Defines what part of velocity is used to generate resistance force.
        /// </summary>
        [SerializeField] protected float _dampeningFactor;    //set to 0.25, will work this way with changes in calculator
        /// <summary>
        /// Stores the most recent raycast hit.
        /// </summary>
        protected RaycastHit _raycastHit;
        /// <summary>
        /// Stores data about the currently hooked obstacle by this glove. If none is hooked - null.
        /// </summary>
        public HookingData HookingData { get; protected set; }
        /// <summary>
        /// Type of the glove.
        /// </summary>
        public ProjectileTypeEnum ProjectileType { get; }

        /// <summary>
        /// Prompts drawing line between connected object and the glove.
        /// </summary>
        protected void DrawHookingLine()
        {
            if (HookingData.HookedObstacle != null)
            {
                float halfDistance = HookingData.HookingDistMagnitude * 0.5f;
                Vector3 secondControlPoint = VectorManipulator.CreateVectorFromRotation(GameConstants.PlayerRotationRefVector, transform.rotation, halfDistance);
                secondControlPoint += transform.position;
                _lineRenderer.DrawBezierSquared(transform.position, secondControlPoint, HookingData.HookedObstacle.GetPosition());
            }
        }
        /// <summary>
        /// Saves data of recently hooked object.
        /// </summary>
        /// <param name="hookedObstacle">Hooked object.</param>
        protected void SaveHookedObjectData(IObstacle hookedObstacle)
        {
            Vector3 hookingRefDistance = hookedObstacle.GetPosition() - transform.position;
            HookingData = new HookingData(hookedObstacle, hookingRefDistance);
        }
        /// <summary>
        /// Prompts erasing of previously drawn line between connected object and glove.
        /// </summary>
        protected void EraseLine()
        {
            _lineRenderer.EraseLine();
        }

        /// <summary>
        /// Seeks for any hookable obstacles/enemies and if found - hooks them to the glove, if possible.
        /// </summary>
        /// <param name="direction">Direction of the seeking raycast.</param>
        /// <param name="team">Team of the seeking caster.</param>
        /// <returns></returns>
        protected abstract HookingResultEnum TryHookingObject(Vector3 direction, TeamEnum team);
        
        /// <summary>
        /// Applies force to the held obstacle, if necessary.
        /// </summary>
        protected void ApplyForceToObstacle()
        {
            Vector3 currentDistance = HookingData.HookedObstacle.GetPosition() - transform.position;
            var forceCalculator = ForceCalculator.GetInstance();
            Vector3 glovePositionalForce = forceCalculator.GlovePositionalForce(currentDistance, HookingData.HookingRefDistance, Strength);
            Vector3 gloveRotationalForce = forceCalculator.GloveRotationalForce(HookingData.HookingRefDistance, GameConstants.PlayerRotationRefVector, transform.rotation, Strength);

            Vector3 dampeningForce = forceCalculator.DampeningForce(HookingData.HookedObstacle.GetVelocity(), _dampeningFactor, Strength);

            Vector3 totalForce = glovePositionalForce + gloveRotationalForce;
            totalForce += dampeningForce;
            totalForce *= Time.deltaTime;
            HookingData.HookedObstacle.ApplyForce(totalForce);
        }
        /// <summary>
        /// Casts a ray. If any objects are on the way - tries to hook the ray to one of these, starting
        /// from the closest one.
        /// </summary>
        /// <param name="direction">Direction in global coords of the raycast.</param>
        /// <param name="team">Team of the using player.</param>
        public HookingResultEnum UseWeapon(Vector3 direction, TeamEnum team)
        {
            if (HookingData == null)
            {
                return TryHookingObject(direction, team);
            }
            else
            {
                ApplyForceToObstacle();
                DrawHookingLine();

                return HookingResultEnum.ObjectHooked;
            }
        }
        /// <summary>
        /// Called when the player stops holding the object.
        /// </summary>
        public void StopUsingWeapon()
        {
            if (HookingData != null)
            {
                HookingData.HookedObstacle.DeselectObject();
                EraseLine();
                HookingData = null;
            }
        }
        /// <summary>
        /// Sets new glove strength.
        /// </summary>
        /// <param name="gloveStrength">New glove strength.</param>
        public void SetStrength(float gloveStrength)
        {
            Strength = gloveStrength;
        }
        /// <summary>
        /// Deactivates the gameobject of the weapon.
        /// </summary>
        public void DeactivateWeapon()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Activates the gameobject of the weapon.
        /// </summary>
        public void ActivateWeapon()
        {
            gameObject.SetActive(true);
        }
    }
}
