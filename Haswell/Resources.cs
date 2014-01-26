using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public abstract class Resource {
        public enum resourceType {
            Material,
            Energy,
            Money,
            None
        };

        private resourceType type;
        
        public Resource() {
            this.type = resourceType.None;
        }
        public Resource(resourceType t) {
            if (t != resourceType.None)
                this.type = t;
            else {
                throw new InvalidResourceTypeException();
            }
        }
    }
}
