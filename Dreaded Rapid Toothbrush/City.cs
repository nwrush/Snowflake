using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRT {
    class City {
        private int population{get; set;}
        private string name{get; set;}
        private Plot[,] cityPlots;

        public City(string name,int xSize, int ySize) {
            this.population = 0;
            this.name = name;
            this.cityPlots = new Plot[xSize, ySize];
            this.cityPlots=createGrid(this.cityPlots);
        }
        private static Plot[,] createGrid(Plot[,] p){
            

            return p;
        }
    }
}
