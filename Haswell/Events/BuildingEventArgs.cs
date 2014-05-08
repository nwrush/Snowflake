using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    /// <summary>
    /// Class BuildingEventArgs.
    /// </summary>
    public class BuildingEventArgs : EventArgs
    {

        /// <summary>
        /// Gets the building.
        /// </summary>
        /// <value>The building.</value>
        public Building Building { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingEventArgs"/> class.
        /// </summary>
        /// <param name="b">The b.</param>
        public BuildingEventArgs(Building b)
        {
            this.Building = b;
        }
    }
}
