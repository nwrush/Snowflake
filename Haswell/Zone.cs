using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class Zone {
        public enum Type {
            Residential,
            Commercial,
            Industrial,
            Conservation,
            Infrastructure
        };

        private System.Drawing.Point corner1, corner2;

        public Zone(int x1, int y1, int x2, int y2) {
            this.corner1 = new System.Drawing.Point(x1, y1);
            this.corner2 = new System.Drawing.Point(x2, y2);
        }
    }
}
