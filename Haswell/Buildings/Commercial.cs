using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    public class Commercial : Building {

        public Commercial() : base(Zone.Type.Commercial) { }
        public Commercial(Dictionary<ResourceType, int> r)
            : base(r, Zone.Type.Commercial) {
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
