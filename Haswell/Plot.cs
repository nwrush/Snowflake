﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haswell.Exceptions;

namespace Haswell {
    class Plot {
        public enum Zone {
            residential,
            commercial,
            industrial
        };

        private Dictionary<Resource, ResourceVal> resource;
        private List<Building> buildings;

        ////These values used until we move to dynamic plots
        //public const int Width = 120;
        //public const int Height = 120;
        //public const int RoadSize = 15;

        //Location of the plot in the city grid
        //Minimum city plot value is (0,0)
        int plotX, plotY;

        public Plot(int x, int y) {
            this.resource = new Dictionary<Resource,ResourceVal>();
            this.buildings = new List<Building>();
            if (x < 0 || y < 0) {
                throw new PlotOutOfRangeException("The position of the plot was negative.");
            } else {
                this.plotX = x;
                this.plotY = y;
            }
        }
    }
}
