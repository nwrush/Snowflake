﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    /// <summary>
    /// Class Infrastructure.
    /// </summary>
    public class Infrastructure : Building {
        /// <summary>
        /// Initializes a new instance of the <see cref="Infrastructure"/> class.
        /// </summary>
        public Infrastructure()
            : base(Zones.Infrastructure) {

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
