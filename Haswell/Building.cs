using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    [Serializable]
    public abstract class Building {

        protected readonly Zones _zone;
        protected Direction _facing;
        private Plot parent;

        //Todo: Implement this into the constructor and init function
        protected Dictionary<ResourceType, int> resourceChanges;
        public event EventHandler<BuildingEventArgs> Deleted;

        protected bool Initialized;
        protected Building(Zones zone) {
            this._zone = zone;
            this.resourceChanges = new Dictionary<ResourceType, int>();
            this.Initialized = true;
            this.Deleted += OnDeleted;
            this._facing = Direction.North;
        }
        protected Building(Dictionary<ResourceType, int> resource, Zones zone)
            : this(zone) {
            this.resourceChanges = resource;
            this.Initialized = true;
            this.Deleted += OnDeleted;
            this._facing = Direction.North;
        }

        public Dictionary<Direction, Plot> GetAdjacentPlots() {
            Dictionary<Direction, Plot> adj = new Dictionary<Direction, Plot>();
            foreach (Plot p in Controller.City.Grid.GetNeighbors(this.Parent)) {
                if (p.X > this.parent.X) { //+X direction, or North
                    adj[Direction.North] = p;
                } else if (p.X < this.parent.X) //-X direction, or South
                {
                    adj[Direction.South] = p;
                } else if (p.Y > this.parent.Y) //+Y direction, or East
                {
                    adj[Direction.East] = p;
                } else if (p.Y < this.parent.Y) //-Y direction, or West
                {
                    adj[Direction.West] = p;
                }
            }
            return adj;
        }

        public Dictionary<Direction, Building> GetAdjacentBuildings() {
            Dictionary<Direction, Building> adj = new Dictionary<Direction, Building>();
            foreach (KeyValuePair<Direction, Plot> kvp in GetAdjacentPlots()) {
                if (kvp.Value.Building != null) {
                    adj[kvp.Key] = kvp.Value.Building;
                }
            }
            return adj;
        }

        public event EventHandler WeeklyUpdate;

        /// <summary>
        /// Updates the specified plot resources.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void Update(ResourceDict plotResources) { }

        public virtual void UpdateHour(ResourceDict plotResources) { }
        public virtual void UpdateDaily(ResourceDict plotResources) { }
        public virtual void UpdateWeekly(ResourceDict plotResources) { if (WeeklyUpdate != null) { WeeklyUpdate.Invoke(this, new EventArgs()); } }
        public virtual void UpdateMonthly(ResourceDict plotResources) { }
        public virtual void UpdateQuarterly(ResourceDict plotResources) { }
        public virtual void UpdateBiannually(ResourceDict plotResources) { }
        public virtual void UpdateYearly(ResourceDict plotResources) { }

        public void Delete() {
            if (Deleted != null) {
                Deleted.Invoke(this, new BuildingEventArgs(this));
            }
        }

        private void OnDeleted(object sender, EventArgs e) {
            this.Parent.Delete();
            this.Parent = null;
            //Do a thing
        }

        /// <summary>
        /// Gives the virtual amount of space this building takes up
        /// on the plot.
        /// </summary>
        /// <returns>Float representing the amount of space on the plot</returns>
        public float GetPlotUsage() {
            return 1.0f;
            //Override in child classes
        }

        public Zones Zone {
            get {
                return this._zone;
            }
        }
        public Direction Facing {
            get {
                return this._facing;
            }
        }
        public Plot Parent {
            get {
                return this.parent;
            }
            set {
                this.parent = value;
            }
        }
    }
}
