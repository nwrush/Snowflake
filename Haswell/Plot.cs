using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Haswell {
    /// <summary>
    /// Class Plot.
    /// </summary>
    [Serializable]
    public class Plot : IComparable<Plot>, ISerializable {

        private Zones zone = Zones.Unzoned;
        private ResourceDict resource;
        private Building building;

        //private List<Links> Links;

        internal InfiniteGrid grid;
        internal List<Plot> hookedPlots;

        public event EventHandler ZoneChanged {
            add { _zoneChanged += value; }
            remove { _zoneChanged -= value; }
        }
        [NonSerialized]
        private EventHandler _zoneChanged;
        /// <summary>
        /// Occurs when a building is deleted
        /// </summary>
        public event EventHandler<BuildingEventArgs> BuildingDeleted {
            add { _buildingDeleted += value; }
            remove { _buildingDeleted -= value; }
        }
        [NonSerialized]
        private EventHandler<BuildingEventArgs> _buildingDeleted;
        /// <summary>
        /// Occurs when a building is added
        /// </summary>
        public event EventHandler<BuildingEventArgs> BuildingAdded {
            add { _buildingAdded += value; }
            remove { _buildingAdded -= value; }
        }
        [NonSerialized]
        private EventHandler<BuildingEventArgs> _buildingAdded;
        /// <summary>
        /// Occurs when an adjacent plot fires a BuildingAdded event
        /// </summary>
        public event EventHandler<BuildingEventArgs> AdjacentBuildingChanged {
            add { _adjacentBuildingChanged += value; }
            remove { _adjacentBuildingChanged -= value; }
        }
        [NonSerialized]
        private EventHandler<BuildingEventArgs> _adjacentBuildingChanged;

        //Location of the plot in the city grid
        //Minimum city plot value is (0,0)
        int plotX, plotY;

        internal Plot(int x, int y, InfiniteGrid grid) {
            this.grid = grid;
            initialize(x, y);
            this.resource = new ResourceDict();
        }

        public Plot(int x, int y) {
            initialize(x, y);
            this.resource = new ResourceDict();
        }
        /// <summary>
        /// Performs construction functions that must be done after the grid is non-null
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void initialize(int x, int y) {
            this.hookedPlots = new List<Plot>();
            this.BuildingAdded += (object sender, BuildingEventArgs e) => {
                e.Building.UpdateFacing();
            };

            this.plotX = x;
            this.plotY = y;

            UpdateAdjacentEventHandlers();
        }

        internal void UpdateAdjacentEventHandlers() {
            foreach (Plot p in GetAdjacentPlots().Values) {
                if (!p.hookedPlots.Contains(this)) {
                    p.BuildingAdded += InvokeAdjacentEvent;
                    p.UpdateAdjacentEventHandlers();
                    p.InvokeAdjacentEvent(this, new BuildingEventArgs(this.Building));
                    p.hookedPlots.Add(this);
                }
            }
        }

        internal void InvokeAdjacentEvent(object sender, BuildingEventArgs e) {
            if (_adjacentBuildingChanged != null) { _adjacentBuildingChanged.Invoke(sender, e); }
        }

        private void onBuildingDeleted(object sender, BuildingEventArgs e) {
            if (this._buildingDeleted != null) {
                this._buildingDeleted.Invoke(sender, e);
            }
        }

        private void onBuildingAdded(object sender, BuildingEventArgs e) {
            if (this._buildingAdded != null) {
                this._buildingAdded.Invoke(sender, e);
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
        public void UpdateDaily() {
            if (this.Building != null)
                this.building.UpdateDaily(this.resource);
        }
        public void UpdateWeekly() {
            if (this.Building != null)
                this.building.UpdateWeekly(this.resource);
        }
        public void UpdateMonthly() {
            if (this.Building != null)
                this.building.UpdateMonthly(this.resource);
        }
        public void UpdateQuarterly() {
            if (this.Building != null)
                this.building.UpdateQuarterly(this.resource);
        }
        public void UpdateBiannually() {
            if (this.Building != null)
                this.building.UpdateBiannually(this.resource);
        }
        public void UpdateYearly() {
            if (this.Building != null)
                this.building.UpdateYearly(this.resource);
        }

        private void UpdateCityResources(ResourceDict cityResources) {
            cityResources = cityResources + this.resource;
        }

        public Dictionary<Direction, Plot> GetAdjacentPlots() {
            Dictionary<Direction, Plot> adj = new Dictionary<Direction, Plot>();
            InfiniteGrid _grid;

            if (this.grid != null) { _grid = this.grid; } else { _grid = Controller.City.Grid; }

            foreach (Plot p in _grid.GetNeighbors(this)) {
                if (p.X > X) { //+X direction, or North
                    adj[Direction.North] = p;
                } else if (p.X < X) //-X direction, or South
                {
                    adj[Direction.South] = p;
                } else if (p.Y > Y) //+Y direction, or East
                {
                    adj[Direction.East] = p;
                } else if (p.Y < Y) //-Y direction, or West
                {
                    adj[Direction.West] = p;
                }
            }
            return adj;
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
                if (this._zoneChanged != null) { this._zoneChanged.Invoke(this, new EventArgs()); }
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
        /// Gets the Building located on this plot
        /// </summary>
        public Building Building {
            get {
                return this.building;
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {

        }
    }
}
