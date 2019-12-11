using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Assets.Scripts.Player.Controller
{
    public class StandaloneInputVROverride : BaseInput
    {
        /// <summary>
        /// Camera used for event retrieving. Required by Unity methods of event tracking.
        /// </summary>
        [SerializeField]
        private Camera _eventCamera  = null;
        /// <summary>
        /// Defines what button on the controller will be interpreted as LMB.
        /// </summary>
        [SerializeField] private OVRInput.Button _leftMouseButtonOverride = OVRInput.Button.PrimaryIndexTrigger;
        /// <summary>
        /// Defines which controller will be used for overriding of standard controls.
        /// </summary>
        [SerializeField] private OVRInput.Controller _usedController = OVRInput.Controller.All;
        protected override void Awake()
        {
            GetComponent<BaseInputModule>().inputOverride = this;
        }

        public override bool GetMouseButton(int button)
        {
            return OVRInput.Get(_leftMouseButtonOverride, _usedController);
        }

        public override bool GetMouseButtonDown(int button)
        {
            return OVRInput.GetDown(_leftMouseButtonOverride, _usedController);
        }

        public override bool GetMouseButtonUp(int button)
        {
            return OVRInput.GetUp(_leftMouseButtonOverride, _usedController);
        }

        public override Vector2 mousePosition => new Vector2(_eventCamera.pixelWidth * 0.5f, _eventCamera.pixelHeight * 0.5f);
    }
}
