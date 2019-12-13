using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Maps
{
    public class ObstacleActivator : MonoBehaviour
    {
        public GameObject GameObject;
        void Update()
        {
            GameObject.SetActive(true);
        }
    }
}
