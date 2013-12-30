using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

using Snowflake.Modules;

namespace Snowflake {
    public class CityManager {
        public static List<Plot> Plots = new List<Plot>();

        private static int MaxX, MaxY, MinX, MinY;

        public static SceneNode CityNode;

        public static void CreateScene(SceneManager sm) {
            //Set up city bounds and create road planes.

            MinX = MinY = MaxX = MaxY = 0;
            foreach (Plot p in Plots) {
                ReviseBounds(p);

                p.Initialize(sm);
                p.Incorporated = true;
            }

            CreateRoads(sm);
        }

        private static void ReviseBounds(Plot p) {
            //Update the min and max x and y coords of plots in this city
            MinX = System.Math.Min(MinX, p.PlotX);
            MaxX = System.Math.Max(MaxX, p.PlotX);
            MinY = System.Math.Min(MinY, p.PlotY);
            MaxY = System.Math.Max(MaxY, p.PlotY);

            //Then do stuff with roads
        }

        private static void CreateRoads(SceneManager sm) {
            for (int x = MinX - 2; x < MaxX + 1; x++) {
                Plane plane = new Plane(Vector3.UNIT_Y, 0);
                MeshManager.Singleton.CreatePlane("road_x" + x.ToString(), ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, Plot.RoadSize, (MaxX - MinX + 2) * Plot.Height, 4, 40, true, 1, 1, 5, Vector3.UNIT_Z);

                Entity road = sm.CreateEntity("RoadEnt_X" + x.ToString(), "road_x" + x.ToString());
                road.SetMaterialName("Road");

                SceneNode RoadNode = sm.RootSceneNode.CreateChildSceneNode();
                RoadNode.AttachObject(road);
                RoadNode.Translate(new Vector3(3 * Plot.Width / 2 + x * Plot.Width, 0, Plot.Height / 2 + (MaxY - MinY) * 0.5f * Plot.Height));
            }
            for (int y = MinY - 2; y < MaxY + 1; y++) {
                Plane plane = new Plane(Vector3.UNIT_Y, 0);
                MeshManager.Singleton.CreatePlane("road_y" + y.ToString(), ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, Plot.RoadSize, (MaxX - MinX + 2) * Plot.Height, 4, 40, true, 1, 1, 5, Vector3.UNIT_Z);

                Entity road = sm.CreateEntity("RoadEnt_Y" + y.ToString(), "road_y" + y.ToString());
                road.SetMaterialName("Road");

                SceneNode RoadNode = sm.RootSceneNode.CreateChildSceneNode();
                RoadNode.AttachObject(road);
                RoadNode.Translate(new Vector3(Plot.Width / 2 + (MaxX - MinX) * 0.5f * Plot.Width, 0, 3 * Plot.Height / 2 + y * Plot.Height));
                RoadNode.Yaw(90);
            }
        }

        public static void Update() {
            //Each tick, check if each plot is incorporated into the city. If so, update
            //Otherwise, incorporate it and revise the city bounds.

            foreach (Plot p in Plots) {
                if (p.Incorporated) { p.Update(); } else {
                    ReviseBounds(p);
                    p.Incorporated = true;
                }
            }
        }
    }
}
