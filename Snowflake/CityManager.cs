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

        private static SceneNode cityNode;
        private static Entity ground;
        private static SceneNode world;
        public static Point Origin { get; private set; }
        private static string cityName;

        private static List<Renderable> cityObjects;

        public static Point selectionStart { get; private set; }
        public static Point selectionEnd { get; private set; }
        private static Point selectionOrigin;

        public static bool Initialized { get; private set; }

        public static Rectangle SelectionBox { 
            get { 
                return new Rectangle(selectionStart, 
                        new Size((selectionEnd.X > selectionStart.X ? selectionEnd.X - selectionStart.X : 0), 
                                 (selectionEnd.Y > selectionStart.Y ? selectionEnd.Y - selectionStart.Y : 0))); 
            } 
        }
        private static List<Building> selectedBuildings;

        public static StateManager StateMgr { get { return GameMgr.StateMgr; } }
        public static SceneManager SceneMgr { get { return GameMgr.StateMgr.Engine.SceneMgr; } }
        public static OgreManager Engine { get { return GameMgr.StateMgr.Engine; } }
        public static GameLoopState GameMgr { get; private set; }
        public static GuiManager GuiMgr { get { return GameMgr.GuiMgr; } }
        public static WeatherManager WeatherMgr { get { return GameMgr.WeatherMgr; } }

        //////TIME
        public static float Time {
            get {
                return Controller.Environment.Time;
            }
        }
        public static float Timescale {
            get {
                return Controller.Environment.Timescale;
            }
        }
        private static float totalTime;
        private static float lastTotalTime;
        public static float MinuteLength { get { return Universe.MinuteLength; } }
        public static float HourLength { get { return Universe.HourLength; } }
        public static float DayLength { get { return Universe.DayLength; } }

        /// <summary>
        /// Sets the amount by which the game time is incremented every tick
        /// </summary>
        /// <param name="timescale"></param>
        public static void SetTimescale(float timescale) {
            Controller.Environment.SetTimescale(timescale);
        }

        /// <summary>
        /// Sets the active city's name
        /// </summary>
        /// <param name="name">Name of the city</param>
        public static void SetCityName(string name) {
            cityName = name;
            if (Initialized) {
                Controller.City.Name = name;
            }
        }
        public static string CityName { get { return Controller.City.Name; } }
        /// <summary>
        /// Gets the active city from Haswell
        /// </summary>
        public static City ActiveCity {
            get {
                return Controller.City;
            }
        }

        static CityManager() {
            cityObjects = new List<Renderable>();
        }

        /// <summary>
        /// Sets up city terrain and creates road planes.
        /// </summary>
        /// <param name="sm">Scenemanager to create scene in</param>
        public static void CreateScene(SceneManager sm) {

            CreateTerrain(sm);
            CreateCity(sm);
        }

        /// <summary>
        /// Sets the current active game state.
        /// </summary>
        /// <param name="gameState">GameLoopState to reference for game functions</param>
        public static void SetGameMgr(GameLoopState gameState) {
            GameMgr = gameState;
        }

        /// <summary>
        /// Set up terrain
        /// </summary>
        private static void CreateTerrain(SceneManager sm) {
           
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 10000, 10000, 100, 100, true, 1, 5, 5, Vector3.UNIT_Z);

            ground = sm.CreateEntity("GroundEntity", "ground");
            ground.SetMaterialName("Grass");
            world = sm.RootSceneNode.CreateChildSceneNode();
            world.AttachObject(ground);
            //world.Translate(new Vector3(0, -1, 0));
        }

        private static void CreateCity(SceneManager sm) {
            cityNode = sm.RootSceneNode.CreateChildSceneNode("CityNode");

            //CreateRoads(sm);
            CreateObjects(sm);
        }
        
        private static void CreateRoads(SceneManager sm) {

            //Todo: initialize roads from Haswell data

            throw new NotImplementedException();
        }

        private static void CreateObjects(SceneManager sm) {

            //Todo: initialize objects from Haswell data

            foreach (Renderable r in cityObjects) {
                r.Create(sm, cityNode);
            }
        }

        private static void CreateBuilding(object sender, BuildingEventArgs e) {
            GameConsole.ActiveInstance.WriteLine("Added a building at " + e.Building.Parent.X + ", " + e.Building.Parent.Y);
            RenderableBuilding rb = new RenderableBuilding(e.Building);
            rb.Create( GameMgr.StateMgr.Engine.SceneMgr, cityNode);
            cityObjects.Add(rb);
            rb.Deleted += (object sender2, EventArgs e2) =>
            {
                cityObjects.Remove(rb);
            };
        }

        public static void CreateBuildingOnCursor() {
            RenderableBuilding rb = new RenderableBuilding(new Residential());
            rb.Create(GameMgr.StateMgr.Engine.SceneMgr, cityNode);
            rb.IsVirtual = true;
            GameMgr.SetCursorBuilding(rb);
            GameMgr.SetMouseMode(MouseMode.PlacingBuilding);
            GuiMgr.ShowBuildingPlacementPanel();
        }

        public static void BeginZoning()
        {
            GameMgr.SetMouseMode(MouseMode.DrawingZone);
        }
            
        /// <summary>
        /// Update the city
        /// </summary>
        /// <param name="frametime">Milliseconds since last frame</param>
        public static void Update(float frametime) {
            totalTime += frametime;
            if (Initialized) {
                
                if ((totalTime - lastTotalTime) / 1000.0 >= 1 / 30.0) {
                    UpdateHaswell((totalTime - lastTotalTime));
                    lastTotalTime = totalTime;
                }

                foreach (Renderable r in cityObjects) {
                    //Check if r needs updating, and if so:
                    try {
                        r.Update();
                    } catch (NotImplementedException e) {
                        DebugPanel.ActiveInstance[2] = e.Message;
                    }
                }
            }
        }

        public static void SetSelectionOrigin(Point p) {
            selectionStart = selectionEnd = selectionOrigin = p;
        }

        /// <summary>
        /// Set the selection box,
        /// ensuring that the selection start represents the minimum bounds of the box
        /// and the selection end represents the maximum bounds of box
        /// </summary>
        public static void UpdateSelectionBox(Point p) {
            int minx, miny, maxx, maxy;
            minx = Utils3D.Min(selectionOrigin.X, p.X);
            miny = Utils3D.Min(selectionOrigin.Y, p.Y);
            maxx = Utils3D.Max(selectionOrigin.X, p.X);
            maxy = Utils3D.Max(selectionOrigin.Y, p.Y);
            selectionStart = new Point(minx, miny);
            selectionEnd = new Point(maxx, maxy);
        }

        /// <summary>
        /// Takes the active selection and performs the selection action on all the things contained within.
        /// </summary>
        public static bool MakeSelection() {
            //Todo: make selection
            if (SelectionBox.Left != Int32.MaxValue - 1 && SelectionBox.Top != Int32.MaxValue - 1) {

                if (selectedBuildings != null) { selectedBuildings.Clear(); }
                selectedBuildings = Haswell.Controller.City.GetAllInSelection(selectionStart.X, selectionStart.Y, selectionEnd.X, selectionEnd.Y);
                foreach (Renderable r in cityObjects) {
                    if (r is RenderableBuilding) {
                        if (selectedBuildings.Contains(((RenderableBuilding)r).GetData())) {
                            r.Select();
                        }
                        else {
                            r.Deselect();
                        }
                    }
                }
                //GameConsole.ActiveInstance.WriteLine("Selecting region " + SelectionBox.ToString());
                return true;
                
            }
            return false;
        }

        public static void SetMouseMode(MouseMode m) {
            GameMgr.SetMouseMode(m);
        }

        /// <summary>
        /// Clears the selection
        /// </summary>
        public static void ClearSelection() {
            selectionStart = new Point(Int32.MaxValue - 1, Int32.MaxValue - 1);
            selectionEnd = new Point(Int32.MaxValue, Int32.MaxValue);
        }

        public static void DeleteSelectedBuildings()
        {
            foreach (Building b in selectedBuildings)
            {
                b.Delete();
            }
        }

        public static List<Building> GetSelectedBuildings()
        {
            return selectedBuildings;
        }

        /// <summary>
        /// Gives the 2D plot coordinates of a given 3D point
        /// </summary>
        /// <param name="src">Source point</param>
        /// <returns>Destination point</returns>
        public static Point GetPlotCoords(Vector3 src) {
            if (!(Origin.X == 0 && Origin.Y == 0)) {
                return new Point((int)System.Math.Floor(src.x / Renderable.PlotWidth - Origin.X), (int)System.Math.Floor(src.z / Renderable.PlotHeight - Origin.Y));
            }
            return new Point((int)System.Math.Floor(src.x / Renderable.PlotWidth), (int)System.Math.Floor(src.z / Renderable.PlotHeight));
        }

        /// <summary>
        /// Gets the 3D point representing the center of the given 2D plot.
        /// </summary>
        /// <param name="x">x position of the plot</param>
        /// <param name="y">y position of the plot</param>
        /// <returns>3D point in the center of the plot</returns>
        public static Vector3 GetPlotCenter(int x, int y) { return GetPlotCenter(new Point(x, y)); }

        /// <summary>
        /// Gets the 3D point representing the center of the given 2D plot.
        /// </summary>
        /// <param name="plotCoord">2D Point representing plot coordinate</param>
        /// <returns>3D point in the center of the plot</returns>
        public static Vector3 GetPlotCenter(Point plotCoord) {
            if (!(Origin.X == 0 && Origin.Y == 0)) {
                //Todo: account for terrain height
                return new Vector3((plotCoord.X + Origin.X) * Renderable.PlotWidth + (Renderable.PlotWidth * 0.5f), 
                                    0.0f,
                                    (plotCoord.Y + Origin.Y) * Renderable.PlotHeight + (Renderable.PlotHeight * 0.5f));
            }
            else {
                return new Vector3(plotCoord.X * Renderable.PlotWidth + (Renderable.PlotWidth * 0.5f),
                    0.0f,
                    plotCoord.Y * Renderable.PlotHeight + (Renderable.PlotHeight * 0.5f));
            }
        }

        public static void Quit()
        {
            GameMgr.StartShutdown();
            //Todo: fancy UI for asking if the player is really absolutely sure they want to quit without saving their life's work
        }
    }
}
