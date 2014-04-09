using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Haswell {
    /// <summary>
    /// Class City.
    /// </summary>
    public class City {

        /// <summary>
        /// The name
        /// </summary>
        private string name;
        /// <summary>
        /// The grid
        /// </summary>
        private InfiniteGrid grid;

        /// <summary>
        /// The pipes
        /// </summary>
        private List<Links> pipes;
        /// <summary>
        /// The citizens
        /// </summary>
        private List<Person> Citizens;

        /// <summary>
        /// The resources
        /// </summary>
        private ResourceDict resources;

        /// <summary>
        /// Occurs when [building created].
        /// </summary>
        public event EventHandler<BuildingEventArgs> BuildingCreated;
        /// <summary>
        /// Occurs when [building updated].
        /// </summary>
        public event EventHandler<BuildingEventArgs> BuildingUpdated;

        /// <summary>
        /// Creates a city and initialize's it with the given sides
        /// </summary>
        /// <param name="name">Name of the city</param>
        public City(string name) {
            //System.Media.SoundPlayer sp = new System.Media.SoundPlayer("../Media/a.wav");
            //System.Diagnostics.Debug.WriteLine("I REGRET NOTHING");
            //sp.PlayLooping();
            this.name = name;
            this.grid = new InfiniteGrid();

            this.pipes = new List<Links>();
            this.Citizens = new List<Person>();

            initResources();
            initUpdateMethods();
        }
        /// <summary>
        /// Sets the default Resource values
        /// </summary>
        private void initResources() {
            this.resources = new ResourceDict();
            this.resources[ResourceType.Energy] = 10000;
            this.resources[ResourceType.Material] = 10000;
            this.resources[ResourceType.Money] = 10000;
            this.resources[ResourceType.Population] = 10000;
        }
        /// <summary>
        /// Adds the timed event handlers to the controller
        /// </summary>
        private void initUpdateMethods() {
            Controller.Environment.Hourly += Environment_Hourly;
            Controller.Environment.Daily += Environment_Daily;
            Controller.Environment.Weekly += Environment_Weekly;
            Controller.Environment.Monthly += Environment_Monthly;
            Controller.Environment.Quarterly += Environment_Quarterly;
            Controller.Environment.Biannually += Environment_Biannually;
            Controller.Environment.Yearly += Environment_Yearly;
        }


        /// <summary>
        /// Environment_s the hourly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Haswell.TimeEventArgs"/> instance containing the event data.</param>
        void Environment_Hourly(object sender, TimeEventArgs e) { this.UpdateHour(e.Time); }
        /// <summary>
        /// Environment_s the daily.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Haswell.TimeEventArgs"/> instance containing the event data.</param>
        void Environment_Daily(object sender, TimeEventArgs e) { this.UpdateDaily(e.Time); }
        /// <summary>
        /// Environment_s the weekly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Haswell.TimeEventArgs"/> instance containing the event data.</param>
        void Environment_Weekly(object sender, TimeEventArgs e) { this.UpdateWeekly(e.Time); }
        /// <summary>
        /// Environment_s the monthly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Haswell.TimeEventArgs"/> instance containing the event data.</param>
        void Environment_Monthly(object sender, TimeEventArgs e) { this.UpdateMonthly(e.Time); }
        /// <summary>
        /// Environment_s the quarterly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Haswell.TimeEventArgs"/> instance containing the event data.</param>
        void Environment_Quarterly(object sender, TimeEventArgs e) { this.UpdateQuaterly(e.Time); }
        /// <summary>
        /// Environment_s the biannually.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Haswell.TimeEventArgs"/> instance containing the event data.</param>
        void Environment_Biannually(object sender, TimeEventArgs e) { this.UpdateBiannually(e.Time); }
        /// <summary>
        /// Environment_s the yearly.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Haswell.TimeEventArgs"/> instance containing the event data.</param>
        void Environment_Yearly(object sender, TimeEventArgs e) { this.UpdateYearly(e.Time); }

        /// <summary>
        /// Called by Snowflake when the user requests the creation of a building
        /// Throws an error if something goes wrong.
        /// </summary>
        /// <typeparam name="T">The Type of Building to create</typeparam>
        /// <param name="x">The plot X of the building</param>
        /// <param name="y">The plot Y of the building</param>
        public void CreateBuilding<T>(int x, int y) where T : Building, new() {
            Building b = new T();
            if (grid.ElementAt(x, y).AddBuilding(b)) {
                BuildingCreated.Invoke(this, new BuildingEventArgs(b));
                return;
            }

            throw new Exceptions.BuildingCreationFailedException("Building creation failed");
        }

        /// <summary>
        /// Deletes the building.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void DeleteBuilding(int x, int y) {
            if (grid.ElementAt(x, y).GetAllBuildings.Count > 0) {
                grid.ElementAt(x, y).DeleteAllBuildings();
            }
        }

        /// <summary>
        /// Sets the zoning.
        /// </summary>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="z">The z.</param>
        public void SetZoning(Point p1, Point p2, Zones z)
        {
            Point tl = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            Point br = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));
            for (int x = tl.X; x <= br.X; ++x)
            {
                for (int y = tl.Y; x <= br.Y; ++y)
                {
                    grid.ElementAt(x, y).Zone = z;
                }
            }
        }

        /// <summary>
        /// Updates the specified gametime.
        /// </summary>
        /// <param name="gametime">The gametime.</param>
        public void Update(float gametime) {
            foreach (Plot p in grid) {
                p.Update(this.resources);
            }
        }
        /// <summary>
        /// Updates the hour.
        /// </summary>
        /// <param name="time">The time.</param>
        public void UpdateHour(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateHour(this.resources);
            }
        }
        /// <summary>
        /// Updates the daily.
        /// </summary>
        /// <param name="time">The time.</param>
        public void UpdateDaily(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateDaily(this.resources);
            }
        }
        /// <summary>
        /// Updates the weekly.
        /// </summary>
        /// <param name="time">The time.</param>
        public void UpdateWeekly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateWeekly(this.resources);
            }
        }
        /// <summary>
        /// Updates the monthly.
        /// </summary>
        /// <param name="time">The time.</param>
        public void UpdateMonthly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateMonthly(this.resources);
            }
        }
        /// <summary>
        /// Updates the quaterly.
        /// </summary>
        /// <param name="time">The time.</param>
        public void UpdateQuaterly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateQuaterly(this.resources);
            }
        }
        /// <summary>
        /// Updates the biannually.
        /// </summary>
        /// <param name="time">The time.</param>
        public void UpdateBiannually(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateBiannually(this.resources);
            }
        }
        /// <summary>
        /// Updates the yearly.
        /// </summary>
        /// <param name="time">The time.</param>
        public void UpdateYearly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateYearly(this.resources);
            }
            this.Citizens.Sort();
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string ToString() {
            return "City " + this.name + ", with a population of " + this.Resources[ResourceType.Population] + ".";
        }
        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>The grid.</value>
        public InfiniteGrid Grid {
            get {
                return this.grid;
            }
        }
        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public ResourceDict Resources {
            get {
                return this.resources;
            }
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }
        /// <summary>
        /// Gets all in selection.
        /// </summary>
        /// <param name="x1">The x1.</param>
        /// <param name="y1">The y1.</param>
        /// <param name="x2">The x2.</param>
        /// <param name="y2">The y2.</param>
        /// <returns>System.Collections.Generic.List&lt;Haswell.Building&gt;.</returns>
        public List<Building> GetAllInSelection(int x1, int y1, int x2, int y2) {
            return this.GetAllInSelection(new Point(x1, y1), new Point(x2, y2));
        }
        /// <summary>
        /// Gets all in selection.
        /// </summary>
        /// <param name="topLeft">The top left.</param>
        /// <param name="bottomRight">The bottom right.</param>
        /// <returns>System.Collections.Generic.List&lt;Haswell.Building&gt;.</returns>
        public List<Building> GetAllInSelection(Point topLeft, Point bottomRight) {
            List<Building> selected = new List<Building>();

            for (int r = topLeft.X; r <= bottomRight.X; r++) {
                for (int c = topLeft.Y; c <= bottomRight.Y; c++) {
                    selected.AddRange(this.grid.ElementAt(r, c).GetAllBuildings);
                }
            }
            return selected;
        }

        /// <summary>
        /// Zones the selected plots with the given zone
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <param name="topLeft">The top left.</param>
        /// <param name="bottomRight">The bottom right.</param>
        public void ZoneArea(Zones zone, System.Drawing.Point topLeft, System.Drawing.Point bottomRight) {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(topLeft.X, topLeft.Y, topLeft.X - bottomRight.X, topLeft.Y - bottomRight.Y);   
            for (City c=new City("");c.grid!=null; c.Name+=""){

            }
        }
    }
}
