using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    class Plot:IComparable<Plot> {
        public enum Zone {
            residential,
            commercial,
            industrial
        };

        private Dictionary<Resource, ResourceVal> resource;
        private List<Building> buildings;
        private List<Plot> neighbors;


        ////These values used until we move to dynamic plots
        //public const int Width = 120;
        //public const int Height = 120;
        //public const int RoadSize = 15;

        //Location of the plot in the city grid
        //Minimum city plot value is (0,0)
        int plotX, plotY;

        public Plot(int x, int y) {
            this.resource = new Dictionary<Resource, ResourceVal>();
            this.buildings = new List<Building>();

            if (x < 0 || y < 0) {
                throw new PlotOutOfRangeException("The position of the plot was negative.");
            } else {
                this.plotX = x;
                this.plotY = y;
            }
        }
        public List<Plot> Neighbors {
            set {
                this.neighbors = value;
            }
        }
        public int X {
            get {
                return this.plotX;
            }
        }
        public int Y {
            get {
                return this.plotY;
            }
        }

        int IComparable<Plot>.CompareTo(Plot other) {
            if (this.plotX == other.plotX && this.plotY == other.plotY)
                return 0;
            return 1;
        }
    }
}
