using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
	public abstract class Building {
		Zone.Type zone;

		Resource.resourceType consumed;
		int amountConsumed;
		Resource.resourceType produced;
		uint amountProduced;

		//#warning This is a potentially bad function
		//public Building() {

		//}
		protected Building(Zone.Type z) {
			this.zone = z;
		}
		public Building(int consumed,Resource.resourceType typeConsumed, int produced,Resource.resourceType typeProduced,Zone.Type z):this(z) {
			this.amountConsumed = consumed;
			this.consumed = typeConsumed;
			this.amountProduced = (uint)produced;
			this.produced = typeProduced;
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
