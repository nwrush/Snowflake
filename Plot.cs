using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake {
    public class Plot {
        //Plots contain Buildings and Resources. Usually, a plot will only have one Building.
        public List<Building> Buildings;
        public List<Resource> Resources;

        public int PlotX;
        public int PlotY;

        public void AddBuilding(Building b) {
            this.Buildings.Add(b);
            b.Plot = this;
        }
    }
}
