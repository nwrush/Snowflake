using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    public class Residential : Building {
        public Residential()
            : base(Zone.Type.Residential) {
            this.resouceChanges.Add(ResourceType.Money, 100);
        }

        public override void Update(ResourceDict plotResources) { base.Update(plotResources); }
        public override void UpdateHour(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        public override void UpdateDaily(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        public override void UpdateWeekly(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        public override void UpdateMonthly(ResourceDict plotResources) { base.UpdateHour(plotResources); }
        public override void UpdateYearly(ResourceDict plotResources) { base.UpdateHour(plotResources); }
    }
}
