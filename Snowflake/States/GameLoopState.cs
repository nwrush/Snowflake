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
using Quaternion = Mogre.Quaternion;

namespace Snowflake.States {
    
    /// <summary>
    /// Program state for playing the game
    /// </summary>
    public class GameLoopState : State {

        private StateManager StateMgr;
        private Environment WeatherMgr;
        private CityManager CityMgr;

        private SceneNode focalPoint;
        private float angle;

        private SceneNode selBox;
        private Entity selBoxEnt;

        private GUI Gui;
        private GameConsole GameConsole;
        private ToolPanel Tools;
        private WeatherOverlay WeatherOverlay;
        private DebugPanel DebugPanel;
        private ContextMenu ContextMenu;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameLoopState() {
            StateMgr = null;
            WeatherMgr = null;
            CityMgr = null;
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
            CityMgr = new CityManager();

            GameConsole = new GameConsole();
            Tools = new ToolPanel();
            WeatherOverlay = new WeatherOverlay();
            DebugPanel = new DebugPanel();
            ContextMenu = new ContextMenu();

            //Initialize everythings
            createScene(engine);
            createUI();
            createCommands();

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

            selBox = engine.SceneMgr.RootSceneNode.CreateChildSceneNode("SelBox");
            selBoxEnt = engine.SceneMgr.CreateEntity("SelBoxEnt", "sel_Box001.mesh");
            selBox.AttachObject(selBoxEnt);
            selBox.Scale(new Vector3(440, 440, 440));
            selBoxEnt.CastShadows = false;

            WeatherMgr.CreateScene(engine.SceneMgr);
            CityMgr.CreateScene(engine.SceneMgr);
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
            Gui = new GUI();
            StateMgr.GuiSystem.GUIManager.GUIs.Add(Gui);

            GameConsole.CreateGui(Gui);
            Tools.CreateGui(Gui);
            DebugPanel.CreateGui(Gui);

            ContextMenu.CreateGui(Gui);
            ContextMenu.AddButton("Create Building", null);

            WeatherOverlay.CreateGui(Gui);
            WeatherMgr.SetWeatherOverlay(WeatherOverlay);
        }

