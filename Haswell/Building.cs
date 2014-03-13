using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public abstract class Building {

        protected ZoneTypes zone;
        public Plot Parent;

        //Todo: Implement this into the constructor and init function
        protected Dictionary<ResourceType, int> resouceChanges;
        public event EventHandler Deleted;

        protected bool Initialized;
        protected Building(ZoneTypes zone) {
            this.resouceChanges = new Dictionary<ResourceType, int>();
            this.Initialized = true;
            this.Deleted += OnDeleted;
        }

        protected Building(Dictionary<ResourceType, int> resource, ZoneTypes zone)
            : this(zone) {
            this.resouceChanges = resource;
            this.Initialized = true;
            this.Deleted += OnDeleted;
        }

        public virtual void Update(ResourceDict plotResources) { }
        public virtual void UpdateHour(ResourceDict plotResources) { }
        public virtual void UpdateDaily(ResourceDict plotResources) { }
        public virtual void UpdateWeekly(ResourceDict plotResources) { }
        public virtual void UpdateMonthly(ResourceDict plotResources) { }
        public virtual void UpdateQuaterly(ResourceDict plotResources) { }
        public virtual void UpdateBiannually(ResourceDict plotResources) { }
        public virtual void UpdateYearly(ResourceDict plotResources) { }

        public void Delete() {
            if (Deleted != null) {
                Deleted.Invoke(this, new EventArgs());
            }
        }

        private void OnDeleted(object sender, EventArgs e) {
            this.Parent.Delete(this);
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
        public ZoneTypes Zone {
            get {
                return this.Zone;
            }
            private set {
                this.zone = value;
            }
        }
    }
}
