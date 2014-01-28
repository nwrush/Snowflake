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
        private StatsPanel Tools;
        private WeatherOverlay WeatherOverlay;
        private DebugPanel DebugPanel;
        private ContextMenu ContextMenu;

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
            Tools = new StatsPanel();
            WeatherOverlay = new WeatherOverlay();
            DebugPanel = new DebugPanel();
            ContextMenu = new ContextMenu();
            bcWindow = new BuildingCreationWindow();

            Gui = new GUI();
            StateMgr.GuiSystem.GUIManager.GUIs.Add(Gui);

            gConsole.CreateGui(Gui);
            Tools.CreateGui(Gui);
            DebugPanel.CreateGui(Gui);
            bcWindow.CreateGui(Gui);

            ContextMenu.CreateGui(Gui);
            ContextMenu.AddButton("Create Building", (object source, EventArgs e) => {
                Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(ContextMenu.Location);
                if (result.first) {
                    if (!CityManager.Initialized) {
                        CityManager.Init(result.second);
                        Point newPos = result.second - CityManager.GetOrigin();
                        //CityManager.NewBuilding(newPos );
                        bcWindow.Show();
                    }
                    else {
                        //CityManager.NewBuilding(result.second);
                        bcWindow.Show();
                    }
                }
                ContextMenu.Visible = false;
            });

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
        /// Update the game
        /// </summary>
        /// <param name="_frameTime"></param>
        public override void Update(float _frameTime) {
            // check if the state was initialized before
            if (StateMgr == null)
                return;

            UpdateCameraPosition();
            HandleInput(StateMgr);

            if (CityManager.Initialized) {
                CityManager.Update(_frameTime);
                UpdateGUI(_frameTime);
            }

            WeatherMgr.Update(StateMgr.Engine.SceneMgr);
            DebugPanel.UpdateFPS(_frameTime);
        }

        private void UpdateCameraPosition() {
            StateMgr.Engine.Camera.Position = new Vector3(focalPoint.Position.x + (-500) * Mogre.Math.Cos(angle), 500, focalPoint.Position.z + -(500) * Mogre.Math.Sin(angle));
            StateMgr.Engine.Camera.Position += StateMgr.Engine.Camera.Direction * (float)System.Math.Pow(dist, 3);
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
                if (intersection.first && selboxShouldUpate()) {
                    Vector3 intersectionPt = mouseRay.Origin + mouseRay.Direction * intersection.second;
                    Vector3 plotCenter = CityManager.GetPlotCenter(CityManager.GetPlotCoords(intersectionPt));
                    cursorPlane.SetPosition(plotCenter.x, plotCenter.y + 1f, plotCenter.z);
                    DebugPanel.SetDebugText(CityManager.GetPlotCoords(intersectionPt).ToString());
                }
                
                //Middle click - rotate the view
                if (mStateMgr.Input.IsMouseButtonDown(MouseButtonID.MB_Middle) && viewShouldUpdate()) {

                    //Mouse rotate control
                    angle += mStateMgr.Input.MouseMoveX * 0.01f;
                    //mStateMgr.Input += mStateMgr.Input.MouseMoveX;

                    //Mouse drag control
                    /*Vector2 mouseMoveRotated = Utils3D.RotateVector2(new Vector2(mStateMgr.Input.MouseMoveX, mStateMgr.Input.MouseMoveY), angle);
                    focalPoint.Translate(new Vector3(mouseMoveRotated.y, 0, mouseMoveRotated.x));
                    mStateMgr.GuiSystem.GUIManager.Cursor.SetActiveMode(CursorMode.ResizeTop);*/

                    if (!ContextMenu.HitTest(MousePosition(mStateMgr.Input))) {
                        ContextMenu.Visible = false;
                    }
                }
                //Right click - context menus
                if (mStateMgr.Input.WasMouseButtonPressed(MouseButtonID.MB_Right))
                {
                    if (ContextMenu.Visible == true && !ContextMenu.HitTest(MousePosition(mStateMgr.Input))) {
                        ContextMenu.Visible = false;
                    }
                    else {
                        ContextMenu.Location = new Point(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY);
                        ContextMenu.Visible = true;
                    }
                }

                //Mouse click - 3D selection
                if (mStateMgr.Input.WasMouseButtonPressed(MouseButtonID.MB_Left)) {
                    if (!ContextMenu.HitTest(MousePosition(mStateMgr.Input))) {
                        ContextMenu.Visible = false;

                        if (canSelect()) {
                            Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                            if (result.first) {
                                CityManager.SetSelectionOrigin(result.second);
                            }
                        }
                    }
                }

                if (mStateMgr.Input.IsMouseButtonDown(MouseButtonID.MB_Left)) {
                   if (canSelect()) {
                       Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                       if (result.first) {
                           CityManager.UpdateSelectionBox(result.second);
                           UpdateSelectionBox();
                       }
                   }
                }

                if (mStateMgr.Input.WasMouseButtonReleased(MouseButtonID.MB_Left)) {
                    if (canSelect()) {
                        Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                        if (result.first) {
                            CityManager.UpdateSelectionBox(result.second);
                        }
                        CityManager.MakeSelection();
                        UpdateSelectionBox();
                    }
                }

                if (mStateMgr.Input.MouseMoveZ != 0.0f) {
                    dist += mStateMgr.Input.MouseMoveZ * 0.002f;
                    if (dist < -12.0f) { dist = -12.0f; }
                    if (dist > 2.0f) { dist = 2.0f; }
                }

                if (viewShouldUpdate()) {
                    //WASD Control
                    int speed = 10;
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_A)) {
                        focalPoint.Translate(new Vector3((-dist + speed) * Mogre.Math.Sin(angle), 0, -(-dist + speed) * Mogre.Math.Cos(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_W)) {
                        focalPoint.Translate(new Vector3((-dist + speed) * Mogre.Math.Cos(angle), 0, (-dist + speed) * Mogre.Math.Sin(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_D)) {
                        focalPoint.Translate(new Vector3(-(-dist + speed) * Mogre.Math.Sin(angle), 0, (-dist + speed) * Mogre.Math.Cos(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_S)) {
                        focalPoint.Translate(new Vector3(-(-dist + speed) * Mogre.Math.Cos(angle), 0, -(-dist + speed) * Mogre.Math.Sin(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_Q)) {
                        angle += 0.01f;
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_E)) {
                        angle -= 0.01f;
                    }
                }

                //Toggle the console with `
                if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_GRAVE)) {
                    gConsole.Visible = !gConsole.Visible;
                }
            }

            // check if the escape key was pressed
            if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_EQUALS)) {
                // quit the application
                mStateMgr.RequestShutdown();
            }
        }

        private void UpdateSelectionBox() {
            Vector3 center = (CityManager.GetPlotCenter(CityManager.SelectionBox.Left, CityManager.SelectionBox.Top)
                + CityManager.GetPlotCenter(CityManager.SelectionBox.Right, CityManager.SelectionBox.Bottom))
                 * 0.5f;
            selectionBox.SetPosition(center.x, center.y, center.z);
            selectionBox.SetScale(CityManager.SelectionBox.Width * SCALEFACTOR + SCALEFACTOR, SCALEFACTOR / 2.0f, CityManager.SelectionBox.Height * SCALEFACTOR + SCALEFACTOR);
            selectionBox.SetVisible(true);
        }
        public void UpdateGUI(float frametime) {
            if (CityManager.Initialized)
                Tools.Update(frametime);
        }

        private bool selboxShouldUpate() {
            return ContextMenu.Visible == false;
        }

        private bool viewShouldUpdate() {
            return ContextMenu.Visible == false;
        }

        private bool canSelect() {
            return ContextMenu.Visible == false && gConsole.HitTest(MousePosition(StateMgr.Input)) == false;
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
