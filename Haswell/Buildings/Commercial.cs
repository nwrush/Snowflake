using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    /// <summary>
    /// Class Commercial.
    /// </summary>
    public class Commercial : Building {

        /// <summary>
        /// Initializes a new instance of the <see cref="Commercial"/> class.
        /// </summary>
        public Commercial() : base(Zones.Commercial) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Commercial"/> class.
        /// </summary>
        /// <param name="r">The r.</param>
        public Commercial(Dictionary<ResourceType, int> r)
            : base(r, Zones.Commercial) {
            this.resouceChanges.Add(ResourceType.Money, 100);
        }

        /// <summary>
        /// Updates the specified plot resources.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public override void Update(ResourceDict plotResources) { base.Update(plotResources); }
        /// <summary>
        /// Updates the hour.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public override void UpdateHour(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        /// <summary>
        /// Updates the daily.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public override void UpdateDaily(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        /// <summary>
        /// Updates the weekly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public override void UpdateWeekly(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        /// <summary>
        /// Updates the monthly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public override void UpdateMonthly(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        /// <summary>
        /// Updates the yearly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public override void UpdateYearly(ResourceDict plotResources) { base.UpdateHour(plotResources); }
    }
}
