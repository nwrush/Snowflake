﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class City {
        //Getting an execuateble directly from someone is less sketchy-
        private int population { get; set; }
        private string name { get; set; }
        private List<Plot> plotList;
        private List<Pipe> pipes;

        /// <summary>
        /// Creates a city and initialize's it with the given sides
        /// </summary>
        /// <param name="name">Name of the city</param>
        /// <param name="xSize">X-length of the city</param>
        /// <param name="ySize">Y-length of the city</param>
        public City(string name, int xSize, int ySize) {
            this.population = 0;
            this.name = name;

            this.plotList = new List<Plot>();


            this.pipes = new List<Pipe>();
        }
        /// <summary>
        /// Creates an infinite sized city, with the given name
        /// </summary>
        /// <param name="name">Name of the City to use</param>
        public City(string name):this(name,100,100) {
        }

        protected void init(int xSize, int ySize) {
            int totalPlots = xSize * ySize;
            Plot[,] tmp=new Plot[xSize,ySize];
            //for (int r=0; r<tmp.GetLength(0))
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
    }
}
