using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ark {
    public class City {
        private int population{get; set;}
        private string name{get; set;}
        private Plot[,] cityPlots;

        public City(string name,int xSize, int ySize) {
            this.population = 0;
            this.name = name;
            this.cityPlots = new Plot[xSize, ySize];
            createGrid(this.cityPlots);
        }
        private static void createGrid(Plot[,] p){
            for (int r = 0; r < p.GetLength(0); r++) {
                for (int c = 0; c < p.GetLength(1); c++) {
                    p[r, c] = new Plot(r, c);
                }
            }
        }

        public override string ToString() {
            //string sb = this.name;
            string sb=" is size ("+this.cityPlots.GetLength(0)+","+this.cityPlots.GetLength(1)+")"+
                ", and has a population of "+this.population+".";
            return sb;
        }
    }
}
