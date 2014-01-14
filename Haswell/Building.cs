using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    abstract class Building {
        Zone.Type zone;

        Resource.resourceType consumed;
        Resource.resourceType produced;

        protected Building(Zone.Type z) {
            this.zone = z;
        }
    }
}
