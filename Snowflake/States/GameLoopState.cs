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
        private Environment WeatherMgr;

        private SceneNode focalPoint;
        private float angle = 0.78539f;
        private float dist = -7.0f;

        private SceneNode cursorPlane;
        private Entity cursorPlaneEnt;

        private SceneNode selectionBox;
        private Entity selectionBoxEnt;

        private GUI Gui;
        private GameConsole gConsole;
        private BuildingCreationWindow bcWindow;
        private StatsPanel StatsPanel;
        private ToolsPanel ToolsPanel;
        private WeatherOverlay WeatherOverlay;
        private DebugPanel DebugPanel;
        private ContextMenu ContextMenu;

        //Scale factor to get from 20 3ds max units to 1 plot grid square (120 mogre units)
        private const float SCALEFACTOR = 473.0f;

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
            WeatherMgr = new Environment();
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

            gConsole = new GameConsole();
            StatsPanel = new StatsPanel();
            ToolsPanel = new ToolsPanel();
            WeatherOverlay = new WeatherOverlay();
            DebugPanel = new DebugPanel();
            ContextMenu = new ContextMenu();
            bcWindow = new BuildingCreationWindow();

            Gui = new GUI();
            StateMgr.GuiSystem.GUIManager.GUIs.Add(Gui);

            gConsole.CreateGui(Gui);
            StatsPanel.CreateGui(Gui);
            ToolsPanel.CreateGui(Gui);
            DebugPanel.CreateGui(Gui);
            bcWindow.CreateGui(Gui);

            ContextMenu.CreateGui(Gui);
            /*ContextMenu.AddButton("Zone as...", (object source, EventArgs e) => {
                Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(ContextMenu.Location);
                if (result.first) {
                    if (!CityManager.Initialized) {
                        CityManager.Init(result.second);
                        Point newPos = result.second - CityManager.GetOrigin();

                    }
                    else {
                        
                    }
                }
                ContextMenu.Visible = false;
            });*/

            WeatherOverlay.CreateGui(Gui);
            WeatherMgr.SetWeatherOverlay(WeatherOverlay);
        }

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
                    WeatherMgr.Timescale = timescale;
                }
                else {
                    gConsole.WriteLine("Please enter a valid number!");
                }
            }, "Sets the timestep of the game to the specified value."));
            gConsole.AddCommand("quit", new ConsoleCommand((string[] args) => { StateMgr.RequestShutdown(); }, "Quits the game."));
            gConsole.AddCommand("exit", new ConsoleCommand((string[] args) => { StateMgr.RequestShutdown(); }, "Exits the game."));
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
        }

        /// <summary>
        /// Shut down the state
        /// </summary>
        public override void Shutdown() {
            CompositorManager.Singleton.RemoveAll();
            CompositorManager.Singleton.UnloadAll();
            CompositorManager.Singleton.Dispose();
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

} // namespace
