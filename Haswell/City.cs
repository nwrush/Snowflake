﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class City {
        //Getting an execuateble directly from someone is less sketchy-
        private int population { get; set; }
        private string name { get; set; }
        private InfiniteGrid grid;
        private List<Pipe> pipes;
        private List<Zone> zones;

        public event EventHandler BuildingCreated;
        public event EventHandler BuildingUpdated;

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
        }

        private static void createGrid(Plot[,] p) {
            for (int r = 0; r < p.GetLength(0); r++) {
                for (int c = 0; c < p.GetLength(1); c++) {
                    p[r, c] = new Plot(r, c);
                }
            }
        }

        /// <summary>
        /// Called by Snowflake when the user requests the creation of a building
        /// Throws an exception error if Building creation fails (e.g. Plot is already occupied)
        /// </summary>
        /// <param name="x">The plot X of the building</param>
        /// <param name="y">The plot Y of the building</param>
        public void CreateBuilding(int x, int y) {
            //if (grid.ElementAt(x, y).AddBuilding(new Building()))

            throw new NotImplementedException();
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

        public void Update(long gametime) {
            throw new NotImplementedException();
        }

        public override string ToString() {
            return "City " + this.name + ", with a population of " + this.population + ".";
        }
        public InfiniteGrid Grid {
            get {
                return this.grid;
            }
        }
    }
}
