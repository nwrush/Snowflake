using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Base class for all resources
    /// </summary>
    public abstract class Resource {
        public enum Type {
            Material,
            Energy,
            Money,
            None
        };

        private Type type;
        
        public Resource() {
            this.type = Type.None;
        }
        public Resource(Type t) {
            if (t != Type.None)
                this.type = t;
            else {
                throw new InvalidResourceTypeException();
            }
        }
    }
}
