using System;

using Mogre;

using Snowflake.Modules;
using Snowflake.GuiComponents;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

using MOIS;

using Vector3 = Mogre.Vector3;

namespace Snowflake.States {
    
    /// <summary>
    /// Program state for playing the game
    /// </summary>
    public partial class GameLoopState : State {

        public StateManager StateMgr { get; private set; }
        public WeatherManager WeatherMgr { get; private set; }
        public GuiManager GuiMgr;

        private SceneNode focalPoint;
        private float angle = 0.78539f;
        private float dist = -7.0f;

        private SceneNode cursorPlane;
        private Entity cursorPlaneEnt;
        private SceneNode selectionBox;
        private Entity selectionBoxEnt;
        private MouseMode mouseMode = MouseMode.Selection;
        private RenderableBuilding tempBuilding;
        private Haswell.Zones tempZone;

        #region Properties

        private DebugPanel DebugPanel { get { return GuiMgr.DebugPanel; } }
        private GameConsole gConsole { get { return GuiMgr.GameConsole; } }
        private ContextMenu ContextMenu { get { return GuiMgr.ContextMenu; } }

        #endregion

        //Scale factor to get from 20 3ds max units to 1 plot grid square (120 mogre units)
        private const float SCALEFACTOR = 473.0f * (Renderable.PlotWidth / 120);

        /// <summary>
        /// Constructor
        /// </summary>
        public GameLoopState() {
            StateMgr = null;
            WeatherMgr = null;
        }

        /// <summary>
        /// Start up the state
        /// </summary>
        /// <param name="_mgr">State manager for this state</param>
        /// <returns></returns>
        public override bool Startup(StateManager _mgr) {
            // store reference to the state manager
            StateMgr = _mgr;

            // get reference to the ogre manager
            OgreManager engine = StateMgr.Engine;

            //Instantiate everything
            WeatherMgr = new WeatherManager();
            CityManager.SetGameMgr(this);

            //Initialize everythings
            createScene(engine);
            createUI();
            createCommands();
            //Initialize the City Manager (that's everything right?)
            CityManager.Init(0, 0);

            CompositorManager.Singleton.AddCompositor(engine.Window.GetViewport(0), "Bloom");
            CompositorManager.Singleton.AddCompositor(engine.Window.GetViewport(0), "Radial Blur");

            // OK
            return true;
        }

        /// <summary>
        /// Set up camera and call other createScene methods
        /// </summary>
        /// <param name="engine"></param>
        public void createScene(OgreManager engine) {
            engine.SceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;

            setupCamera(engine);

            cursorPlane = engine.SceneMgr.RootSceneNode.CreateChildSceneNode("cursorPlane");
            cursorPlaneEnt = engine.SceneMgr.CreateEntity("cursorPlaneEnt", "cursorplane.mesh");
            cursorPlane.AttachObject(cursorPlaneEnt);
            cursorPlane.Scale(new Vector3(SCALEFACTOR, SCALEFACTOR, SCALEFACTOR));
            cursorPlaneEnt.CastShadows = false;

            selectionBox = engine.SceneMgr.RootSceneNode.CreateChildSceneNode("selectionBox");
            selectionBoxEnt = engine.SceneMgr.CreateEntity("selectionBoxEnt", "selectionbox.mesh");
            selectionBox.AttachObject(selectionBoxEnt);
            selectionBox.Scale(new Vector3(SCALEFACTOR, SCALEFACTOR / 2.0f, SCALEFACTOR));
            selectionBoxEnt.CastShadows = false;

            selectionBox.SetVisible(false);

            WeatherMgr.CreateScene(engine.SceneMgr);
            CityManager.CreateScene(engine.SceneMgr);
        }

