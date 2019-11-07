using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.Weapons.Aesthetics
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Renders connecting line between the activated glove and held object.
    /// </summary>
    public class GloveLineRenderer : MonoBehaviour
    {
        [SerializeField]
        private LineRenderer _lineRenderer;
        [SerializeField]
        private Color _lineColor;
        [SerializeField]
        private float _width;
        /// <summary>
        /// Amount of points used to draw the line.
        /// </summary>
        [SerializeField]
        private int VerticesCount = 3;
        /// <summary>
        /// Defines the value (interpolation step) of the t value in Bezier's equations.
        /// </summary>
        private float step;

        private List<Vector3> bezierPoints;

        void Start()
        {
            if (VerticesCount < 2)
            {
                throw new Exception($"Bezier curve must have at least 2 vertices! Had {VerticesCount}!");
            }
            
            bezierPoints = new List<Vector3>(VerticesCount);
            for (int i = 0; i < VerticesCount; i++)
            {
                bezierPoints.Add(Vector3.zero);
            }

            step = 1.0f / VerticesCount;

            _lineRenderer.endWidth = _lineRenderer.startWidth = _width;
            _lineRenderer.endColor = _lineRenderer.startColor = _lineColor;
            _lineRenderer.positionCount = VerticesCount + 1;//Add one more vertex that will be always at the hooked object's position.
                                                            //Otherwise, upon extending quite much, the line will look detached from the object.

            EraseLine();
        }

        void Update()
        {

        }
        /// <summary>
        /// Sets positions of freshly calculated points in the line rendering component.
        /// </summary>
        private void ShowBezier(Vector3 hookedObjectPos)
        {
            _lineRenderer.enabled = true;
            for (int i = 0; i < bezierPoints.Count; i++)
            {
                _lineRenderer.SetPosition(i, bezierPoints[i]);
            }
            _lineRenderer.SetPosition(bezierPoints.Count, hookedObjectPos);
        }
        /// <summary>
        /// Draws simple straight line, from beginning to end.
        /// </summary>
        /// <param name="beginning"></param>
        /// <param name="end"></param>
        public void DrawStraightLine(Vector3 beginning, Vector3 end)
        {
            _lineRenderer.enabled = true;
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                _lineRenderer.SetPosition(i, beginning);

            }
            //There's one additional vertex that's always positioned at the hooked object's position.
            //Its purpose is to prevent a Bezier curve from detaching from the object upon strong extension.
            _lineRenderer.SetPosition(_lineRenderer.positionCount, end);
        }
        
        /// <summary>
        /// Draws a second-level Bezier curve.
        /// </summary>
        /// <param name="p0">Beginning position of the curved line.</param>
        /// <param name="p1">Middle control point of the line (defining the curve).</param>
        /// <param name="p2">End position of the curved line.</param>
        public void DrawBezierSquared(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 currentPoint;
            for (int i = 0; i < VerticesCount; i++)
            {
                float currentStep = i * step;
                currentPoint = p0 * (1 - currentStep) * (1 - currentStep);
                currentPoint += p1 * 2 * (1 - currentStep) * currentStep;
                currentPoint += p2 * currentStep * currentStep;
                bezierPoints[i] = currentPoint;
            }

            ShowBezier(p2);
        }
        /// <summary>
        /// Makes the line disappear.
        /// </summary>
        public void EraseLine()
        {
            _lineRenderer.enabled = false;
        }
    }
}
