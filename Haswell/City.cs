﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class City {
        //Getting an execuateble directly from someone is less sketchy-

        private string name;
        private InfiniteGrid grid;

        private List<Pipe> pipes;
        private List<Zone> zones;

        private ResourceDict resources;

        public event EventHandler<BuildingEventArgs> BuildingCreated;
        public event EventHandler<BuildingEventArgs> BuildingUpdated;

        /// <summary>
        /// Creates a city and initialize's it with the given sides
        /// </summary>
        /// <param name="name">Name of the city</param>
        public City(string name) {
            System.Media.SoundPlayer sp = new System.Media.SoundPlayer("../Media/a.wav");
            System.Diagnostics.Debug.WriteLine("I REGRET NOTHING");
            sp.PlayLooping();
            this.name = name;
            this.grid = new InfiniteGrid();

            this.pipes = new List<Pipe>();
            this.zones = new List<Zone>();

            initResources();
        }
        /// <summary>
        /// Sets the default Resource values
        /// </summary>
        private void initResources() {
            this.resources = new ResourceDict();
            this.resources[Resource.Type.Energy] = 10000;
            this.resources[Resource.Type.Material] = 10000;
            this.resources[Resource.Type.Money] = 10000;
            this.resources[Resource.Type.Population] = 10000;
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
            return "City " + this.name + ", with a population of " + this.Resources[Resource.Type.Population] + ".";
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
