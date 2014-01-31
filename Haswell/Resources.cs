using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Base class for all resources
    /// </summary>
    public abstract class Type {
        public enum Type {
            Material,
            Energy,
            Money,
            Population,
            None
        };

        private Type type;
        
        protected Resource() {
            this.type = Type.None;
        }
        protected Resource(Type t) {
            if (t != Type.None)
                this.type = t;
            else {
                throw new InvalidResourceTypeException();
            }
        }
    }
}
