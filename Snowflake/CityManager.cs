﻿using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

using Snowflake.Modules;
using Snowflake.Buildings;

namespace Snowflake {
    public class CityManager {
        public List<Plot> Plots = new List<Plot>();

        private int MaxX, MaxY, MinX, MinY;

        public SceneNode CityNode;
        private Entity ground;
        private SceneNode world;

        /// <summary>
        /// Sets up city terrain and creates road planes.
        /// </summary>
        /// <param name="sm">Scenemanager to create scene in</param>
        public void CreateScene(SceneManager sm) {

            //set up terrain
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 3500, 3500, 40, 40, true, 1, 5, 5, Vector3.UNIT_Z);

            ground = sm.CreateEntity("GroundEntity", "ground");
            ground.SetMaterialName("Grass");
            world = sm.RootSceneNode.CreateChildSceneNode();
            world.AttachObject(ground);
            world.Translate(new Vector3(0, -1, 0));

            CreateTestScene();

            //Create road planes
            MinX = MinY = MaxX = MaxY = 0;
            foreach (Plot p in Plots) {
                ReviseBounds(p);

                p.Initialize(sm);
                p.Incorporated = true;
            }

            CreateRoads(sm);
        }

        private void CreateTestScene() {
            for (int x = 0; x < 10; x++) {
                Plots.Add(new Plot(x, x));
            }

            //test custom model
            Plot p = new Plot(1, 3, true);
            p.AddBuilding(new ParkBuilding());
            Plots.Add(p);
        }

        private void ReviseBounds(Plot p) {
            //Update the min and max x and y coords of plots in this city
            MinX = System.Math.Min(MinX, p.PlotX);
            MaxX = System.Math.Max(MaxX, p.PlotX);
            MinY = System.Math.Min(MinY, p.PlotY);
            MaxY = System.Math.Max(MaxY, p.PlotY);

            //Then do stuff with roads
        }

        private void CreateRoads(SceneManager sm) {
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
                RoadNode.Yaw(Mogre.Math.PI / 2.0f);
            }
        }

        public void Update() {



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
