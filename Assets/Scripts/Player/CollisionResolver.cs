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
        /// Defines with which layers will the collisions be checked.
        /// </summary>
        [SerializeField] private LayerMask _collisionLayerMask;
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
        /// <summary>
        /// FP values below this value will be treated as zero.
        /// </summary>
        [SerializeField] private float _zeroTreshold = 0.000001f;

        private List<Ray> _colliderRays = new List<Ray>();
        private int _raysPerWall;
        private const int WallsCount = 6;
        private float _amountOfGapsBetweenRays;

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
            
            _amountOfGapsBetweenRays = _raysPerEdge - 1;
            if (_raysPerEdge <= 1)
            {
                throw new Exception("At least 2 rays are required per edge.");
            }

            _collisionLayerMask.value = ~_collisionLayerMask.value;
        }

        /// <summary>
        /// Adds rotated accordingly to the collider rotation normals to the list.
        /// </summary>
        /// <param name="rayDataList">List containing ray data for each of the collider's walls.</param>
        /// <param name="colliderRotation">Rotation of the collider.</param>
        /// <returns></returns>
        private void AddNormals(List<RayData> rayDataList, Quaternion colliderRotation)
        {
            rayDataList[(int)BoxColliderWallsEnum.Right].normal = VectorManipulator.RotateVector(colliderRotation, Vector3.right);
            rayDataList[(int)BoxColliderWallsEnum.Left].normal = VectorManipulator.RotateVector(colliderRotation, Vector3.left);
            rayDataList[(int)BoxColliderWallsEnum.Up].normal = VectorManipulator.RotateVector(colliderRotation, Vector3.up);
            rayDataList[(int)BoxColliderWallsEnum.Down].normal = VectorManipulator.RotateVector(colliderRotation, Vector3.down);
            rayDataList[(int)BoxColliderWallsEnum.Forward].normal = VectorManipulator.RotateVector(colliderRotation, Vector3.forward);
            rayDataList[(int)BoxColliderWallsEnum.Back].normal = VectorManipulator.RotateVector(colliderRotation, Vector3.back);
        }
        /// <summary>
        /// Adds width offsets to ray data.
        /// </summary>
        /// <param name="rayDataList">Data of rays which will be modified.</param>
        private void CreateWidths(List<RayData> rayDataList)
        {
            float widthX = _boxCollider.size.x * 0.5f;
            float widthZ = _boxCollider.size.z * 0.5f;
            //For normal Vector3.right and Vector3.left, z is the width.
            rayDataList[(int)BoxColliderWallsEnum.Right].WidthOffset = new Vector3(0, 0, widthZ);
            rayDataList[(int)BoxColliderWallsEnum.Left].WidthOffset = new Vector3(0, 0, widthZ);
            //For normal Vector3.up and Vector3.down, x is the width.
            rayDataList[(int)BoxColliderWallsEnum.Up].WidthOffset = new Vector3(widthX, 0, 0);
            rayDataList[(int)BoxColliderWallsEnum.Down].WidthOffset = new Vector3(widthX, 0, 0);
            //For normal Vector3.right and Vector3.left, x is the width.
            rayDataList[(int)BoxColliderWallsEnum.Forward].WidthOffset = new Vector3(widthX, 0, 0);
            rayDataList[(int)BoxColliderWallsEnum.Back].WidthOffset = new Vector3(widthX, 0, 0);
        }
        /// <summary>
        /// Adds height offsets to ray data.
        /// </summary>
        /// <param name="rayDataList">Data of rays which will be modified.</param>
        private void CreateHeights(List<RayData> rayDataList)
        {
            float widthY = _boxCollider.size.y * 0.5f;
            float widthZ = _boxCollider.size.z * 0.5f;
            //For normal Vector3.right and Vector3.left, z is the width.
            rayDataList[(int)BoxColliderWallsEnum.Right].HeightOffset = new Vector3(0, widthY, 0);
            rayDataList[(int)BoxColliderWallsEnum.Left].HeightOffset = new Vector3(0, widthY, 0);
            //For normal Vector3.up and Vector3.down, x is the width.
            rayDataList[(int)BoxColliderWallsEnum.Up].HeightOffset = new Vector3(0, 0, widthZ);
            rayDataList[(int)BoxColliderWallsEnum.Down].HeightOffset = new Vector3(0, 0, widthZ);
            //For normal Vector3.right and Vector3.left, x is the width.
            rayDataList[(int)BoxColliderWallsEnum.Forward].HeightOffset = new Vector3(0, widthY, 0);
            rayDataList[(int)BoxColliderWallsEnum.Back].HeightOffset = new Vector3(0, widthY, 0);
        }
        /// <summary>
        /// Rotates offsets of the rays data using provided quaternion.
        /// </summary>
        /// <param name="rayDataList">Offsets to rotate.</param>
        /// <param name="colliderRotation">Rotation info.</param>
        private void RotateOffsets(List<RayData> rayDataList, Quaternion colliderRotation)
        {
            foreach (var singleRayData in rayDataList)
            {
                singleRayData.WidthOffset = VectorManipulator.RotateVector(colliderRotation, singleRayData.WidthOffset);
                singleRayData.HeightOffset =
                    VectorManipulator.RotateVector(colliderRotation, singleRayData.HeightOffset);
            }
        }
        /// <summary>
        /// Checks which of the normals point in the direction of the velocity vector. If the angle between given normal
        /// and the velocity vector is at least 90° - the normal is omitted and removed from normals list.
        /// </summary>
        /// <param name="rayDataList">List of normals.</param>
        /// <param name="velocityVector">Velocity vector.</param>
        private void ChkMovement(List<RayData> rayDataList, Vector3 velocityVector)
        {
            Vector3 normalizedVelocity = velocityVector.normalized;
            var rayIndexesToRemove = new List<int>();
            for (int i = 0; i < rayDataList.Count; i++)
            {
                rayDataList[i].dpAgainstVelocity = VectorManipulator.CalcDotProduct(rayDataList[i].normal, normalizedVelocity);
                if (rayDataList[i].dpAgainstVelocity <= _zeroTreshold)
                {
                    rayIndexesToRemove.Add(i);
                }

            }

            for (int i = rayIndexesToRemove.Count - 1; i >= 0; i--)
            {
                rayDataList.RemoveAt(rayIndexesToRemove[i]);
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
                ray.rayLength = velocityLength * ray.dpAgainstVelocity;
            }
        }
        /// <summary>
        /// Calculates the position of the rays beginnings in one corner of each wall.
        /// </summary>
        /// <param name="raysDataList">Rays data.</param>
        private void CalculateRayPositions(List<RayData> raysDataList, Vector3 colliderCenterPosition)
        {
            Vector3 boxColliderSize = _boxCollider.size * 0.5f; //We need only half of the size since the anchor is in the middle of the collider.
            foreach (var singleRayData in raysDataList)
            {
                singleRayData.rayPosition = colliderCenterPosition + VectorManipulator.MultiplyVectorParts(singleRayData.normal, boxColliderSize);
            }
        }
        /// <summary>
        /// Sets up _colliderRays to resemble the provided reference data.
        /// </summary>
        /// <param name="rayData"></param>
        private void SetupRaysForWall(RayData rayData)
        {
            float lengthFactor = 2.0f / _amountOfGapsBetweenRays;   //We need full length of the edge. We got half from the data.
                                                        //Divide by amount of edges once (would have twice otherwise).
            float widthOffsetLength = rayData.WidthOffset.magnitude * lengthFactor;
            float heightOffsetLength = rayData.HeightOffset.magnitude * lengthFactor;
            Vector3 widthOffsetStep = rayData.WidthOffset.normalized * widthOffsetLength;
            Vector3 heightOffsetStep = rayData.HeightOffset.normalized * heightOffsetLength;
            int rayIndex = 0;
            for (int i = 0; i < _raysPerEdge; i++)
            {
                Vector3 currentRayPosition = rayData.rayPosition + i * widthOffsetStep;
                for (int j = 0; j < _raysPerEdge; j++)
                {
                    Ray ray = _colliderRays[rayIndex];

                    ray.origin = currentRayPosition;
                    ray.direction = rayData.normal;
                    _colliderRays[rayIndex] = ray;
                    
                    currentRayPosition += heightOffsetStep;
                    rayIndex++;
                }
            }

        }
        /// <summary>
        /// Casts rays for given wall. Returns shortest partial vector for this wall. If no collision found -
        /// returns partial vector as long as the cast ray.
        /// </summary>
        /// <param name="currentVelocity"></param>
        /// <returns></returns>
        private Vector3 CastRaysForWall(RayData rayData)
        {
            float closestDistance = rayData.rayLength;
            foreach (var ray in _colliderRays)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, rayData.rayLength, _collisionLayerMask))
                {
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        Vector3 distance = hit.point - rayData.rayPosition;
                        Debug.Log("Distance by points: " + distance.magnitude);
                        Debug.Log("Distance by distance: " + hit.distance);
                    }
                }

                Debug.DrawRay(ray.origin, ray.direction * rayData.rayLength, Color.magenta);
            }

            return rayData.normal * closestDistance;
        }
        /// <summary>
        /// Casts rays for given collider walls. Returns max velocity allowed for the object during the frame.
        /// If there was no collision detected - returns starting value of currentVelocity.
        /// </summary>
        /// <param name="rayDataList">Reference data for setting up the cast rays.</param>
        /// <returns></returns>
        private Vector3 CastRays(List<RayData> rayDataList)
        {
            Vector3 minimumMovementVector = Vector3.zero;
            foreach (var singleRayData in rayDataList)
            {
                SetupRaysForWall(singleRayData);
                minimumMovementVector += CastRaysForWall(singleRayData);
            }

            return minimumMovementVector;
        }
        /// <summary>
        /// Repositions all rays to the corners of their collider wall.
        /// </summary>
        /// <param name="rayDataList">Rays data list.</param>
        private void MoveNormalsToCorners(List<RayData> rayDataList)
        {
            foreach (var singleRayData in rayDataList)
            {
                singleRayData.rayPosition = singleRayData.rayPosition - singleRayData.HeightOffset;
                singleRayData.rayPosition = singleRayData.rayPosition - singleRayData.WidthOffset;
            }
        }
        /// <summary>
        /// Creates list containing placeholders for ray data for each collider wall.
        /// </summary>
        /// <returns></returns>
        private List<RayData> CreateRayDataList()
        {
            var list = new List<RayData>();
            for (int i = 0; i < WallsCount; i++)
            {
                list.Add(new RayData());
            }

            return list;
        }

        /// <summary>
        /// Checks for any collisions and returns movement correction vector. It should be subtracted from the movement vector.
        /// </summary>
        /// <param name="colliderCenterPosition">Transform of the object that will this class be checking.</param>
        /// <param name="currentVelocity">Velocity of the checked object.</param>
        /// <param name="currentRotation">Current rotation of the collider.</param>
        /// <returns></returns>
        public Vector3 SolveCollisions(Vector3 colliderCenterPosition, Vector3 currentVelocity, Quaternion currentRotation)
        {
            var rayData = CreateRayDataList();
            AddNormals(rayData, currentRotation);
            CreateWidths(rayData);
            CreateHeights(rayData);
            ChkMovement(rayData, currentVelocity);
            RotateOffsets(rayData, currentRotation);
            CalculateRayLengths(rayData, currentVelocity);
            CalculateRayPositions(rayData, colliderCenterPosition);
            MoveNormalsToCorners(rayData);
            Vector3 correctionVector = CastRays(rayData);


            //Debug.Log("Velocity vector: " + currentVelocity);
            //Debug.Log("Correction vector: " + correctionVector);
            ShowDebugRays(rayData);
            if (correctionVector.magnitude < 0.001f)
            {
                return Vector3.zero;
            }
            return correctionVector;
        }

        private void ShowDebugRays(List<RayData> raysDataList)
        {
            foreach (var rayData in raysDataList)
            {
                Debug.DrawRay(rayData.rayPosition, rayData.normal, Color.red);
            }
        }
        /// <summary>
        /// Stores data about given wall rays.
        /// </summary>
        class RayData
        {
            public Vector3 normal;
            public float rayLength;
            public float dpAgainstVelocity;
            public Vector3 rayPosition;

            /// <summary>
            /// Single step offset by which the ray position will be moved along width of the collider wall.
            /// </summary>
            public Vector3 WidthOffset;
            /// <summary>
            /// Single step offset by which the ray position will be moved along height of the collider wall.
            /// </summary>
            public Vector3 HeightOffset;
        }
    }
}
