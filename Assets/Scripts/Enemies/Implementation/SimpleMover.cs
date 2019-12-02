
using Assets.Scripts.Match;
using UnityEngine;

namespace Assets.Scripts.Enemies.Implementation
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Pushes an object onwards.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleMover : MonoBehaviour, IPausable
    {
        private Rigidbody _rigidbody;
        /// <summary>
        /// Constant force that will be applied to object that has this script attached.
        /// </summary>
        public Vector3 ConstantForce { get; set; }
        /// <summary>
        /// Velocity of the rigidbody upon triggering pause.
        /// </summary>
        private Vector3 _velocityUponPausing;
        /// <summary>
        /// Angular velocity of the rigidbody upon triggering pause.
        /// </summary>
        private Vector3 _angularVelocityUponPausing;
        private bool _isPaused = false;
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            MatchController.RegisterOnPause(Pause);
            MatchController.RegisterOnResume(Resume);
        }

        void Update()
        {
            if (_isPaused)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                return;
            }

            _rigidbody.AddForce(ConstantForce);
        }
        /// <summary>
        /// Resets the paused state of the object to unpaused.
        /// </summary>
        public void ResetPause()
        {
            _isPaused = false;
        }
        /// <summary>
        /// Causes the object to halt executing its behavior.
        /// </summary>
        public void Pause()
        {
            _isPaused = true;
            _velocityUponPausing = _rigidbody.velocity;
            _angularVelocityUponPausing = _rigidbody.angularVelocity;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// Causes the object to resume executing its behavior.
        /// </summary>
        public void Resume()
        {
            _isPaused = false;
            _rigidbody.velocity = _velocityUponPausing;
            _rigidbody.angularVelocity = _angularVelocityUponPausing;
        }
    }
}
