using Assets.Scripts.Enemies.Interface;
using Assets.Scripts.Match.Entities;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using System;
using Assets.Scripts.Factories;
using UnityEngine;

namespace Assets.Scripts.Enemies.Implementation
{
    /// <summary>
    ///Author: Karol Kozuch
    /// 
    /// Controller for the energy blocks (obstacles).
    /// </summary>
    public class EnergyBlockController : Block, IDamageDealer
    {
        /// <summary>
        /// All layers that trigger destruction of this block.
        /// </summary>
        [SerializeField]
        private LayerMask _destroyingLayers;
        /// <summary>
        /// Defines who can receive damage upon collision with this object.
        /// </summary>
        [SerializeField] private LayerMask _dealsDamageToLayers;
        /// <summary>
        /// Highlight color of the selected obstacle.
        /// </summary>
        [SerializeField]
        private Color _selectedColor;

        /// <summary>
        /// Is the current obstacle used?
        /// </summary>
        private bool _isActive = false;
        /// <summary>
        /// Material of the object.
        /// </summary>
        private Material _material;

        /// <summary>
        /// Type of glove the object will react to.
        /// </summary>
        private const ProjectileTypeEnum ObjectType = ProjectileTypeEnum.Physical;


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
                //this.DeactivationCallback(ActivationIndex);
            }
            else if ((layerBitValue & _dealsDamageToLayers.value) != 0)
            {
                var damageReceiver = collision.gameObject.GetComponent<IDamageReceiver>();

                if (damageReceiver == null)
                {
                    return;
                }

                DealDamage(damageReceiver);
                this.Deactivate();
            }
            else
            {
                var otherBlock = collision.gameObject.GetComponent<EnergyBlockController>();
                if (otherBlock != null)
                {
                    MergeBlocks(otherBlock);
                }
            }
        }
        /// <summary>
        /// Merges two energy blocks together. Smaller block will be deactivated later.
        /// Bigger block will get bigger on the smallest dimension.
        /// </summary>
        private void MergeBlocks(EnergyBlockController otherBlock)
        {
            var calculator = Calculations.GetInstance();
            float thisCuboidVolume = calculator.CalcCuboidVolume(transform.localScale);
            float otherCuboidVolume = calculator.CalcCuboidVolume(otherBlock.transform.localScale);

            var biggerCuboid = thisCuboidVolume >= otherCuboidVolume ? this : otherBlock;
            var smallerCuboid = biggerCuboid == this ? otherBlock : this;
            Vector3 biggerCuboidDims = biggerCuboid.transform.localScale;
            //Find out which dimension is the biggest, which smallest and which is in between.
            float biggestDim = Math.Max(biggerCuboidDims.x, Math.Max(biggerCuboidDims.y, biggerCuboidDims.z));
            float smallestDim = Math.Min(biggerCuboidDims.x, Math.Min(biggerCuboidDims.y, biggerCuboidDims.z));
            float mediocreDim = biggerCuboidDims.z;
            if (biggerCuboidDims.x < biggestDim && biggerCuboidDims.x > smallestDim)
            {
                mediocreDim = biggerCuboidDims.x;
            }
            else if (biggerCuboidDims.y < biggestDim && biggerCuboidDims.y > smallestDim)
            {
                mediocreDim = biggerCuboidDims.y;
            }
            //Calculate value by which the smallest dimension of the bigger cuboid will be increased.
            float smallestDimAddition = calculator.DivideVolumeByArea(biggestDim, mediocreDim,
                Math.Min(thisCuboidVolume, otherCuboidVolume));
            //Find which dimension of the scale vector is the smallest. Could use equal operator, but these are floats after all...
            if (biggerCuboidDims.x < biggestDim && biggerCuboidDims.x < mediocreDim)
            {
                biggerCuboidDims.x += smallestDimAddition;
            }
            else if (biggerCuboidDims.y < biggestDim && biggerCuboidDims.y < mediocreDim)
            {
                biggerCuboidDims.y += smallestDimAddition;
            }
            else
            {
                biggerCuboidDims.z += smallestDimAddition;
            }

            float newMass = _rigidBody.mass + otherBlock._rigidBody.mass;
            Vector3 newConstantForce = _simpleMover.ConstantForce + otherBlock._simpleMover.ConstantForce;
            //Prepare mutation data.
            var mutationData = new ObstacleIniData()
            {
                Mass = newMass,
                ConstantForce = newConstantForce,
                Scale = biggerCuboidDims,
                Rotation = transform.rotation,
                ParentTransform = transform.parent,
                Position = transform.position
            };

            otherBlock.Deactivate();
            Mutate(mutationData);
        }
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Normalized force vector.</param>
        /// <param name="value">Value by which the vector shall be multiplied.</param>
        public override void ApplyForce(Vector3 direction, float value)
        {
            direction *= value;
            ApplyForce(direction);
        }
        /// <summary>
        /// Applies force to object.
        /// </summary>
        /// <param name="direction">Direction of force, including its value.</param>
        public override void ApplyForce(Vector3 direction)
        {
            _rigidBody.AddForce(direction);
        }
        /// <summary>
        /// Applies highlight to the object.
        /// </summary>
        /// <param name="team">What team color should the highlight have?</param>
        public override void SelectObject(TeamEnum team)
        {
            _renderer.material.color = ColorHelper.GetHighlightColor(team);
        }
        /// <summary>
        /// Checks if type of object is the same as type of the glove that tried to hook it.
        /// </summary>
        /// <param name="projectileType">Type of hooking glove.</param>
        /// <returns>True if types match, false otherwise.</returns>
        public override bool ChkGloveType(ProjectileTypeEnum projectileType)
        {
            return projectileType == ObjectType;
        }
    }
}
//TODO test merging abilities