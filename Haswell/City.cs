using System;
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

        /// <summary>
        /// Creates a city and initialize's it with the given sides
        /// </summary>
        /// <param name="name">Name of the city</param>
        public City(string name) {
            this.population = 0;
            this.name = name;
            this.grid = new InfiniteGrid();

            this.pipes = new List<Pipe>();
        }

        private static void createGrid(Plot[,] p) {
            for (int r = 0; r < p.GetLength(0); r++) {
                for (int c = 0; c < p.GetLength(1); c++) {
                    p[r, c] = new Plot(r, c);
                }
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
    }
}
