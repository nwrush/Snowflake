using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    [Serializable]
    public class Industrial : Building {
        public Industrial()
            : base(Zones.Industrial) {

        }
        protected Industrial(Industrial i)
            : base(Zones.Industrial) {
            this._facing = i.Facing;
            this.Parent = i.Parent;
            this.Initialized = i.Initialized;
        }
        public override void Update(ResourceDict plotResources) {
            base.Update(plotResources);
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
        [Obsolete("Are you sure you need to use this?")]
        public override object Clone() {
            return new Industrial(this);
        }
    }
    public enum IndustrialTypes {
        thing1,
        thing2,
        thing3
    };
}
