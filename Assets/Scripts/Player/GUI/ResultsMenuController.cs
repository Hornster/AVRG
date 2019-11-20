﻿using Assets.Scripts.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Player.GUI
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Class that controls the results menu.
    /// </summary>
    public class ResultsMenuController : MonoBehaviour
    {
        /// <summary>
        /// Control that shows the amount of time the player survived.
        /// </summary>
        [SerializeField]
        private TMP_Text _playTimeText;


        void Start()
        {
            
        }
        /// <summary>
        /// Shows results menu together with the time the player survived.
        /// </summary>
        /// <param name="playedTimeMs">The time the player survived, in milliseconds.</param>
        public void ShowResultsMenu(int playedTimeMs)
        {
            var formattedTime = new TimeSpan(0, 0, 0, 0, playedTimeMs);
            _playTimeText.text = formattedTime.ToString(GameConstants.TimeFormat);

            gameObject.SetActive(true);
        }
        /// <summary>
        /// Resets the shown time and hides the results panel.
        /// </summary>
        public void HideResultsMenu()
        {
            _playTimeText.text = GameConstants.TimeTemplate;
            gameObject.SetActive(false);
        }
    }
}