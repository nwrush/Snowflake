using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class Plot : IComparable<Plot> {

        private Zones zone;
        private ResourceDict resource;
        private List<Building> buildings;

        //Todo: Implement this list
        private List<Plot> neighbors;

        //Buildings use up different amounts of space on the plot
        //For ex. if a building takes up 5, and the plot has 47/50, then building creation will fail.
        private float plotUsage = 0.0f;
        private float plotCapacity = 1.0f;

        ////These values used until we move to dynamic plots
        //public const int Width = 120;
        //public const int Height = 120;
        //public const int RoadSize = 15;

        //Location of the plot in the city grid
        //Minimum city plot value is (0,0)
        int plotX, plotY;

        public Plot(int x, int y) {
            this.resource = new ResourceDict();
            this.buildings = new List<Building>();

            this.plotX = x;
            this.plotY = y;

        }

        /// <summary>
        /// Attempts to add a building to this plot. If successful, returns true, otherwise
        /// returns false.
        /// </summary>
        /// <param name="b">The building to add to the plot.</param>
        /// <returns>Whether or not building addition was successful.</returns>
        public bool AddBuilding(Building b) {
            if (b.Zone != this.Zone) {
                return false;
            }
            if (plotUsage + b.GetPlotUsage() <= plotCapacity && b.Parent == null) {
                this.buildings.Add(b);
                b.Parent = this;
                return true;
            }
            return false;
        }

        public void Delete(Building b) {
            this.buildings.Remove(b);
            if (b.Parent != null) { b.Parent = null; }
        }

        public void DeleteAllBuildings() {
            foreach (Building b in buildings) {
                b.Delete();
            }
            this.buildings.Clear();
        }

        public void Update(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.Update(this.resource);
            }
            UpdateCityResources(cityResources);
        }
        public void UpdateHour(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateHour(this.resource);
            }
        }
        public void UpdateDaily(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateDaily(this.resource);
            }
        }
        public void UpdateWeekly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateWeekly(this.resource);
            }
        }
        public void UpdateMonthly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateMonthly(this.resource);
            }
        }
        public void UpdateQuaterly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateQuaterly(this.resource);
            }
        }
        public void UpdateBiannually(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateBiannually(this.resource);
            }
        }
        public void UpdateYearly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateYearly(this.resource);
            }
        }

        private void UpdateCityResources(ResourceDict cityResources) {
            foreach (KeyValuePair<ResourceType, float> kvp in this.resource) {
                if (cityResources.ContainsKey(kvp.Key)) {
                    cityResources[kvp.Key] += kvp.Value;
                }
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
        public List<Building> GetAllBuildings {
            get {
                return this.buildings;
            }
        }

        public override string ToString() {
            return "Plot at (" + this.X + "," + this.Y + ") with " + this.buildings.Count.ToString() + " buildings.";
        }

        public Zones Zone {
            get {
                return this.zone;
            }
            set {
                this.zone = value;
            }
        }
        public ResourceDict Resources {
            get {
                return this.resource;
            }
        }
    }
}
