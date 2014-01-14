using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class City {
        //Getting an execuateble directly from someone is less sketchy-
        private int population { get; set; }
        private string name { get; set; }
        //private Plot[,] cityPlots;
        private List<Plot> cityPlots;
        private List<Pipe> pipes;

        [Obsolete("Don't pass a size")]
        public City(string name, int xSize, int ySize) {
            this.population = 0;
            this.name = name;
            this.cityPlots = new List<Plot>();

            this.pipes = new List<Pipe>();
        }
        /// <summary>
        /// Creates an infinite sized city, with the given name
        /// </summary>
        /// <param name="name">Name of the City to use</param>
        public City(string name) {
            this.name = name;
        }
        private static void createGrid(Plot[,] p) {
            for (int r = 0; r < p.GetLength(0); r++) {
                for (int c = 0; c < p.GetLength(1); c++) {
                    p[r, c] = new Plot(r, c);
                }
            }
        }

        //public override string ToString() {
        //    //string sb = this.name;
        //    string sb = " is size (" + this.cityPlots.GetLength(0) + "," + this.cityPlots.GetLength(1) + ")" +
        //        ", and has a population of " + this.population + ".";
        //    return sb;
        //}
    }
}
