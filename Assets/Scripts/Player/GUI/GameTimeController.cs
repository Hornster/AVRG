using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player.GUI
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Used to measure and show to the user the time they managed to survive.
    /// </summary>
    public class GameTimeController : MonoBehaviour
    {
        [SerializeField]
        private Text _gameElapsedTime;
        /// <summary>
        /// Time since the beginning of the measurement.
        /// </summary>
        private float _currentTime;

        void Update()
        {
            UpdateTimer();
        }
        /// <summary>
        /// Updates what shows the timer on the screen.
        /// </summary>
        private void UpdateTimer()
        {
            _currentTime += Time.deltaTime*1000;//Get milliseconds.
            var formattedTime = new TimeSpan(0,0,0,0, (int)_currentTime);
            _gameElapsedTime.text = formattedTime.ToString(@"hh\:mm\:ss\.fff");// + $".{formattedTime.Milliseconds}";
        }
    }
}
