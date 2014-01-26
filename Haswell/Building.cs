using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public abstract class Building {

        protected Zone.Type zone;
        public Plot Parent;

        Resource.Type consumed;
        int amountConsumed;
        Resource.Type produced;
        uint amountProduced;

        //Todo: Implement this into the constructor and init function
        protected Dictionary<Resource.Type, int> resouceChanges;

        protected bool Initialized;

        protected Building(Zone.Type zone) {
            this.resouceChanges = new Dictionary<Resource.Type, int>();
            this.Initialized = true;
        }
        protected Building(Dictionary<Resource.Type, int> resource, Zone.Type zone)
            : this(zone) {
            this.resouceChanges = resource;
            this.Initialized = true;
        }

        [Obsolete("Instead pass a dictionary of the Resources to change")]
        protected Building(int consumed, Resource.Type typeConsumed, int produced, Resource.Type typeProduced) {
            this.Init(consumed, typeConsumed, produced, typeProduced);
        }

        [Obsolete("Pass a Resource dictionary to the constructor")]
        public void Init(int consumed, Resource.Type typeConsumed, int produced, Resource.Type typeProduced) {
            this.amountConsumed = consumed;
            this.consumed = typeConsumed;
            this.amountProduced = (uint)produced;
            this.produced = typeProduced;

            Initialized = true;
        }

        public void Update(Dictionary<Resource.Type, int> plotResources) {
            foreach (KeyValuePair<Resource.Type, int> kvp in this.resouceChanges) {
                if (plotResources.ContainsKey(kvp.Key)) {
                    plotResources[kvp.Key] += kvp.Value;
                }
            }
        }

        /// <summary>
        /// Gives the virtual amount of space this building takes up
        /// on the plot.
        /// </summary>
        /// <returns>Float representing the amount of space on the plot</returns>
        public float GetPlotUsage() {
            return 1.0f;
            //Override in child classes
        }
    }
}
