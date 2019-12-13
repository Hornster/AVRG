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
    /// Physical glove which is used by the player to interact with the world.
    /// </summary>
    public class PhysicsGlove : Glove
    {
        /// <summary>
        /// Type of the glove.
        /// </summary>
        public new ProjectileTypeEnum ProjectileType { get; } = ProjectileTypeEnum.Physical;

        /// <summary>
        /// Seeks for any hookable obstacles/enemies and if found - hooks them to the glove, if possible.
        /// </summary>
        /// <param name="direction">Direction of the seeking raycast.</param>
        /// <param name="team">Team of the seeking caster.</param>
        /// <returns></returns>
        protected override HookingResultEnum TryHookingObject(Vector3 direction, TeamEnum team)
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

        public override ProjectileTypeEnum GetProjectileType()
        {
            return ProjectileType;
        }
    }
}
