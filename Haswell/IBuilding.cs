using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Interface for methods that all top level buildings need to have
    /// </summary>
    interface IBuilding {
        /// <summary>
        /// Updates the hour.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        void UpdateHour(ResourceDict plotResources);
        /// <summary>
        /// Updates the daily.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        void UpdateDaily(ResourceDict plotResources);
        /// <summary>
        /// Updates the weekly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        void UpdateWeekly(ResourceDict plotResources);
        /// <summary>
        /// Updates the monthly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        void UpdateMonthly(ResourceDict plotResources);
        /// <summary>
        /// Updates the Quarterly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        void UpdateQuarterly(ResourceDict plotResources);
        /// <summary>
        /// Updates the biannually.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        void UpdateBiannually(ResourceDict plotResources);
        /// <summary>
        /// Updates the yearly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        void UpdateYearly(ResourceDict plotResources);
    }
}
