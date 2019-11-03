using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Shared.Enums;
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
        /// The currently hooked obstacle by this glove. If none is hooked - null.
        /// </summary>
        public IObstacle HookedObstacle { get; private set; }
        /// <summary>
        /// Stores the most recent raycast hit.
        /// </summary>
        private RaycastHit _raycastHit;

        public ProjectileTypeEnum ProjectileType { get; } = ProjectileTypeEnum.Physical;

        /// <summary>
        /// Casts a ray. If any objects are on the way - tries to hook the ray to one of these, starting
        /// from the closest one.
        /// </summary>
        /// <param name="direction">Direction in global coords of the raycast.</param>
        /// <param name="team">Team of the using player.</param>
        public HookingResultEnum TryHookingObject(Vector3 direction, TeamEnum team)
        {
            Debug.DrawRay(gameObject.transform.position, direction*100, Color.red);
            if (Physics.Raycast(gameObject.transform.position, direction, out _raycastHit))
            {
                HookedObstacle = _raycastHit.transform.gameObject.GetComponent<IObstacle>();

                if (HookedObstacle.ChkGloveType(ProjectileType) == false)
                {
                    return HookingResultEnum.WrongType;
                }

                HookedObstacle.SelectObject(team);

                return HookingResultEnum.ObjectHooked;
            }

            return HookingResultEnum.NoObjectFound;
        }
        /// <summary>
        /// Called when the player stops holding the object.
        /// </summary>
        public void UnhookObject()
        {
            if (HookedObstacle != null)
            {
                HookedObstacle.DeselectObject();
                HookedObstacle = null;
            }
        }
    }
}
