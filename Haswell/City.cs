using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class City {

        private string name;
        private InfiniteGrid grid;

        private List<Pipe> pipes;
        private List<Zone> zones;
        private List<Person> Citizens;

        private ResourceDict resources;

        public event EventHandler<BuildingEventArgs> BuildingCreated;
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

            this.pipes = new List<Pipe>();
            this.zones = new List<Zone>();
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


        void Environment_Hourly(object sender, TimeEventArgs e) { this.UpdateHour(e.Time); }
        void Environment_Daily(object sender, TimeEventArgs e) { this.UpdateDaily(e.Time); }
        void Environment_Weekly(object sender, TimeEventArgs e) { this.UpdateWeekly(e.Time); }
        void Environment_Monthly(object sender, TimeEventArgs e) { this.UpdateMonthly(e.Time); }
        void Environment_Quarterly(object sender, TimeEventArgs e) { this.UpdateQuaterly(e.Time); }
        void Environment_Biannually(object sender, TimeEventArgs e) { this.UpdateBiannually(e.Time); }
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

        public void DeleteBuilding(int x, int y) {
            if (grid.ElementAt(x, y).GetAllBuildings.Count > 0) {
                grid.ElementAt(x, y).DeleteAllBuildings();
            }
        }

        /// <summary>
        /// Creates a zone for automatic building creation by the game's AI
        /// </summary>
        /// <param name="x1">Leftmost x position of the zone</param>
        /// <param name="y1">Topmost y position of the zone</param>
        /// <param name="x2">Rightmost x position of the zone (inclusive)</param>
        /// <param name="y2">Bottommost y position of the zone (inclusive)</param>
        /// <returns>The newly created Zone object</returns>
        public Zone CreateZone(int x1, int y1, int x2, int y2,ZoneTypes type) {
            Zone tmp = new Zone(x1, y1, x2, y2, type);
            this.zones.Add(tmp);
            return tmp;
        }

        public void Update(float gametime) {
            foreach (Plot p in grid) {
                p.Update(this.resources);
            }
        }
        public void UpdateHour(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateHour(this.resources);
            }
        }
        public void UpdateDaily(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateDaily(this.resources);
            }
        }
        public void UpdateWeekly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateWeekly(this.resources);
            }
        }
        public void UpdateMonthly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateMonthly(this.resources);
            }
        }
        public void UpdateQuaterly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateQuaterly(this.resources);
            }
        }
        public void UpdateBiannually(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateBiannually(this.resources);
            }
        }
        public void UpdateYearly(DateTime time) {
            foreach (Plot p in this.grid) {
                p.UpdateYearly(this.resources);
            }
            this.Citizens.Sort();
        }

        public override string ToString() {
            return "City " + this.name + ", with a population of " + this.Resources[ResourceType.Population] + ".";
        }
        public InfiniteGrid Grid {
            get {
                return this.grid;
            }
        }
        public ResourceDict Resources {
            get {
                return this.resources;
            }
        }
        public string Name {
            get {
                return this.name;
            }
            set {
                this.name = value;
            }
        }
        public List<Building> GetAllInSelection(int x1, int y1, int x2, int y2) {
            return this.GetAllInSelection(new System.Drawing.Point(x1, y1), new System.Drawing.Point(x2, y2));
        }
        public List<Building> GetAllInSelection(System.Drawing.Point topLeft, System.Drawing.Point bottomRight) {
            List<Building> selected = new List<Building>();

            for (int r = topLeft.X; r <= bottomRight.X; r++) {
                for (int c = topLeft.Y; c <= bottomRight.Y; c++) {
                    selected.AddRange(this.grid.ElementAt(r, c).GetAllBuildings);
                }
            }
            return selected;
        }
    }
}
