using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Haswell;

namespace TEST
{
    class Program
    {
        static List<Person> citizens = new List<Person>();
        static void Main(string[] args)
        {
            Haswell.Controller.init("Help");

            Haswell.Controller.City.SetZoning(new Point(-10, -10), new Point(10, 10), Zones.Residential);
            Haswell.Controller.City.CreateBuilding(0, 0, new BuildingConfiguration(BuildingType.Residential, 1));
            Haswell.Controller.Update(1f);

            List<Haswell.Building> buildings = Haswell.Controller.City.GetAllInSelection(-10, -10, 10, 10);
            Haswell.BuildingLoad.saveResidential((Haswell.Buildings.Residential)buildings[0]);
            //Haswell.BuildingLoad.loadResidential();
            Haswell.Controller.City.CreateBuilding(1, 1, new BuildingConfiguration(BuildingType.Residential, 1));

            Console.WriteLine("All Done");
            Console.ReadKey();
        }
    }
}
