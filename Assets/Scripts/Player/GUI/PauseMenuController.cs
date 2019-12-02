using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Player.GUI
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Controls the behavior of the pause menu.
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        /// <summary>
        /// Shows pause menu.
        /// </summary>
        public void ShowPauseMenu()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// Hides pause menu.
        /// </summary>
        public void HidePauseMenu()
        {
            gameObject.SetActive(false);
        }
    }
}
