using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class Plot:IComparable<Plot> {
        public enum Zone {
            residential,
            commercial,
            industrial
        };

        private Dictionary<Resource, ResourceVal> resource;
        private List<Building> buildings;
        private List<Plot> neighbors;

        //Buildings use up different amounts of space on the plot
        //For ex. if a building takes up 5, and the plot has 47/50, then building creation will fail.
        private float plotUsage;
        private float plotCapacity;

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

        /// <summary>
        /// Attempts to add a building to this plot. If successful, returns true, otherwise
        /// returns false.
        /// </summary>
        /// <param name="b">The building to add to the plot.</param>
        /// <returns>Whether or not building addition was successful.</returns>
        public bool AddBuilding(Building b) {
            if (plotUsage + b.GetPlotUsage() <= plotCapacity && b.Parent == null) {
                this.buildings.Add(b);
                b.Parent = this;
                return true;
            }
            return false;
        }


        public void Update() {
            throw new NotImplementedException();
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
