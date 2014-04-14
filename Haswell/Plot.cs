using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Class Plot.
    /// </summary>
    public class Plot : IComparable<Plot> {

        /// <summary>
        /// The zone
        /// </summary>
        private Zones zone;
        /// <summary>
        /// The resource
        /// </summary>
        private ResourceDict resource;
        /// <summary>
        /// The buildings
        /// </summary>
        private List<Building> buildings;

        //Todo: Implement this list
        /// <summary>
        /// The neighbors
        /// </summary>
        private List<Plot> neighbors;

        /// <summary>
        /// The links
        /// </summary>
        private List<Links> Links;

        //Buildings use up different amounts of space on the plot
        //For ex. if a building takes up 5, and the plot has 47/50, then building creation will fail.
        /// <summary>
        /// The plot usage
        /// </summary>
        private float plotUsage = 0.0f;
        /// <summary>
        /// The plot capacity
        /// </summary>
        private float plotCapacity = 1.0f;

        /// <summary>
        /// Occurs when [zone changed].
        /// </summary>
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
        /// <summary>
        /// The plot y
        /// </summary>
        int plotX, plotY;

        /// <summary>
        /// Initializes a new instance of the <see cref="Plot"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Plot(int x, int y) {
            this.resource = new ResourceDict();
            this.buildings = new List<Building>();

            this.plotX = x;
            this.plotY = y;

        }

        private void onBuildingDeleted(object sender, BuildingEventArgs e)
        {
            if (this.BuildingDeleted != null) {
                this.BuildingDeleted.Invoke(sender, e);
            }
        }

        private void onBuildingAdded(object sender, BuildingEventArgs e)
        {
            if (this.BuildingAdded != null)
            {
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
            if (b.Zone != this.Zone) {
                return false;
            }
            if (plotUsage + b.GetPlotUsage() <= plotCapacity && b.Parent == null) {
                this.buildings.Add(b);
                b.Parent = this;
                b.Deleted += this.onBuildingDeleted;
                this.onBuildingAdded(b, new BuildingEventArgs(b));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes the specified b.
        /// </summary>
        /// <param name="b">The b.</param>
        public void Delete(Building b) {
            this.buildings.Remove(b);
            if (b.Parent != null) { b.Parent = null; }
        }

        /// <summary>
        /// Deletes all buildings.
        /// </summary>
        public void DeleteAllBuildings() {
            foreach (Building b in buildings) {
                b.Delete();
            }
            this.buildings.Clear();
        }

        /// <summary>
        /// Updates the specified city resources.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void Update(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.Update(this.resource);
            }
            UpdateCityResources(cityResources);
        }
        /// <summary>
        /// Updates the hour.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void UpdateHour(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateHour(this.resource);
            }
        }
        /// <summary>
        /// Updates the daily.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void UpdateDaily(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateDaily(this.resource);
            }
        }
        /// <summary>
        /// Updates the weekly.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void UpdateWeekly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateWeekly(this.resource);
            }
        }
        /// <summary>
        /// Updates the monthly.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void UpdateMonthly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateMonthly(this.resource);
            }
        }
        /// <summary>
        /// Updates the quaterly.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void UpdateQuaterly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateQuaterly(this.resource);
            }
        }
        /// <summary>
        /// Updates the biannually.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void UpdateBiannually(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateBiannually(this.resource);
            }
        }
        /// <summary>
        /// Updates the yearly.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        public void UpdateYearly(ResourceDict cityResources) {
            foreach (Building b in buildings) {
                b.UpdateYearly(this.resource);
            }
        }

        /// <summary>
        /// Updates the city resources.
        /// </summary>
        /// <param name="cityResources">The city resources.</param>
        private void UpdateCityResources(ResourceDict cityResources) {
            foreach (KeyValuePair<ResourceType, float> kvp in this.resource) {
                if (cityResources.ContainsKey(kvp.Key)) {
                    cityResources[kvp.Key] += kvp.Value;
                }
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
        /// Gets the x.
        /// </summary>
        /// <value>The x.</value>
        public int X {
            get {
                return this.plotX;
            }
        }
        /// <summary>
        /// Gets the y.
        /// </summary>
        /// <value>The y.</value>
        public int Y {
            get {
                return this.plotY;
            }
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        int IComparable<Plot>.CompareTo(Plot other) {
            if (this.plotX == other.plotX && this.plotY == other.plotY)
                return 0;
            return 1;
        }
        /// <summary>
        /// Gets the get all buildings.
        /// </summary>
        /// <value>The get all buildings.</value>
        public List<Building> GetAllBuildings {
            get {
                return this.buildings;
            }
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string ToString() {
            return "Plot at (" + this.X + "," + this.Y + ") with " + this.buildings.Count.ToString() + " buildings.";
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
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public ResourceDict Resources {
            get {
                return this.resource;
            }
        }
    }
}
