using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Miyagi.Common.Data;

using Snowflake.Modules;
using Snowflake.GuiComponents;
using Snowflake.States;

using Haswell;
using Haswell.Buildings;
using Haswell.Exceptions;

using Vector3 = Mogre.Vector3;
using Rectangle = Miyagi.Common.Data.Rectangle;

namespace Snowflake {
    public static partial class CityManager {

        /// <summary>
        /// Initializes the City, setting its origin point and creating the relevant Haswell objects.
        /// </summary>
        /// <param name="originx">X position of city origin</param>
        /// <param name="originy">Y Position of city origin</param>
        public static void Init(int originx, int originy) {
            if (!Initialized) {
                GameConsole.ActiveInstance.WriteLine("Founding new City at " + originx.ToString() + ", " + originy.ToString());

                Origin = new Point(originx, originy);
                Haswell.Controller.init(cityName ?? "New City");
                Haswell.Controller.City.BuildingCreated += CreateBuilding;
                Initialized = true;
            }
            else {
                GameConsole.ActiveInstance.WriteError("Attempting to found city in an already initialized area!");
            }
        }
        /// <summary>
        /// Initializes the City, setting its origin point and creating the relevant Haswell objects.
        /// </summary>
        /// <param name="p">Origin point of the city</param>
        public static void Init(Point p) { Init(p.X, p.Y); }

        /// <summary>
        /// Add a new building to the city at the specified coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void NewBuilding<T>(int x, int y) where T : Building, new() {
            if (Initialized) {
                try { Haswell.Controller.City.CreateBuilding<T>(x, y); }
                catch (BuildingCreationFailedException e) {
                    GameConsole.ActiveInstance.WriteLine(e.Message);
                }
            }
            else {
                GameConsole.ActiveInstance.WriteError("Unable to create building, no city initialized!");
            }
        }

        /// <summary>
        /// Set the Zoning regulation for a plot at the given coordinates
        /// </summary>
        /// <param name="p">Point to set the zoning of</param>
        /// <param name="z">The type of zone to set</param>
        public static bool SetZoning(Point p, Haswell.Zones z) { return SetZoning(p, p, z); }
        public static bool SetZoning(Rectangle r, Haswell.Zones z ) { return SetZoning(r.Location, new Point(r.Right, r.Bottom), z); }
        /// <summary>
        /// Set the Zoning regulation for a given area defined by the points p1 and p2 at the corners of the area.
        /// </summary>
        /// <param name="p1">One corner of the area</param>
        /// <param name="p2">The corner opposite p2</param>
        /// <param name="z">The type of zone to set</param>
        public static bool SetZoning(Point p1, Point p2, Haswell.Zones z)
        {
            if (Initialized)
            {
                Haswell.Controller.City.SetZoning(new System.Drawing.Point(p1.X, p1.Y), new System.Drawing.Point(p2.X, p2.Y), z);
                return true;
            }
            else
            {
                GameConsole.ActiveInstance.WriteError("Unable to set zoning, no city initialized!");
            }
            return false;
        }

        public static void DeleteBuilding(int x, int y)
        {
            Haswell.Controller.City.DeleteBuilding(x, y);
        }

        private static void UpdateHaswell(float frametime) {
            try {
                Haswell.Controller.Update(frametime);
            }
            catch (NotImplementedException e) {
                DebugPanel.ActiveInstance[2] = (e.Message);
            }
        }
    }
}
