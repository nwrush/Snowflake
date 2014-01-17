using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    abstract class Building {
        Zone.Type zone;

        Resource.resourceType consumed;
        int amountConsumed;
        Resource.resourceType produced;
        uint amountProduced;

        protected Building(Zone.Type z) {
            this.zone = z;
        }
        public Building(int consumed,Resource.resourceType typeConsumed, int produced,Resource.resourceType typeProduced,Zone.Type z):this(z) {
            this.amountConsumed = consumed;
            this.consumed = typeConsumed;
            this.amountProduced = (uint)produced;
            this.produced = typeProduced;
        }
    }
}
