using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public struct Zone {

        private System.Drawing.Point corner1, corner2;
        ZoneTypes type;

        public Zone(int x1, int y1, int x2, int y2,ZoneTypes type) {
            this.corner1 = new System.Drawing.Point(x1, y1);
            this.corner2 = new System.Drawing.Point(x2, y2);
            this.type = type;
        }
    }

    public enum ZoneTypes {
        Residential,
        Commercial,
        Industrial,
        Conservation,
        Infrastructure
    };
}
