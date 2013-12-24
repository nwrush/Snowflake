using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Buildings {
    public class PowerBuilding : Building, CommercialBuilding {

        public float PowerOutputVolts; //Different types output different voltages, and this can be changed depending on how far the power needs to go
        public float PowerOutputKWH; //Standard measure of power output, not all may be used

        public float Pollution; //Temp, figure out a better way to represent pollution. Maybe a Pollution class with different types and descriptions?

        private float _consumption; //Resource consumption this tick

        public override void ConsumeResources() {
            foreach (Resource r in this.Plot.Resources) {
                //if (r.Type 

                //whatever look for coal or something
                //consume it and 
            }
        }
    }

    //Enum for determining the values associated with this class. 
    public enum PowerType {
        Coal,
        NaturalGas,
        Oil,
        Nuclear,
        Hydro,
        SolarThermal,
        SolarPV,
        Wind,
        Geothermal
    }
}
