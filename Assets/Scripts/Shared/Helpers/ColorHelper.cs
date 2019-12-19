using Assets.Scripts.Shared.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Class storing colors found in the game, like selected objects highlights (dependent on selecting entity's team).
    /// </summary>
    public static class ColorHelper
    {
        private static Dictionary<TeamEnum, Color> _highlightColor = new Dictionary<TeamEnum, Color>();

        static ColorHelper()
        {
            _highlightColor.Add(TeamEnum.Enemy, Color.magenta);
            _highlightColor.Add(TeamEnum.PlayerGreen, Color.green);
            _highlightColor.Add(TeamEnum.PlayerRed, Color.red);
        }
        /// <summary>
        /// Returns color by which a selected object can be highlighted, basing on provided team.
        /// </summary>
        /// <param name="team">Team that selected an object.</param>
        /// <returns></returns>
        public static Color GetHighlightColor(TeamEnum team)
        {
            Color color;
            if (_highlightColor.TryGetValue(team, out color) == false)
            {
                throw new Exception($"Error: team {team} has no color assigned!");
            }

            return color;
        }
    }
}
