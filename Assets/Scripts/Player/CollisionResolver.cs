using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Shared.Enums;
using Assets.Scripts.Shared.Helpers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Solves colliison between player and nearest terrain. Used 
    /// </summary>
    public class CollisionResolver : MonoBehaviour
    {
        /// <summary>
        /// How deeply into the body will the rays be positioned.
        /// </summary>
        [SerializeField]
        private float _skinWidth = 0.1f;
        /// <summary>
        /// Amount of rays that will be cast from each edge of the obstacle.
        /// </summary>
        [SerializeField] private int _raysPerEdge = 3;
        /// <summary>
        /// Collider of the object that will be checked against collisions.
        /// </summary>
        [SerializeField] private BoxCollider _boxCollider;

        private List<Ray> _colliderRays = new List<Ray>();
        private int _raysPerWall;
        private Vector3 _axesRaysOffsets;
        private const int WallsCount = 6;

        void Start()
        {
            _raysPerWall = _raysPerEdge * _raysPerEdge;
            for (int i = 0; i < _raysPerWall; i++)
            {
                _colliderRays.Add(new Ray());
            }

            if (_boxCollider == null)
            {
                throw new Exception("Error: no box collider found. Did you forget to provide the reference?");
            }

            Bounds bounds = _boxCollider.bounds;
            float amountOfGaps = _raysPerEdge - 1;
            if (amountOfGaps - 1 <= 1)
            {
                throw new Exception("At least 2 rays are required per edge.");
            }
            
            _axesRaysOffsets = bounds.size / amountOfGaps;
        }
        /// <summary>
        /// Adds single normal to normals.
        /// </summary>
        /// <param name="normals">List which will store new normals.</param>
        /// <param name="colliderBounds">Bounds of the collider which will have the normals assigned to.</param>
        /// <param name="colliderRotation">Rotation of the collider.</param>
        /// <param name="axisAlignedNormal">Axis aligned to given collider wall normal.</param>
        private void AddAxisAlignedNormal(List<RayData> normals, Bounds colliderBounds, Quaternion colliderRotation,
            Vector3 axisAlignedNormal)
        {
            axisAlignedNormal = colliderRotation * axisAlignedNormal;
            var rayData = new RayData()
            {
                normal = colliderBounds.center + axisAlignedNormal
            };
            normals.Add(rayData);
        }
        /// <summary>
        /// Returns a list of axis aligned normals, rotated and positioned around the collider.
        /// </summary>
        /// <returns></returns>
        private List<RayData> GetAxisAlignedNormals(Bounds colliderBounds, Quaternion colliderRotation)
        {
            var normals = new List<RayData>(WallsCount);

            AddAxisAlignedNormal(normals, colliderBounds, colliderRotation, new Vector3(colliderBounds.extents.x, 0, 0));
            AddAxisAlignedNormal(normals, colliderBounds, colliderRotation, new Vector3(-colliderBounds.extents.x, 0, 0));
            AddAxisAlignedNormal(normals, colliderBounds, colliderRotation, new Vector3(0, colliderBounds.extents.y, 0));
            AddAxisAlignedNormal(normals, colliderBounds, colliderRotation, new Vector3(0, -colliderBounds.extents.y, 0));
            AddAxisAlignedNormal(normals, colliderBounds, colliderRotation, new Vector3(0, 0, colliderBounds.extents.z));
            AddAxisAlignedNormal(normals, colliderBounds, colliderRotation, new Vector3(0, 0, -colliderBounds.extents.z));
            
            return normals;
        }
        /// <summary>
        /// Returns axis vector of length of 1 and positive direction. 
        /// </summary>
        /// <param name="whatAxis">What axis vector to return.</param>
        /// <returns></returns>
        private Vector3 GetAxis(AxisEnum whatAxis)
        {
            Vector3 axisToReturn = Vector3.zero;
            switch (whatAxis)
            {
                case AxisEnum.X:
                    axisToReturn = Vector3.right;
                    break;
                case AxisEnum.Y:
                    axisToReturn =  Vector3.up;
                    break;
                case AxisEnum.Z:
                    axisToReturn = Vector3.forward;
                    break;
            }

            return axisToReturn;
        }
        /// <summary>
        /// Checks which of the normals point in the direction of the velocity vector. If the angle between given normal
        /// and the velocity vector is at least 90° - the normal is omitted and removed from normals list.
        /// </summary>
        /// <param name="normals">List of normals.</param>
        /// <param name="velocityVector">Velocity vector.</param>
        private void ChkMovement(List<RayData> normals, Vector3 velocityVector)
        {
            Vector3 normalizedVelocity = velocityVector.normalized;
            for (int i = 0; i < normals.Count; i++)
            {
                normals[i].dpAgainstVelocity = VectorManipulator.CalcDotProduct(normals[i].normal, normalizedVelocity);
                if (normals[i].dpAgainstVelocity <= 0.0f)
                {
                    normals.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// Calculates length of rays for given walls.
        /// </summary>
        /// <param name="rays">List containing single ray data for each wall that will be tested against collision.</param>
        /// <param name="currentVelocity">CurrentVelocity of the object.</param>
        private void CalculateRayLengths(List<RayData> rays, Vector3 currentVelocity)
        {
            float velocityLength = currentVelocity.magnitude;
            foreach (var ray in rays)
            {
                ray.ray = ray.normal * (velocityLength * ray.dpAgainstVelocity);
            }
        }
        /// <summary>
        /// Casts rays on given axis using provided length.
        /// </summary>
        /// <param name="whatAxis">What axis will the rays be cast at.</param>
        /// <param name="rayLength">What should be the length of a single ray.</param>
        /// <returns></returns>
        private float CastRaysFromWall(AxisEnum whatAxis, float rayLength)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks for any collisions and returns movement correction vector. It should be subtracted from the movement vector.
        /// </summary>
        /// <param name="objectTransform">Transform of the object that will this class be checking.</param>
        /// <param name="currentVelocity">Velocity of the checked object.</param>
        /// <returns></returns>
        public Vector3 SolveCollisions(Transform objectTransform, Vector3 currentVelocity, Quaternion currentRotation)
        {
            var normals = GetAxisAlignedNormals(_boxCollider.bounds, currentRotation);
            ChkMovement(normals, currentVelocity);
            CalculateRayLengths(normals, currentVelocity);

            throw new NotImplementedException();
        }
        /// <summary>
        /// Stores data about given wall rays.
        /// </summary>
        class RayData
        {
            public Vector3 normal;
            public float dpAgainstVelocity;
            public Vector3 ray;
        }
    }
}
