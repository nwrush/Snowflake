using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;

namespace Haswell
{
    [Serializable]
    public class City : ISerializable
    {

        private string name;
        private InfiniteGrid grid;
        private Weather weather;

        private List<Links> pipes;
        private List<Person> Citizens;

        private ResourceDict resources;

        public event EventHandler<BuildingEventArgs> BuildingCreated;

        public Weather Weather { get { return weather; } }

        /// <summary>
        /// Creates a city and initialize's it with the given sides
        /// </summary>
        /// <param name="name">Name of the city</param>
        public City(string name)
        {
            //System.Media.SoundPlayer sp = new System.Media.SoundPlayer("../Media/a.wav");
            //System.Diagnostics.Debug.WriteLine("I REGRET NOTHING");
            //sp.PlayLooping();
            this.name = name;
            this.grid = new InfiniteGrid();

            this.pipes = new List<Links>();
            this.Citizens = new List<Person>();
            this.weather = new Weather();

            initResources();
        }
        /// <summary>
        /// Set the default resource value
        /// </summary>
        private void initResources()
        {
            this.resources = new ResourceDict();
            this.resources[ResourceType.Energy] = 10000;
            this.resources[ResourceType.Material] = 10000;
            this.resources[ResourceType.Money] = 10000;
            this.resources[ResourceType.Population] = 10000;
        }

        void Environment_Daily(object sender, TimeEventArgs e) { this.UpdateDaily(e.Time); }
        void Environment_Weekly(object sender, TimeEventArgs e) { this.UpdateWeekly(e.Time); }
        void Environment_Monthly(object sender, TimeEventArgs e) { this.UpdateMonthly(e.Time); }
        void Environment_Quarterly(object sender, TimeEventArgs e) { this.UpdateQuarterly(e.Time); }
        void Environment_Biannually(object sender, TimeEventArgs e) { this.UpdateBiannually(e.Time); }
        void Environment_Yearly(object sender, TimeEventArgs e) { this.UpdateYearly(e.Time); }

        /// <summary>
        /// Called by Snowflake when the user requests the creation of a building
        /// Throws an error if something goes wrong.
        /// </summary>
        /// <typeparam name="T">The Type of Building to create</typeparam>
        /// <param name="x">The plot X of the building</param>
        /// <param name="y">The plot Y of the building</param>
        private void CreateBuilding(int x, int y, Building b)
        {
            if (grid.ElementAt(x, y).AddBuilding(b))
            { //Zone check already happens in AddBuilding
                if (BuildingCreated != null)
                    BuildingCreated.Invoke(this, new BuildingEventArgs(b));
                return;
            }

            if (grid.ElementAt(x, y).Building != null)
            {
                throw new Exceptions.BuildingCreationFailedException("Unable to place building - there's already something there!");
            }
            else if (grid.ElementAt(x, y).Zone != b.Zone)
            {
                throw new Exceptions.BuildingCreationFailedException("Cannot create this type of building in " + grid.ElementAt(x, y).Zone.ToString() + " plot!");
            }
            else
            {
                throw new Exceptions.BuildingCreationFailedException("Building creation failed");
            }
        }
        public void CreateBuilding(int x, int y, BuildingConfiguration configuration)
        {
            switch (configuration.BuildingType)
            {
                case BuildingType.Residential:
                    CreateBuilding(x, y, new Buildings.Residential(configuration));
                    break;
                case BuildingType.Commercial:
                    CreateBuilding(x, y, new Buildings.Commercial(configuration));
                    break;
                case BuildingType.Industrial:
                    CreateBuilding(x, y, new Buildings.Industrial(configuration));
                    break;
                case BuildingType.Infrastructure:
                    CreateBuilding(x, y, new Buildings.Industrial(configuration));
                    break;
            }
        }