        private void setupCamera(OgreManager engine) {
            focalPoint = engine.SceneMgr.RootSceneNode.CreateChildSceneNode("focalPoint");
            focalPoint.Position = new Vector3(0, 0, 0);

            engine.Camera.NearClipDistance = 5;
            engine.Camera.FarClipDistance = 2048;
            engine.Camera.AutoAspectRatio = true;
            engine.Camera.SetAutoTracking(true, focalPoint);
        }

        /// <summary>
        /// Set up overlays for user interface
        /// </summary>
        public void createUI() {
            GuiMgr = new GuiManager();
            GuiMgr.CreateDefaultGui(StateMgr.GuiSystem);
            GuiMgr.HideBuildingPlacementPanel();
        }
        #region Console Commands
        /// <summary>
        /// Register the console commands 
        /// </summary>
        private void createCommands() {
            gConsole.AddCommand("sw", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { gConsole.WriteLine("Usage: sw [weathertype]"); return; }
                Weather w;
                Enum.TryParse<Weather>(args[0], out w);
                if (w != Weather.Null) {
                    WeatherMgr.SwitchWeather(w);
                    gConsole.WriteLine("Switching weather to " + args[0]);
                }
                else { gConsole.WriteLine("Invalid weather type \"" + args[0] + "\"!"); }
            }, "Sets the weather to the specified type, resetting the timer."));
            gConsole.AddCommand("fw", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { gConsole.WriteLine("Usage: fw [weathertype]"); return; }
                Weather w;
                Enum.TryParse<Weather>(args[0], out w);
                if (w != Weather.Null) {
                    WeatherMgr.ForceWeather(w);
                    gConsole.WriteLine("Forcing weather to " + args[0]);
                }
                else { gConsole.WriteLine("Invalid weather type \"" + args[0] + "\"!"); }
            }, "Sets the weather to the specified type without resetting the timer."));
            gConsole.AddCommand("timescale", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { gConsole.WriteLine("Usage: timescale [n], default 1.0"); return; }
                float timescale;
                if (Single.TryParse(args[0], out timescale)) {
                    CityManager.SetTimescale(timescale);
                }
                else {
                    gConsole.WriteLine("Please enter a valid number!");
                }
            }, "Sets the timestep of the game to the specified value."));
            gConsole.AddCommand("quit", new ConsoleCommand((string[] args) => { StartShutdown(); }, "Quits the game."));
            gConsole.AddCommand("exit", new ConsoleCommand((string[] args) => { StartShutdown(); }, "Exits the game."));
            gConsole.AddCommand("debug", new ConsoleCommand((string[] args) => { DebugPanel.Visible = !DebugPanel.Visible; }, "Toggles the debug panel."));
            gConsole.AddCommand("wireframe", new ConsoleCommand((string[] args) => {
                if (StateMgr.Engine.Camera.PolygonMode == PolygonMode.PM_WIREFRAME) {
                    StateMgr.Engine.Camera.PolygonMode = PolygonMode.PM_SOLID;
                }
                else { StateMgr.Engine.Camera.PolygonMode = PolygonMode.PM_WIREFRAME; }
            }, "Toggles rendering in wireframe mode."));
            gConsole.AddCommand("bloom", new ConsoleCommand((string[] args) => { 
                bool enabled;
                if (args.Length == 0) {
                    enabled = !(CompositorManager.Singleton.GetCompositorChain(StateMgr.Engine.Window.GetViewport(0)).GetCompositor("Bloom")).Enabled;
                }
                else { enabled = (args[0] == "0" || args[0] == "false") ? true : false; }
                CompositorManager.Singleton.SetCompositorEnabled(StateMgr.Engine.Window.GetViewport(0), "Bloom", enabled);
            }, "Toggles the bloom shader."));
            gConsole.AddCommand("radialblur", new ConsoleCommand((string[] args) => {
                bool enabled;
                if (args.Length == 0) {
                    enabled = !(CompositorManager.Singleton.GetCompositorChain(StateMgr.Engine.Window.GetViewport(0)).GetCompositor("Radial Blur")).Enabled;
                }
                else { enabled = (args[0] == "0" || args[0] == "false") ? true : false; }
                CompositorManager.Singleton.SetCompositorEnabled(StateMgr.Engine.Window.GetViewport(0), "Radial Blur", enabled);
            }, "Toggles the radial blur shader."));
            gConsole.AddCommand("addbuilding", new ConsoleCommand((string[] args) => {
                if (args.Length < 2) { gConsole.WriteLine("Usage: addbuilding <x> <y>"); return; }
                else {
                    int x, y;
                    if (Int32.TryParse(args[0], out x) && Int32.TryParse(args[1], out y)) {
                        CityManager.NewBuilding<Haswell.Buildings.Commercial>(x, y);
                        return;
                    }
                    else {
                        gConsole.WriteLine("Usage: addbuilding <x> <y>, where x and y are valid integers."); 
                        return;
                    }
                }
            }, "Adds a building at x, y"));
            gConsole.AddCommand("setzone", new ConsoleCommand((string[] args) => {
                if (args.Length != 3 && args.Length != 5) { gConsole.WriteLine("Usage: setzone <z> <x1> <y1> [<x2> <y2>]"); }
                else
                {
                    int x1, y1, x2, y2;
                    Haswell.Zones z;

                }
            }, "Sets zone z from x1, y1 to x2, y2"));
        }
        #endregion

        public void SetMouseMode(MouseMode m) {
            this.mouseMode = m;
        }
        public void SetCursorBuilding(RenderableBuilding rb) {
            this.tempBuilding = rb;
        }

        /// <summary>
        /// Shut down the state
        /// </summary>
        public override void Shutdown() {
            CompositorManager.Singleton.RemoveAll();
            CompositorManager.Singleton.UnloadAll();
            CompositorManager.Singleton.Dispose();
        }

        public void StartShutdown()
        {
            StateMgr.RequestShutdown();
        }

        /// <summary>
        /// Gets the specified point as a PointF relative to the origin, from 0 to 1.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private PointF GetSelectionOrigin(Point p) {
            //Store w and h because long variable names
            int w = StateMgr.Engine.Window.GetViewport(0).ActualWidth;
            int h = StateMgr.Engine.Window.GetViewport(0).ActualHeight;

            //get p relative to center of screen, as a number from 0 to 1
            return new PointF(p.X / (float)w, p.Y / (float)h);
        }

        /// <summary>
        /// Returns a Ray in 3D space from the given mouse coordinates on the screen.
        /// </summary>
        /// <param name="mousex">X Position of the mouse</param>
        /// <param name="mousey">Y Position of the mouse</param>
        /// <returns></returns>
        private Ray GetSelectionRay(int mousex, int mousey) {
            PointF offset = GetSelectionOrigin(new Point(mousex, mousey));
            return StateMgr.Engine.Camera.GetCameraToViewportRay(offset.X, offset.Y);
        }
        private Ray GetSelectionRay(Point pt) { return GetSelectionRay(pt.X, pt.Y); }

        private Mogre.Pair<bool, Point> getPlotCoordsFromScreenPoint(Point p) {
            Ray mouseRay = GetSelectionRay(p);

            Mogre.Pair<bool, float> intersection = mouseRay.Intersects(new Plane(Vector3.UNIT_Y, Vector3.ZERO));
            if (intersection.first) {
                Vector3 intersectionPt = mouseRay.Origin + mouseRay.Direction * intersection.second;
                return new Mogre.Pair<bool,Point> (true, CityManager.GetPlotCoords(intersectionPt));
            }
            else { return new Mogre.Pair<bool, Point>(false, Point.Empty); }
        }

        private Point MousePosition(MoisManager input) {
            return new Point(input.MousePosX, input.MousePosY);
        }
    } // class

    public enum MouseMode {
        None,
        Selection,
        PlacingBuilding,
        DrawingZone
    }

} // namespace
