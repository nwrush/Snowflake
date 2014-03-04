using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    interface IBuilding {
        void UpdateHour(ResourceDict plotResources);
        void UpdateDaily(ResourceDict plotResources);
        void UpdateWeekly(ResourceDict plotResources);
        void UpdateMonthly(ResourceDict plotResources);
        void UpdateYearly(ResourceDict plotResources);
    }
}
