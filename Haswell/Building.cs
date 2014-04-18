using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
        public abstract class Building {

        protected readonly Zones _zone;
        protected Facing _facing;
        private Plot parent;

        //Todo: Implement this into the constructor and init function
        protected Dictionary<ResourceType, int> resouceChanges;
        public event EventHandler<BuildingEventArgs> Deleted;

        protected bool Initialized;
        protected Building(Zones zone) {
            this._zone = zone;
            this.resouceChanges = new Dictionary<ResourceType, int>();
            this.Initialized = true;
            this.Deleted += OnDeleted;
            this._facing = Facing.East;
        }
        protected Building(Dictionary<ResourceType, int> resource, Zones zone)
            : this(zone) {
            this.resouceChanges = resource;
            this.Initialized = true;
            this.Deleted += OnDeleted;
            this._facing = Facing.East;
        }

        /// <summary>
        /// Updates the specified plot resources.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void Update(ResourceDict plotResources) { }

        public virtual void UpdateHour(ResourceDict plotResources) { }
        public virtual void UpdateDaily(ResourceDict plotResources) { }
        public virtual void UpdateWeekly(ResourceDict plotResources) { }
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
        public Facing Facing
        {
            get
            {
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
