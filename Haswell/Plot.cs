using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Class Plot.
    /// </summary>
    public class Plot : IComparable<Plot> {

        private Zones zone = Zones.Unzoned;
        private ResourceDict resource;
        private Building building;

        //Todo: Implement this list
        private List<Plot> neighbors;

        private List<Links> Links;

        //Buildings use up different amounts of space on the plot
        //For ex. if a building takes up 5, and the plot has 47/50, then building creation will fail.
        private float plotUsage = 0.0f;
        private float plotCapacity = 1.0f;

        public event EventHandler ZoneChanged;
        /// <summary>
        /// Occurs when a building is deleted
        /// </summary>
        public event EventHandler<BuildingEventArgs> BuildingDeleted;
        /// <summary>
        /// Occurs when a building is added
        /// </summary>
        public event EventHandler<BuildingEventArgs> BuildingAdded;

        //Location of the plot in the city grid
        //Minimum city plot value is (0,0)
        int plotX, plotY;

        public Plot(int x, int y) {
            this.resource = new ResourceDict();

            this.plotX = x;
            this.plotY = y;

        }

        private void onBuildingDeleted(object sender, BuildingEventArgs e) {
            if (this.BuildingDeleted != null) {
                this.BuildingDeleted.Invoke(sender, e);
            }
        }

        private void onBuildingAdded(object sender, BuildingEventArgs e) {
            if (this.BuildingAdded != null) {
                this.BuildingAdded.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Attempts to add a building to this plot. If successful, returns true, otherwise
        /// returns false.
        /// </summary>
        /// <param name="b">The building to add to the plot.</param>
        /// <returns>Whether or not building addition was successful.</returns>
        public bool AddBuilding(Building b) {
            if (b.Zone != this.Zone && !(b is Road)) {
                return false;
            }
            if (b.Parent == null && this.Building == null) {
                this.building = b;
                b.Parent = this;
                b.Deleted += this.onBuildingDeleted;
                this.onBuildingAdded(b, new BuildingEventArgs(b));
                return true;
            }
            return false;
        }

        public void Delete() {
            this.building = null;
        }

        public void Update(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.Update(this.resource);
            UpdateCityResources(cityResources);
        }
        public void UpdateHour(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.UpdateHour(this.resource);
        }
        public void UpdateDaily(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.UpdateDaily(this.resource);
        }
        public void UpdateWeekly(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.UpdateWeekly(this.resource);
        }
        public void UpdateMonthly(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.UpdateMonthly(this.resource);
        }
        public void UpdateQuarterly(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.UpdateQuarterly(this.resource);
        }
        public void UpdateBiannually(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.UpdateBiannually(this.resource);
        }
        public void UpdateYearly(ResourceDict cityResources) {
            if (this.Building != null)
                this.building.UpdateYearly(this.resource);
        }

        private void UpdateCityResources(ResourceDict cityResources) {
            foreach (KeyValuePair<ResourceType, float> kvp in this.resource) {
                if (cityResources.ContainsKey(kvp.Key)) {
                    cityResources[kvp.Key] += kvp.Value;
                }
            }
        }


        int IComparable<Plot>.CompareTo(Plot other) {
            if (this.plotX == other.plotX && this.plotY == other.plotY)
                return 0;
            return 1;
        }

        public override string ToString() {
            return "Plot at (" + this.X + "," + this.Y + ").";
        }

        /// <summary>
        /// Gets or sets the zone.
        /// </summary>
        /// <value>The zone.</value>
        public Zones Zone {
            get {
                return this.zone;
            }
            set {
                this.zone = value;
                if (this.ZoneChanged != null) { this.ZoneChanged.Invoke(this, new EventArgs()); }
            }
        }
        /// <summary>
        /// Gets the current plot resources
        /// </summary>
        public ResourceDict Resources {
            get {
                return this.resource;
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
        /// <summary>
        /// Sets the neighbors.
        /// </summary>
        /// <value>The neighbors.</value>
        public List<Plot> Neighbors {
            set {
                this.neighbors = value;
            }
        }
        /// <summary>
        /// Gets the Building located on this plot
        /// </summary>
        public Building Building {
            get {
                return this.building;
            }
        }
    }
}
