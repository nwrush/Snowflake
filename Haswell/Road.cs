using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Haswell {
    class Road : Buildings.Infrastructure {
        public Road[] connections;
        int speed;//In mph?

        public Road()
            : base() {
            this.connections = new Road[4];
            this.speed = 35;
        }
        public Road(int _speed)
            : this() {
            this.speed = _speed;
        }
        public void GetConnections() {
            InfiniteGrid grid = Haswell.Controller.City.Grid;
            Point loc = new Point(this.Parent.X, this.Parent.Y);

            if (this.GetType() == grid.ElementAt(loc.X + 1, loc.Y).GetType()) {
                this.connections[0] = grid.ElementAt(loc.X + 1, loc.Y);
            }
        }
    }
}
