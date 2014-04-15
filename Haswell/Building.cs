using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Class Building.
    /// </summary>
    public abstract class Building {

        /// <summary>
        /// The zone
        /// </summary>
        protected Zones zone;
        /// <summary>
        /// The parent
        /// </summary>
        public Plot Parent;

        //Todo: Implement this into the constructor and init function
        /// <summary>
        /// The resouce changes
        /// </summary>
        protected Dictionary<ResourceType, int> resouceChanges;
        /// <summary>
        /// Occurs when [deleted].
        /// </summary>
        public event EventHandler<BuildingEventArgs> Deleted;

        /// <summary>
        /// The initialized
        /// </summary>
        protected bool Initialized;
        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        /// <param name="zone">The zone.</param>
        protected Building(Zones zone) {
            this.resouceChanges = new Dictionary<ResourceType, int>();
            this.Initialized = true;
            this.Deleted += OnDeleted;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Building"/> class.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <param name="zone">The zone.</param>
        protected Building(Dictionary<ResourceType, int> resource, Zones zone)
            : this(zone) {
            this.resouceChanges = resource;
            this.Initialized = true;
            this.Deleted += OnDeleted;
        }

        /// <summary>
        /// Updates the specified plot resources.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void Update(ResourceDict plotResources) { }
        /// <summary>
        /// Updates the hour.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void UpdateHour(ResourceDict plotResources) { }
        /// <summary>
        /// Updates the daily.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void UpdateDaily(ResourceDict plotResources) { }
        /// <summary>
        /// Updates the weekly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void UpdateWeekly(ResourceDict plotResources) { }
        /// <summary>
        /// Updates the monthly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void UpdateMonthly(ResourceDict plotResources) { }
        /// <summary>
        /// Updates the Quarterly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void UpdateQuarterly(ResourceDict plotResources) { }
        /// <summary>
        /// Updates the biannually.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void UpdateBiannually(ResourceDict plotResources) { }
        /// <summary>
        /// Updates the yearly.
        /// </summary>
        /// <param name="plotResources">The plot resources.</param>
        public virtual void UpdateYearly(ResourceDict plotResources) { }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete() {
            if (Deleted != null) {
                Deleted.Invoke(this, new BuildingEventArgs(this));
            }
        }

        /// <summary>
        /// Handles the <see cref="E:Deleted"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
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
        /// <summary>
        /// Gets the zone.
        /// </summary>
        /// <value>The zone.</value>
        public Zones Zone {
            get {
                return this.zone;
            }
            private set {
                this.zone = value;
            }
        }
    }
}
