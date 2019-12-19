using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Data;
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
        private bool _isCounting;
        [SerializeField]
        private Text _gameElapsedTime;
        /// <summary>
        /// Time since the beginning of the measurement.
        /// </summary>
        public TimeSpan CurrentPlayTime { get; private set; }
        /// <summary>
        /// Stores the time that the round started at.
        /// </summary>
        private TimeSpan _roundStartTime = new TimeSpan();
        
        void Update()
        {
            if (_isCounting == false)
            {
                return;
            }

            UpdateTimer();
        }
        /// <summary>
        /// Updates what shows the timer on the screen.
        /// </summary>
        private void UpdateTimer()
        {
            var timeDelta = Time.deltaTime * 1000;
            CurrentPlayTime += new TimeSpan(0,0,0,0,(int)timeDelta);
            
            _gameElapsedTime.text = CurrentPlayTime.ToString(GameConstants.TimeFormat);
        }
        /// <summary>
        /// Halts the clock.
        /// </summary>
        public void StopCounting()
        {
            _isCounting = false;
        }
        /// <summary>
        /// Resets the measurement and begins counting immediately after.
        /// </summary>
        public void RestartCounting()
        {
            CurrentPlayTime = new TimeSpan();
            _roundStartTime = DateTime.Now.TimeOfDay;
            _isCounting = true;
        }
        /// <summary>
        /// Resumes counting the playtime.
        /// </summary>
        public void ResumeCounting()
        {
            _isCounting = true;
        }
        /// <summary>
        /// Shows the timer.
        /// </summary>
        public void ShowTimer()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// Hides the timer.
        /// </summary>
        public void HideTimer()
        {
            gameObject.SetActive(false);
        }
    }
}
