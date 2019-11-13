using Assets.Scripts.Data;
using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Player.Weapons.Aesthetics;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Player.Helpers;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;
using HookingResultEnum = Assets.Scripts.Shared.Enums.HookingResultEnum;

namespace Assets.Scripts.Player.Weapons
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Glove which is used by the player to interact with the world.
    /// </summary>
    public class PhysicsGlove : MonoBehaviour, IWeapon
    {
        /// <summary>
        /// Renders line between the glove and held object.
        /// </summary>
        [SerializeField]
        private GloveLineRenderer _lineRenderer;
        /// <summary>
        /// Defines the value of force that this glove can apply to hooked obstacles.
        /// </summary>
        public float Strength { get; private set; }
        
        /// <summary>
        /// Defines what part of velocity is used to generate resistance force.
        /// </summary>
        [SerializeField] private float _dampeningFactor;    //set to 0.25, will work this way with changes in calculator
        /// <summary>
        /// Stores the most recent raycast hit.
        /// </summary>
        private RaycastHit _raycastHit;
        /// <summary>
        /// Stores data about the currently hooked obstacle by this glove. If none is hooked - null.
        /// </summary>
        public HookingData HookingData { get; private set; }
        public ProjectileTypeEnum ProjectileType { get; } = ProjectileTypeEnum.Physical;

        /// <summary>
        /// Prompts drawing line between connected object and the glove.
        /// </summary>
        private void DrawHookingLine()
        {
            if (HookingData.HookedObstacle != null)
            {
                float halfDistance = HookingData.HookingDistMagnitude * 0.5f;
                Vector3 secondControlPoint = VectorManipulator.CreateVectorFromRotation(PlayerConstants.PlayerRotationRefVector, transform.rotation, halfDistance);
                secondControlPoint += transform.position;
                _lineRenderer.DrawBezierSquared(transform.position, secondControlPoint, HookingData.HookedObstacle.GetPosition());
            }
        }
        /// <summary>
        /// Saves data of recently hooked object.
        /// </summary>
        /// <param name="hookedObstacle">Hooked object.</param>
        private void SaveHookedObjectData(IObstacle hookedObstacle)
        {
            Vector3 hookingRefDistance = hookedObstacle.GetPosition() - transform.position;
            HookingData = new HookingData(hookedObstacle, hookingRefDistance);
        }
        /// <summary>
        /// Prompts erasing of previously drawn line between connected object and glove.
        /// </summary>
        private void EraseLine()
        {
            _lineRenderer.EraseLine();
        }
        /// <summary>
        /// Seeks for any hookable obstacles/enemies and if found - hooks them to the glove, if possible.
        /// </summary>
        /// <param name="direction">Direction of the seeking raycast.</param>
        /// <param name="team">Team of the seeking caster.</param>
        /// <returns></returns>
        private HookingResultEnum TryHookingObject(Vector3 direction, TeamEnum team)
        {
            if (Physics.Raycast(gameObject.transform.position, direction, out _raycastHit))
            {
                var hookedObstacle = _raycastHit.transform.gameObject.GetComponent<IObstacle>();

                if (hookedObstacle == null)
                {
                    return HookingResultEnum.NoObjectFound;
                }

                if (hookedObstacle.ChkGloveType(ProjectileType) == false)
                {
                    return HookingResultEnum.WrongType;
                }

                hookedObstacle.SelectObject(team);
                SaveHookedObjectData(hookedObstacle);
                DrawHookingLine();

                return HookingResultEnum.ObjectHooked;
            }

            return HookingResultEnum.NoObjectFound;
        }
        /// <summary>
        /// Applies force to the held obstacle, if necessary.
        /// </summary>
        private void ApplyForceToObstacle()
        {
            Vector3 currentDistance = HookingData.HookedObstacle.GetPosition() - transform.position;
            var forceCalculator = ForceCalculator.GetInstance();
            Vector3 glovePositionalForce = forceCalculator.GlovePositionalForce(currentDistance, HookingData.HookingRefDistance, Strength);
            Vector3 gloveRotationalForce = forceCalculator.GloveRotationalForce(HookingData.HookingRefDistance, PlayerConstants.PlayerRotationRefVector, transform.rotation, Strength);
            
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
    }
}
