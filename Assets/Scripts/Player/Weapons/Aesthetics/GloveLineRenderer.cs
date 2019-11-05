
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
        [SerializeField]
        private int _verticesCount;

        void Start()
        {
            _lineRenderer.endWidth = _lineRenderer.startWidth = _width;
            _lineRenderer.endColor = _lineRenderer.startColor = _lineColor;
            _lineRenderer.positionCount = _verticesCount;

            EraseLine();
        }

        void Update()
        {

        }

        public void DrawLine(Vector3 beginning, Vector3 end)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, beginning);
            _lineRenderer.SetPosition(1, end);
        }

        public void EraseLine()
        {
            _lineRenderer.enabled = false;
        }
    }
}
