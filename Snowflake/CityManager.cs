using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
        public static bool ShowZones = true;

        public static Dictionary<Building, RenderableBuilding> Buildings;
        public static Dictionary<Plot, RenderablePlot> Plots;

        public static Point selectionStart { get; private set; }
        public static Point selectionEnd { get; private set; }
        private static Point selectionOrigin;

        public static Zones scratchZoneType;
        public static Point scratchZoneStart { get; private set; }
        public static Point scratchZoneEnd { get; private set; }
        private static Point scratchZoneOrigin;

        public static bool Initialized { get; private set; }

        public static Rectangle SelectionBox { 
            get { 
                return new Rectangle(selectionStart, 
                        new Size((selectionEnd.X > selectionStart.X ? selectionEnd.X - selectionStart.X : 0), 
                                 (selectionEnd.Y > selectionStart.Y ? selectionEnd.Y - selectionStart.Y : 0))); 
            } 
        }
        public static Rectangle scratchZoneBox
        { get
            { return new Rectangle(scratchZoneStart,
                        new Size((scratchZoneEnd.X > scratchZoneStart.X ? scratchZoneEnd.X - scratchZoneStart.X : 0),
                                 (scratchZoneEnd.Y > scratchZoneStart.Y ? scratchZoneEnd.Y - scratchZoneStart.Y : 0)));
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
                return Controller.Time;
            }
        }
        public static float Timescale {
            get {
                return Controller.Timescale;
            }
        }
        private static float totalTime;
        private static float lastTotalTime;
        public static float MinuteLength { get { return Controller.MinuteLength; } }
        public static float HourLength { get { return Controller.HourLength; } }
        public static float DayLength { get { return Controller.DayLength; } }

        /// <summary>
        /// Sets the amount by which the game time is incremented every tick
        /// </summary>
        /// <param name="timescale"></param>
        public static void SetTimescale(float timescale) {
            Controller.SetTimescale(timescale);
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
            Plots = new Dictionary<Plot, RenderablePlot>();
            Buildings = new Dictionary<Building, RenderableBuilding>();

            selectionOrigin = new Point(Int32.MaxValue, Int32.MaxValue);
            scratchZoneOrigin = new Point(Int32.MaxValue, Int32.MaxValue);
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

            Random r = new Random();
            for (int i = 0; i < 10; ++i)
            {
                CreateTree(sm, new Vector3((float)(r.NextDouble() * 10000 - 5000), 0, (float)(r.NextDouble() * 10000 - 5000)));
            }
        }

        private static void CreateTree(SceneManager sm, Vector3 offset)
        {
            Entity tree = sm.CreateEntity("tree01.mesh");
            SceneNode treeNode = sm.RootSceneNode.CreateChildSceneNode();
            SceneNode child1 = treeNode.CreateChildSceneNode();
            child1.AttachObject(tree);
            child1.Rotate(Vector3.UNIT_X, Mogre.Math.HALF_PI);
            child1.Rotate(Vector3.UNIT_Z, Mogre.Math.HALF_PI / 2);
            Entity tree2 = sm.CreateEntity("tree01.mesh");
            SceneNode child2 = treeNode.CreateChildSceneNode();
            child2.AttachObject(tree2);
            child2.Rotate(Vector3.UNIT_X, Mogre.Math.HALF_PI);
            child2.Rotate(Vector3.UNIT_Z, Mogre.Math.HALF_PI / -2);
            treeNode.Scale(20, 20, 20);
            treeNode.Translate(offset.x, offset.y + 30, offset.z);
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
        }

        private static void CreateBuilding(object sender, BuildingEventArgs e) {
            GameConsole.ActiveInstance.WriteLine("Added a building at " + e.Building.Parent.X + ", " + e.Building.Parent.Y);

            RenderableBuilding rb = new RenderableBuilding(e.Building);
            if (!Buildings.ContainsKey(e.Building)) { Buildings.Add(e.Building, rb); }

            RenderablePlot rp;
            if (Plots.ContainsKey(e.Building.Parent)) { rp = Plots[e.Building.Parent]; rb.Create(SceneMgr, cityNode); }
            else { 
                rp = new RenderablePlot(e.Building.Parent);
                rp.Create(SceneMgr, cityNode);
                Plots.Add(e.Building.Parent, rp);
            }

            rb.Deleted += (object sender2, EventArgs e2) =>
            {
                Buildings.Remove(e.Building);
            };
        }

        public static bool RenderablePlotExistsAtCoordinate(Point coord)
        {
            return Plots[Haswell.Controller.City.Grid.ElementAt(coord.X, coord.Y)] != null;
        }

        public static void CreateBuildingOnCursor() {
            CreateBuildingOnCursor(new Residential());
        }
        public static void CreateBuildingOnCursor(Building template)
        {
            RenderableBuilding rb = new RenderableBuilding(template);
            rb.Create(GameMgr.StateMgr.Engine.SceneMgr, cityNode);
            rb.IsVirtual = true;
            GameMgr.SetCursorBuilding(rb);
            GameMgr.SetMouseMode(MouseMode.PlacingBuilding);
            GuiMgr.ShowBuildingPlacementPanel();
        }

        public static void BeginZoning(Zones z)
        {
            if (z != Zones.Unzoned)
            {
                GameMgr.SetMouseMode(MouseMode.DrawingZone);
                scratchZoneType = z;
                GameMgr.UpdateScratchZoneBoxZone(scratchZoneType);
            }
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

                foreach (RenderablePlot r in Plots.Values) {
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
        public static void SetScratchZoneOrigin(Point p)
        {
            scratchZoneStart = scratchZoneEnd = scratchZoneOrigin = p;
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

        public static void UpdateScratchZoneBox(Point p) {
            int minx, miny, maxx, maxy;
            minx = Utils3D.Min(scratchZoneOrigin.X, p.X);
            miny = Utils3D.Min(scratchZoneOrigin.Y, p.Y);
            maxx = Utils3D.Max(scratchZoneOrigin.X, p.X);
            maxy = Utils3D.Max(scratchZoneOrigin.Y, p.Y);
            scratchZoneStart = new Point(minx, miny);
            scratchZoneEnd = new Point(maxx, maxy);
        }

        /// <summary>
        /// Takes the active selection and performs the selection action on all the things contained within.
        /// </summary>
        public static bool MakeSelection() {

            if (SelectionBox.Left != Int32.MaxValue - 1 && SelectionBox.Top != Int32.MaxValue - 1 && selectionEnd.X != Int32.MaxValue && selectionEnd.Y != Int32.MaxValue) {

                if (selectedBuildings != null) { selectedBuildings.Clear(); }
                selectedBuildings = Haswell.Controller.City.GetAllInSelection(selectionStart.X, selectionStart.Y, selectionEnd.X, selectionEnd.Y);
                foreach (RenderablePlot r in Plots.Values) {
                    if (selectedBuildings.Intersect(r.Data.Buildings).Count() > 0) {
                        r.Select();
                        GameConsole.ActiveInstance.WriteLine("Selecting building");
                    }
                    else {
                        r.Deselect();
                    }
                }
                //GameConsole.ActiveInstance.WriteLine("Selecting region " + SelectionBox.ToString());
                return true;
                
            }
            return false;
        }

        public static bool MakeZone()
        {
            return SetZoning(scratchZoneBox, scratchZoneType);
        }

        public static void SetMouseMode(MouseMode m) { GameMgr.SetMouseMode(m); }
        public static MouseMode GetMouseMode() { return GameMgr.GetMouseMode(); }

        /// <summary>
        /// Clears the selection
        /// </summary>
        public static void ClearSelection() {
            selectionStart = new Point(Int32.MaxValue - 1, Int32.MaxValue - 1);
            selectionEnd = new Point(Int32.MaxValue, Int32.MaxValue);
            selectionOrigin = new Point(Int32.MaxValue, Int32.MaxValue);
        }
        public static bool SelectionOriginIsValid()
        {
            return selectionOrigin != new Point(Int32.MaxValue, Int32.MaxValue);
        }
        public static bool ScratchZoneOriginIsValid()
        {
            return scratchZoneOrigin != new Point(Int32.MaxValue, Int32.MaxValue);
        }

        public static void ClearScratchZone()
        {
            scratchZoneStart = new Point(Int32.MaxValue - 1, Int32.MaxValue - 1);
            scratchZoneEnd = new Point(Int32.MaxValue, Int32.MaxValue);
            scratchZoneOrigin = new Point(Int32.MaxValue, Int32.MaxValue);
        }

        public static void DeleteSelectedBuildings()
        {
            foreach (Building b in selectedBuildings)
            {
                b.Delete();
            }
        }

        public static void DeselectBuildings()
        {
            foreach (Building b in selectedBuildings)
            {
                Plots[b.Parent].Deselect();
            }
            selectedBuildings.Clear();
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
