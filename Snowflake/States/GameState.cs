using System;

using Mogre;

using Snowflake.Modules;
using Snowflake.GuiComponents;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

using Vector3 = Mogre.Vector3;

namespace Snowflake.States {
    
    /// <summary>
    /// Program state for playing the game
    /// </summary>
    public class GameState : State {

        private StateManager StateMgr;
        private WeatherManager WeatherMgr;
        private CityManager CityMgr;

        private SceneNode focalPoint;
        private float angle;

        private GameConsole GameConsole;
        private ToolPanel Tools;
        private WeatherOverlay WeatherOverlay;
        private DebugPanel DebugPanel;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameState() {
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
            WeatherMgr = new WeatherManager();
            CityMgr = new CityManager();

            GameConsole = new GameConsole();
            Tools = new ToolPanel();
            WeatherOverlay = new WeatherOverlay();
            DebugPanel = new DebugPanel();

            //Initialize everythings
            createScene(engine);
            createUI();
            createCommands();

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

            Utils3D.DrawLine(engine.SceneMgr, new Vector3(0, 0, 0), new Vector3(0, 500, 0));

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
            GameConsole.CreateGui(this.StateMgr.GuiSystem);
            Tools.CreateGui(this.StateMgr.GuiSystem);
            DebugPanel.CreateGui(this.StateMgr.GuiSystem);

            WeatherOverlay.CreateGui(this.StateMgr.GuiSystem);
            WeatherMgr.SetWeatherOverlay(WeatherOverlay);
        }

        /// <summary>
        /// Register the console commands 
        /// </summary>
        private void createCommands() {
            GameConsole.AddCommand("sw", new ConsoleCommand((string[] args) => {
                if (args.Length == 0 || (args.Length > 0 && args[0].Trim() == String.Empty)) { GameConsole.WriteLine("Usage: sw [weathertype]"); return; }
                Weather w;
                Enum.TryParse<Weather>(args[0], out w);
                if (w != Weather.Null) {
                    WeatherMgr.SwitchWeather(w);
                    GameConsole.WriteLine("Switching weather to " + args[0]);
                }
                else { GameConsole.WriteLine("Invalid weather type \"" + args[0] + "\"!"); }
            }, "Sets the weather to the specified type, resetting the timer."));
            GameConsole.AddCommand("fw", new ConsoleCommand((string[] args) => {
                if (args.Length == 0 || (args.Length > 0 && args[0].Trim() == String.Empty)) { GameConsole.WriteLine("Usage: fw [weathertype]"); return; }
                Weather w;
                Enum.TryParse<Weather>(args[0], out w);
                if (w != Weather.Null) {
                    WeatherMgr.ForceWeather(w);
                    GameConsole.WriteLine("Forcing weather to " + args[0]);
                }
                else { GameConsole.WriteLine("Invalid weather type \"" + args[0] + "\"!"); }
            }, "Sets the weather to the specified type without resetting the timer."));
            GameConsole.AddCommand("timescale", new ConsoleCommand((string[] args) => {
                if (args.Length == 0 || (args.Length > 0 && args[0].Trim() == String.Empty)) { GameConsole.WriteLine("Usage: timescale [n], default 1.0"); return; }
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
        }

        /// <summary>
        /// Shut down the state
        /// </summary>
        public override void Shutdown() {

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

            CityMgr.Update();
            WeatherMgr.Update();
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

                //Mouse drag control
                if (mStateMgr.Input.IsMouseButtonDown(MOIS.MouseButtonID.MB_Middle)) {

                    Vector2 mouseMoveRotated = Utils3D.RotateVector2(new Vector2(mStateMgr.Input.MouseMoveX, mStateMgr.Input.MouseMoveY), angle);
                    focalPoint.Translate(new Vector3(mouseMoveRotated.y, 0, mouseMoveRotated.x));
                    mStateMgr.GuiSystem.GUIManager.Cursor.SetActiveMode(CursorMode.ResizeTop);
                }
                //Mouse rotate control
                if (mStateMgr.Input.IsMouseButtonDown(MOIS.MouseButtonID.MB_Right))
                {
                    angle += mStateMgr.Input.MouseMoveX * 0.01f;
                }
                //Mouse click - 3D selection
                if (mStateMgr.Input.WasMouseButtonPressed(MOIS.MouseButtonID.MB_Left)) {
                    Vector3 origin = engine.Camera.Position;
                    Vector3 Direction = engine.Camera.Direction;
                    PointF offset = GetSelectionOrigin(new Point(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY));

                    Ray r = new Ray(origin, Direction);
                    //Uhhh...now do something with that nice ray of sunshine
                    Utils3D.DrawLine(engine.SceneMgr, r.Origin, r.Origin + r.Direction * 9999);
                }

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

            //get p relative to center of screen, as a number from -1 to 1
            PointF rel = new PointF((p.X - (int)(w * 0.5)) / (w * 0.5f), (p.Y - (int)(h * 0.5)) / (h * 0.5f));
            return rel;
        }

    } // class

} // namespace
