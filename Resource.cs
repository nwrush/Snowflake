using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake {
    public class Resource {

        public float Volume; //how much of the resource there is. Tons for coal, liters or whatever for NG, lumens or something for solar, etc.
        public ResourceType Type;

        //Use some of the resource.
        public void Deplete(float amount) {
            Volume -= amount;
        }

        //Get some more resource from somewhere
        public void Replentish(float amount) {
            Volume += amount;
        }
    }

    //What kind of resource. Amount of resource on a plot is calculated by summing up the sums of all available resources on the plot. Might have 3 deposits of 6000 tons of coal or whatever.
    //Resources can be partially mined by using Deplete
    //For renewable resources, Deplete and Replentish represent variable volume of increase.
    public enum ResourceType {
        Coal,
        Oil,
        NaturalGas,
        Water,
        Sunlight,
        UndergroundHeat,
        Metal,
        SmallWoodlandAnimals,
        Electricity
    }
}