        /// <summary>
        /// Called by Snowflake when the user requests the creation of a road
        /// </summary>
        /// <param name="x">The plot X of the road</param>
        /// <param name="y">The plot Y of the road</param>
        public void CreateRoad(int x, int y)
        {
            Road r = new Road();

            grid.ElementAt(x, y).Zone = r.Zone;

            if (grid.ElementAt(x, y).Building != null)
            {
                return;
            }
            if (grid.ElementAt(x, y).AddBuilding(r))
            {
                BuildingCreated.Invoke(this, new BuildingEventArgs(r));
                return;
            }
        }

        /// <summary>
        /// Deletes the building.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void DeleteBuilding(int x, int y)
        {
            grid.ElementAt(x, y).Delete();
        }

        public void SetZoning(Point p1, Point p2, Zones z)
        {
            Point tl = new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            Point br = new Point(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));
            for (int x = tl.X; x <= br.X; ++x)
            {
                for (int y = tl.Y; y <= br.Y; ++y)
                {
                    grid.ElementAt(x, y).Zone = z;
                }
            }
        }

        /// <summary>
        /// Updates the specified gametime.
        /// </summary>
        /// <param name="gametime">The gametime.</param>
        public void Update(float gametime)
        {
            foreach (Plot p in grid)
            {
                p.Update(this.resources);
            }
            weather.Update();
        }

        public delegate void UpdateDelegate(DateTime time);
        public void UpdateDaily(DateTime time)
        {
            foreach (Plot p in this.grid)
            {
                p.UpdateDaily();
            }
        }
        public void UpdateWeekly(DateTime time)
        {
            foreach (Plot p in this.grid)
            {
                p.UpdateWeekly();
            }
        }
        public void UpdateMonthly(DateTime time)
        {
            foreach (Plot p in this.grid)
            {
                p.UpdateMonthly();
            }
        }
        public void UpdateQuarterly(DateTime time)
        {
            foreach (Plot p in this.grid)
            {
                p.UpdateQuarterly();
            }
        }
        public void UpdateBiannually(DateTime time)
        {
            foreach (Plot p in this.grid)
            {
                p.UpdateBiannually();
            }
        }
        public void UpdateYearly(DateTime time)
        {
            foreach (Plot p in this.grid)
            {
                p.UpdateYearly();
            }
            this.Citizens.Sort();
        }

        public override string ToString()
        {
            return "City " + this.name + ", with a population of " + this.Resources[ResourceType.Population] + ".";
        }
        /// <summary>
        /// Gets the grid.
        /// </summary>
        /// <value>The grid.</value>
        public InfiniteGrid Grid
        {
            get
            {
                return this.grid;
            }
        }
        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <value>The resources.</value>
        public ResourceDict Resources
        {
            get
            {
                return this.resources;
            }
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
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
        public List<Building> GetAllInSelection(int x1, int y1, int x2, int y2)
        {
            return this.GetAllInSelection(new Point(x1, y1), new Point(x2, y2));
        }
        /// <summary>
        /// Gets all in selection.
        /// </summary>
        /// <param name="topLeft">The top left.</param>
        /// <param name="bottomRight">The bottom right.</param>
        /// <returns>System.Collections.Generic.List&lt;Haswell.Building&gt;.</returns>
        public List<Building> GetAllInSelection(Point topLeft, Point bottomRight)
        {
            List<Building> selected = new List<Building>();

            for (int r = topLeft.X; r <= bottomRight.X; r++)
            {
                for (int c = topLeft.Y; c <= bottomRight.Y; c++)
                {
                    if (this.grid.ElementAt(r, c).Building != null)
                    {
                        selected.Add(this.grid.ElementAt(r, c).Building);
                    }
                }
            }
            return selected;
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("City Name", name);

            info.AddValue("Resource-Money", Resources[ResourceType.Money]);
            info.AddValue("Resource-Pop", Resources[ResourceType.Population]);
            info.AddValue("Resource-Energy", Resources[ResourceType.Energy]);
            info.AddValue("Resource-Material", Resources[ResourceType.Material]);

            info.AddValue("Grid", this.grid);
        }
    }
}
