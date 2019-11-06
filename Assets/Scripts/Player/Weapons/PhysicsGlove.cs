using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Player.Weapons.Aesthetics;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Player.Helpers;
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
        public float Strength { get{ return _strength; } }
        /// <summary>
        /// Stores the value of force that this glove can apply to hooked obstacles and allows for its modification
        /// from the editor level.
        /// </summary>
        [SerializeField]
        private float _strength;
        /// <summary>
        /// Defines what part of velocity is used to generate resistance force.
        /// </summary>
        [SerializeField] private float _dampeningFactor;    //set to 0.25, will work this way with changes in calculator
        /// <summary>
        /// Stores the most recent raycast hit.
        /// </summary>
        private RaycastHit _raycastHit;
        /// <summary>
        /// Offset of the object towards the glove upon hooking it up.
        /// </summary>
        private Vector3 _hookingReferenceDistance;

        /// <summary>
        /// The currently hooked obstacle by this glove. If none is hooked - null.
        /// </summary>
        public IObstacle HookedObstacle { get; private set; }
        public ProjectileTypeEnum ProjectileType { get; } = ProjectileTypeEnum.Physical;

        /// <summary>
        /// Prompts drawing line between connected object and the glove.
        /// </summary>
        private void DrawHookingLine()
        {
            if (HookedObstacle != null)
            {
                _lineRenderer.DrawLine(transform.position, HookedObstacle.GetPosition());
            }
        }

        private void SaveReferenceVector()
        {
            _hookingReferenceDistance = HookedObstacle.GetPosition() - transform.position;
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
                HookedObstacle = _raycastHit.transform.gameObject.GetComponent<IObstacle>();

                if (HookedObstacle == null)
                {
                    return HookingResultEnum.NoObjectFound;
                }

                if (HookedObstacle.ChkGloveType(ProjectileType) == false)
                {
                    return HookingResultEnum.WrongType;
                }

                HookedObstacle.SelectObject(team);
                SaveReferenceVector();
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
            Vector3 currentDistance = HookedObstacle.GetPosition() - transform.position;
            var forceCalculator = ForceCalculator.GetInstance();
            Vector3 glovePositionalForce = forceCalculator.GlovePositionalForce(currentDistance, _hookingReferenceDistance, Strength);
            Vector3 gloveRotationalForce = forceCalculator.GloveRotationalForce(currentDistance, Vector3.forward, transform.rotation, Strength);
            

            Vector3 dampeningForce = forceCalculator.DampeningForce(HookedObstacle.GetVelocity(), _dampeningFactor);

            Vector3 totalForce = glovePositionalForce + gloveRotationalForce;
            totalForce += dampeningForce;
            HookedObstacle.ApplyForce(totalForce);
            //Debug.Log("RefVector: " + _hookingReferenceDistance);
            //Debug.Log("PositionalForce " + glovePositionalForce);
            //Debug.Log("Rotational force " + gloveRotationalForce);
        }
        /// <summary>
        /// Casts a ray. If any objects are on the way - tries to hook the ray to one of these, starting
        /// from the closest one.
        /// </summary>
        /// <param name="direction">Direction in global coords of the raycast.</param>
        /// <param name="team">Team of the using player.</param>
        public HookingResultEnum UseWeapon(Vector3 direction, TeamEnum team)
        {
            if (HookedObstacle == null)
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
            if (HookedObstacle != null)
            {
                HookedObstacle.DeselectObject();
                EraseLine();
                HookedObstacle = null;
            }
        }
    }
}