        /// <summary>
        /// Register the console commands 
        /// </summary>
        private void createCommands() {
            GameConsole.AddCommand("sw", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { GameConsole.WriteLine("Usage: sw [weathertype]"); return; }
                Weather w;
                Enum.TryParse<Weather>(args[0], out w);
                if (w != Weather.Null) {
                    WeatherMgr.SwitchWeather(w);
                    GameConsole.WriteLine("Switching weather to " + args[0]);
                }
                else { GameConsole.WriteLine("Invalid weather type \"" + args[0] + "\"!"); }
            }, "Sets the weather to the specified type, resetting the timer."));
            GameConsole.AddCommand("fw", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { GameConsole.WriteLine("Usage: fw [weathertype]"); return; }
                Weather w;
                Enum.TryParse<Weather>(args[0], out w);
                if (w != Weather.Null) {
                    WeatherMgr.ForceWeather(w);
                    GameConsole.WriteLine("Forcing weather to " + args[0]);
                }
                else { GameConsole.WriteLine("Invalid weather type \"" + args[0] + "\"!"); }
            }, "Sets the weather to the specified type without resetting the timer."));
            GameConsole.AddCommand("timescale", new ConsoleCommand((string[] args) => {
                if (args.Length == 0) { GameConsole.WriteLine("Usage: timescale [n], default 1.0"); return; }
                float timescale;
                if (Single.TryParse(args[0], out timescale)) {
                    WeatherMgr.Timescale = timescale;
                }
                else {
                    GameConsole.WriteLine("Please enter a valid number!");
                }
            }, "Sets the timestep of the game to the specified value."));
            GameConsole.AddCommand("quit", new ConsoleCommand((string[] args) => { StateMgr.RequestShutdown(); }, "Quits the game."));
            GameConsole.AddCommand("exit", new ConsoleCommand((string[] args) => { StateMgr.RequestShutdown(); }, "Exits the game."));
            GameConsole.AddCommand("debug", new ConsoleCommand((string[] args) => { DebugPanel.Visible = !DebugPanel.Visible; }, "Toggles the debug panel."));
            GameConsole.AddCommand("wireframe", new ConsoleCommand((string[] args) => {
                if (StateMgr.Engine.Camera.PolygonMode == PolygonMode.PM_WIREFRAME) {
                    StateMgr.Engine.Camera.PolygonMode = PolygonMode.PM_SOLID;
                }
                else { StateMgr.Engine.Camera.PolygonMode = PolygonMode.PM_WIREFRAME; }
            }, "Toggles rendering in wireframe mode."));
            GameConsole.AddCommand("bloom", new ConsoleCommand((string[] args) => { 
                bool enabled;
                if (args.Length == 0) {
                    enabled = !(CompositorManager.Singleton.GetCompositorChain(StateMgr.Engine.Window.GetViewport(0)).GetCompositor("Bloom")).Enabled;
                }
                else { enabled = (args[0] == "0" || args[0] == "false") ? true : false; }
                CompositorManager.Singleton.SetCompositorEnabled(StateMgr.Engine.Window.GetViewport(0), "Bloom", enabled);
            }, "Toggles the bloom shader."));
            GameConsole.AddCommand("radialblur", new ConsoleCommand((string[] args) => {
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
        /// Update the game
        /// </summary>
        /// <param name="_frameTime"></param>
        public override void Update(long _frameTime) {
            // check if the state was initialized before
            if (StateMgr == null)
                return;

            UpdateCameraPosition();
            HandleInput(StateMgr);

            CityMgr.Update(_frameTime);
            WeatherMgr.Update(StateMgr.Engine.SceneMgr);
            DebugPanel.UpdateFPS(_frameTime);
        }

        private void UpdateCameraPosition() {
            StateMgr.Engine.Camera.Position = new Vector3(focalPoint.Position.x + -500 * Mogre.Math.Cos(angle), 500, focalPoint.Position.z + -500 * Mogre.Math.Sin(angle));
        }

        /// <summary>
        /// Provide input handling during the game.
        /// </summary>
        /// <param name="mStateMgr"></param>
        private void HandleInput(StateManager mStateMgr) {
            // get reference to the ogre manager
            OgreManager engine = mStateMgr.Engine;

            //If we're not typing into a form or something...
            if (!StateManager.SupressGameControl) {
                Ray mouseRay = GetSelectionRay(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY);

                Mogre.Pair<bool, float> intersection = mouseRay.Intersects(new Plane(Vector3.UNIT_Y, Vector3.ZERO));
                if (intersection.first) {
                    Vector3 intersectionPt = mouseRay.Origin + mouseRay.Direction * intersection.second;
                    intersectionPt = new Vector3((float)System.Math.Floor(intersectionPt.x / Renderable.PlotWidth) * Renderable.PlotWidth + (Renderable.PlotWidth * 0.5f),
                                                0,
                                                (float)System.Math.Floor(intersectionPt.z / Renderable.PlotHeight) * Renderable.PlotHeight + (Renderable.PlotHeight * 0.5f));
                    selBox.SetPosition(intersectionPt.x, intersectionPt.y, intersectionPt.z);
                }
                
                if (mStateMgr.Input.IsMouseButtonDown(MOIS.MouseButtonID.MB_Middle)) {

                    //Mouse rotate control
                    angle += mStateMgr.Input.MouseMoveX * 0.01f;
                    //mStateMgr.Input += mStateMgr.Input.MouseMoveX;

                    //Mouse drag control
                    /*Vector2 mouseMoveRotated = Utils3D.RotateVector2(new Vector2(mStateMgr.Input.MouseMoveX, mStateMgr.Input.MouseMoveY), angle);
                    focalPoint.Translate(new Vector3(mouseMoveRotated.y, 0, mouseMoveRotated.x));
                    mStateMgr.GuiSystem.GUIManager.Cursor.SetActiveMode(CursorMode.ResizeTop);*/

                    if (!ContextMenu.HitTest(new Point(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY))) {
                        ContextMenu.Visible = false;
                    }
                }
                //Right click - context menus
                if (mStateMgr.Input.WasMouseButtonPressed(MOIS.MouseButtonID.MB_Right))
                {
                    ContextMenu.Location = new Point(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY);
                    ContextMenu.Visible = true;
                }
                //Mouse click - 3D selection
                if (mStateMgr.Input.WasMouseButtonPressed(MOIS.MouseButtonID.MB_Left)) {
                    if (!ContextMenu.HitTest(new Point(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY))) {
                        ContextMenu.Visible = false;
                        //Uhhh...now do something with that nice ray of sunshine
                        //Utils3D.DrawRay(engine.SceneMgr, mouseRay);
                    }
                }
                DebugPanel.SetDebugText(engine.Camera.Position.ToString());

                //WASD Control
                if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_A)) {
                    focalPoint.Translate(new Vector3(5 * Mogre.Math.Sin(angle), 0, -5 * Mogre.Math.Cos(angle)));
                }
                if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_W)) {
                    focalPoint.Translate(new Vector3(5 * Mogre.Math.Cos(angle), 0, 5 * Mogre.Math.Sin(angle)));
                }
                if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_D)) {
                    focalPoint.Translate(new Vector3(-5 * Mogre.Math.Sin(angle), 0, 5 * Mogre.Math.Cos(angle)));
                }
                if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_S)) {
                    focalPoint.Translate(new Vector3(-5 * Mogre.Math.Cos(angle), 0, -5 * Mogre.Math.Sin(angle)));
                }

                //Toggle the console with `
                if (mStateMgr.Input.WasKeyPressed(MOIS.KeyCode.KC_GRAVE)) {
                    GameConsole.Visible = !GameConsole.Visible;
                }
            }

            // check if the escape key was pressed
            if (mStateMgr.Input.WasKeyPressed(MOIS.KeyCode.KC_EQUALS)) {
                // quit the application
                mStateMgr.RequestShutdown();
            }
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

    } // class

} // namespace
