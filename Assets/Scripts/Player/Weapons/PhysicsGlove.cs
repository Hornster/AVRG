using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Player.Interface;
using Assets.Scripts.Player.Weapons.Aesthetics;
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
        /// Renders line between the glove and held object.
        /// </summary>
        [SerializeField]
        private GloveLineRenderer _lineRenderer;
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
        /// Prompts drawing line between connected object and the glove.
        /// </summary>
        private void DrawHookingLine()
        {
            if (HookedObstacle != null)
            {
                _lineRenderer.DrawLine(transform.position, HookedObstacle.GetPosition());
            }
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
                DrawHookingLine();

                return HookingResultEnum.ObjectHooked;
            }

            return HookingResultEnum.NoObjectFound;
        }
        /// <summary>
        /// Casts a ray. If any objects are on the way - tries to hook the ray to one of these, starting
        /// from the closest one.
        /// </summary>
        /// <param name="direction">Direction in global coords of the raycast.</param>
        /// <param name="team">Team of the using player.</param>
        public HookingResultEnum UseWeapon(Vector3 direction, TeamEnum team)
        {
            Debug.DrawRay(gameObject.transform.position, direction * 100, Color.red);
            Debug.Log("Raycast direction: " + direction);
            if (HookedObstacle == null)
            {
                return TryHookingObject(direction, team);
            }
            else
            {
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
