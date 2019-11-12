
using UnityEngine;

namespace Assets.Scripts.Enemies.Implementation
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Pushes an object onwards.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleMover : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        /// <summary>
        /// Constant force that will be applied to object that has this script attached.
        /// </summary>
        public Vector3 ConstantForce { get; set; }

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            _rigidbody.AddForce(ConstantForce);
        }
    }
}
