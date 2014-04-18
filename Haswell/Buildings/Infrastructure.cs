using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    public class Infrastructure : Building {
        public Infrastructure()
            : base(Zones.Infrastructure) {

        }

        public override void Update(ResourceDict plotResources) {
            base.Update(plotResources);
        }
        public override void UpdateHour(ResourceDict plotResources) {
            base.UpdateHour(plotResources);
        }
        public override void UpdateDaily(ResourceDict plotResources) {
            base.UpdateDaily(plotResources);
        }
        public override void UpdateWeekly(ResourceDict plotResources) {
            base.UpdateWeekly(plotResources);
        }
        public override void UpdateMonthly(ResourceDict plotResources) {
            base.UpdateMonthly(plotResources);
        }
        public override void UpdateQuarterly(ResourceDict plotResources) {
            base.UpdateQuarterly(plotResources);
        }
        public override void UpdateBiannually(ResourceDict plotResources) {
            base.UpdateBiannually(plotResources);
        }
        public override void UpdateYearly(ResourceDict plotResources) {
            base.UpdateYearly(plotResources);
        }
    }
}
