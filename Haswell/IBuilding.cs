using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Interface for methods that all top level buildings need to have
    /// </summary>
    interface IBuilding {
        void UpdateDaily(ResourceDict plotResources);
        void UpdateWeekly(ResourceDict plotResources);
        void UpdateMonthly(ResourceDict plotResources);
        void UpdateQuarterly(ResourceDict plotResources);
        void UpdateBiannually(ResourceDict plotResources);
        void UpdateYearly(ResourceDict plotResources);
    }
}
