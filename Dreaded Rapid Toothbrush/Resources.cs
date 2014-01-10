using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DRT.Exceptions;

namespace DRT {
    abstract class Resource {
        public enum resourceType {
            Material,
            Energy,
            OtherStuff,
            None
        };

        private resourceType type;
        
        public Resource() {
            this.type = resourceType.OtherStuff;
        }
        public Resource(resourceType t) {
            if (t != resourceType.OtherStuff)
                this.type = t;
            else {
                throw new InvalidResourceTypeException();
            }
        }
    }
}
