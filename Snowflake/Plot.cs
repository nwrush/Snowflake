using System;
using System.Collections.Generic;
using System.Text;

using Snowflake.Buildings;
using Snowflake.Modules;

using Mogre;

namespace Snowflake {
    public class Plot {
        //Universal plot size constant - use until we move away from a rectangular grid system.
        public const int Width = 120;
        public const int Height = 120;

        public const int RoadSize = 15;

        //Plots contain Buildings and Resources. Usually, a plot will only have one Building.
        public List<Building> Buildings;
        public List<Resource> Resources;

        //PlotX and Y coordinate (in the CityState grid)
        //Child buildings read this to 
        public int PlotX;
        public int PlotY;

        //Are we part of a city?
        public bool Incorporated = false;

        public Plot(int x, int y, bool flag = false) {
            this.PlotX = x;
            this.PlotY = y;

            Buildings = new List<Building>();
            Resources = new List<Resource>();

            //temp flag for debugging models
            if (!flag) { this.AddBuilding(new PowerBuilding()); }
        }

        public void Initialize(SceneManager sm) {
            foreach (Building b in this.Buildings) {
                b.Initialize(sm);
            }
        }

        public void AddBuilding(Building b) {
            this.Buildings.Add(b);
            b.Plot = this;
        }

        public void Update() {
            foreach (Building b in Buildings) {
                b.Update();
            }
        }
    }
}
