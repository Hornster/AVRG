using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Factories;

namespace Assets.Scripts.Maps.Interfaces
{
    public interface IPoolable
    {

        /// <summary>
        /// Applies new data to obstacle.
        /// </summary>
        /// <param name="newData">Data to be applied to obstacle.</param>
        void Mutate(ObstacleIniData newData);
        /// <summary>
        /// Enables the gameobject of the obstacle.
        /// </summary>
        void Enable();
        /// <summary>
        /// Disables the gameobject of the obstacle.
        /// </summary>
        void Disable();
    }
}
