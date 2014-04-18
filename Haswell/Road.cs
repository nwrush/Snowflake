using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    class Road:Buildings.Infrastructure {
        public Road[] connections;
        int speed;//In mph?

        public Road()
            : base() {
                this.connections = new Road[8];
                this.speed = 35;
        }
        public Road(int _speed)
            : this() {
                this.speed = _speed;
        }
    }
}
