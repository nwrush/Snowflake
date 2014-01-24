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

namespace Snowflake {
    public class CityManager {

        private GameLoopState GameMgr;

        private int MaxX, MaxY, MinX, MinY;
        private bool initialized = false;

        private SceneNode cityNode;
        private Entity ground;
        private SceneNode world;
        private Point origin;

        private List<Renderable> cityObjects;

        public bool Initialized { get { return initialized; } }

        public CityManager(GameLoopState gameloop) {
            cityObjects = new List<Renderable>();
            this.GameMgr = gameloop;
        }

        /// <summary>
        /// Sets up city terrain and creates road planes.
        /// </summary>
        /// <param name="sm">Scenemanager to create scene in</param>
        public void CreateScene(SceneManager sm) {

            CreateTerrain(sm);
            CreateCity(sm);
        }

        /// <summary>
        /// Set up terrain
        /// </summary>
        private void CreateTerrain(SceneManager sm) {
           
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 10000, 10000, 100, 100, true, 1, 5, 5, Vector3.UNIT_Z);

            ground = sm.CreateEntity("GroundEntity", "ground");
            ground.SetMaterialName("Grass");
            world = sm.RootSceneNode.CreateChildSceneNode();
            world.AttachObject(ground);
            //world.Translate(new Vector3(0, -1, 0));
        }

        private void CreateCity(SceneManager sm) {
            cityNode = sm.RootSceneNode.CreateChildSceneNode("CityNode");

            //CreateRoads(sm);
            CreateObjects(sm);
        }
        
        private void CreateRoads(SceneManager sm) {

            //Todo: initialize roads from Haswell data

            throw new NotImplementedException();
        }

        private void CreateObjects(SceneManager sm) {

            //Todo: initialize objects from Haswell data

            foreach (Renderable r in cityObjects) {
                r.Create(this, sm, cityNode);
            }
        }

        public void Init(int originx, int originy) {
            if (!initialized) {
                GameConsole.ActiveInstance.WriteLine("Founding new City at " + originx.ToString() + ", " + originy.ToString());

                origin = new Point(originx, originy);
                Haswell.Controller.init("shrug");
                Haswell.Controller.City.BuildingCreated += CreateBuilding;
                initialized = true;
            }
            else {
                GameConsole.ActiveInstance.WriteError("Attempting to found city in an already initialized area!");
            }
        }
        public void Init(Point p) { Init(p.X, p.Y); }

        /// <summary>
        /// Add a new building to the city at the specified coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void NewBuilding(int x, int y) {
            if (initialized) {
                try { Haswell.Controller.City.CreateBuilding<Commercial>(x, y); }
                catch (BuildingCreationFailedException e) {
                    GameConsole.ActiveInstance.WriteLine(e.Message);
                }
            }
            else {
                GameConsole.ActiveInstance.WriteError("Unable to create building, no city initialized!");
            }
        }
        public void NewBuilding(Point p) { this.NewBuilding(p.X, p.Y); }

        private void CreateBuilding(object sender, BuildingEventArgs e) {
            GameConsole.ActiveInstance.WriteLine("Added a building at " + e.Building.Parent.X + ", " + e.Building.Parent.Y);
            RenderableBuilding rb = new RenderableBuilding(e.Building);
            rb.Create(this, GameMgr.StateMgr.Engine.SceneMgr, cityNode);
            this.cityObjects.Add(rb);
        }
            
        /// <summary>
        /// Update the city
        /// </summary>
        /// <param name="frametime">Milliseconds since last frame</param>
        public void Update(float frametime) {

            if (initialized && false) {
                try {
                    Haswell.Controller.Update(frametime);
                }
                catch (NotImplementedException e) {
                    DebugPanel.ActiveInstance.SetDebugText(e.Message);
                }

                foreach (Renderable r in cityObjects) {
                    //Check if r needs updating, and if so:
                    r.Update();
                }
            }
        }

        public Point GetPlotCoords(Vector3 src) {
            if (!(origin.X == 0 && origin.Y == 0)) {
                return new Point((int)System.Math.Floor(src.x / Renderable.PlotWidth - origin.X), (int)System.Math.Floor(src.z / Renderable.PlotHeight - origin.Y));
            }
            return new Point((int)System.Math.Floor(src.x / Renderable.PlotWidth), (int)System.Math.Floor(src.z / Renderable.PlotHeight));
        }

        public Vector3 GetPlotCenter(int x, int y) { return this.GetPlotCenter(new Point(x, y)); }
        public Vector3 GetPlotCenter(Point plotCoord) {
            if (!(origin.X == 0 && origin.Y == 0)) {
                //Todo: account for terrain height
                return new Vector3((plotCoord.X + origin.X) * Renderable.PlotWidth + (Renderable.PlotWidth * 0.5f), 
                                    0.0f,
                                    (plotCoord.Y + origin.Y) * Renderable.PlotHeight + (Renderable.PlotHeight * 0.5f));
            }
            else {
                return new Vector3(plotCoord.X * Renderable.PlotWidth + (Renderable.PlotWidth * 0.5f),
                    0.0f,
                    plotCoord.Y * Renderable.PlotHeight + (Renderable.PlotHeight * 0.5f));
            }
        }

        public Point GetOrigin() {
            return this.origin;
        }
    }
}
