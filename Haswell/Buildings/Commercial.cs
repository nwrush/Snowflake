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

        public override void Update(ResourceDict plotResources) {
            base.Update(plotResources);
        }
    }
}
