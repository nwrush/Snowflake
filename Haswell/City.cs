using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class City {
        //Getting an execuateble directly from someone is less sketchy-

        //Todo: Make this a resource
        private int population { get; set; }
        private string name { public get; public set; }
        private InfiniteGrid grid;

        private List<Pipe> pipes;
        private List<Zone> zones;
        private Dictionary<Resource.Type, int> resources;

        public event EventHandler<BuildingEventArgs> BuildingCreated;
        public event EventHandler<BuildingEventArgs> BuildingUpdated;

        /// <summary>
        /// Creates a city and initialize's it with the given sides
        /// </summary>
        /// <param name="name">Name of the city</param>
        public City(string name) {
            this.population = 0;
            this.name = name;
            this.grid = new InfiniteGrid();

            this.pipes = new List<Pipe>();
            this.zones = new List<Zone>();
            this.resources = new Dictionary<Resource.Type, int> 
                {
                    //Default Values for a new city
                    {Resource.Type.Energy,100},
                    {Resource.Type.Material,100},
                    {Resource.Type.Money,100},
                    {Resource.Type.Population,100}
                };
        }

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
        /// Creates a zone for automatic building creation by the game's AI
        /// </summary>
        /// <param name="x1">Leftmost x position of the zone</param>
        /// <param name="y1">Topmost y position of the zone</param>
        /// <param name="x2">Rightmost x position of the zone (inclusive)</param>
        /// <param name="y2">Bottommost y position of the zone (inclusive)</param>
        /// <returns>The newly created Zone object</returns>
        public Zone CreateZone(int x1, int y1, int x2, int y2) {
            this.zones.Add(new Zone(x1, y1, x2, y2));

            throw new NotImplementedException();
        }

        public void Update(float gametime) {
            foreach (Plot p in grid) {
                p.Update(this.resources);
            }
        }

        public override string ToString() {
            return "City " + this.name + ", with a population of " + this.population + ".";
        }
        public InfiniteGrid Grid {
            get {
                return this.grid;
            }
        }
        public Dictionary<Resource.Type, int> Resources {
            get {
                return this.resources;
            }
        }
    }
}
