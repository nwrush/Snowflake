using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    public class Residential : Building {
        public Residential():base(Zone.Type.Residential) {
            this.resouceChanges.Add(Resource.Type.Money, 100);
        }

        public override void Update(ResourceDict plotResources) {
            base.Update(plotResources);
        }
    }
}
