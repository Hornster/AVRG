using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Player.CameraSettings
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Contains actions that can be performed on regular camera.
    /// </summary>
    public class RegularCameraController : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _cameraSpeed;

        private float rotationX;
        private float rotationY;
        void Start()
        {
            InputController.RegisterOnMouseAxesChange(this.RotatePlayer);
        }
        /// <summary>
        /// Rotates player accordingly to provided input vector, camera speed and last frame time.
        /// </summary>
        /// <param name="newAxesValue"></param>
        void RotatePlayer(Vector2 newAxesValue)
        {
            //var deltaTime = Time.deltaTime;
            rotationX += _cameraSpeed.x * newAxesValue.x;// * deltaTime;
            rotationY -= _cameraSpeed.y * newAxesValue.y;// * deltaTime;

            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0.0f);
        }
    }
}
