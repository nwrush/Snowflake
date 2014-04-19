using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Haswell {
    public class Road : Buildings.Infrastructure {
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
            if (this.GetType() == grid.ElementAt(loc.X + 1, loc.Y).Building.GetType()) {//North
                this.connections[0] = (Road)grid.ElementAt(loc.X + 1, loc.Y).Building;
            }
            if (this.GetType() == grid.ElementAt(loc.X, loc.Y + 1).Building.GetType()) {//East
                this.connections[1] = (Road)grid.ElementAt(loc.X, loc.Y + 1).Building;
            }
            if (this.GetType() == grid.ElementAt(loc.X - 1, loc.Y).Building.GetType()) {//South
                this.connections[2] = (Road)grid.ElementAt(loc.X - 1, loc.Y).Building;
            }
            if (this.GetType() == grid.ElementAt(loc.X, loc.Y - 1).Building.GetType()) {//West
                this.connections[3] = (Road)grid.ElementAt(loc.X, loc.Y - 1).Building;
            }
        }
    }
}
