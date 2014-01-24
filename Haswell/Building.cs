using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
	public abstract class Building {

		protected Zone.Type zone;
        public Plot Parent;

		Resource.resourceType consumed;
		int amountConsumed;
		Resource.resourceType produced;
		uint amountProduced;

        protected ResourceVal energyConsumed;

        protected bool Initialized;

        protected Building() {

        }

		public Building(int consumed,Resource.resourceType typeConsumed, int produced,Resource.resourceType typeProduced) {
            this.Init(consumed, typeConsumed, produced, typeProduced);
		}

        public void Init(int consumed, Resource.resourceType typeConsumed, int produced, Resource.resourceType typeProduced) {
            this.amountConsumed = consumed;
            this.consumed = typeConsumed;
            this.amountProduced = (uint)produced;
            this.produced = typeProduced;

            Initialized = true;
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

        public int EnergyUsage {
            get {
                return this.energyConsumed.Val;
            }
        }
	}
}
