using System;
using Mogre;

namespace Snowflake {
    class MainClass {

        Camera camera;
        Viewport viewport;
        Light sun;

        SceneNode world;
        Entity ground;

        //long timeLast, timeNow, timeDelta;
        bool rotating = true;

        public virtual bool UseBufferedInput {
            get { return false; }
        }

        public void init() {
            GV.Root = new Root("test.log");

            GV.Root.FrameStarted += OnFrameStarted;

            ResourceGroupManager.Singleton.AddResourceLocation("media", "Folder");

            GV.Window = GV.Root.Initialise(true);
            GV.Window = GV.Root.CreateRenderWindow("Sustain", 800, 600, false);

            GV.OverlayManager = OverlayManager.Singleton;

            LogManager.Singleton.LogMessage("*** Initializing OIS ***");
            MOIS.ParamList pl = new MOIS.ParamList();
            IntPtr windowHnd;
            GV.Window.GetCustomAttribute("WINDOW", out windowHnd);
            pl.Insert("WINDOW", windowHnd.ToString());

            GV.inputManager = MOIS.InputManager.CreateInputSystem(pl);
            GV.inputKeyboard = (MOIS.Keyboard)GV.inputManager.CreateInputObject(MOIS.Type.OISKeyboard, UseBufferedInput);
            GV.inputMouse = (MOIS.Mouse)GV.inputManager.CreateInputObject(MOIS.Type.OISMouse, UseBufferedInput);

            TextureManager.Singleton.DefaultNumMipmaps = 5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups();

            GV.SceneManager = GV.Root.CreateSceneManager(SceneType.ST_GENERIC);

            camera = GV.SceneManager.CreateCamera("MainCamera");
            camera.Position = new Vector3(0, 500, -500);
            camera.LookAt(new Vector3(0, 0, 0));
            camera.NearClipDistance = 5;
            camera.AutoAspectRatio = true;

            viewport = GV.Window.AddViewport(camera);
            viewport.BackgroundColour = ColourValue.Black;

            sun = GV.SceneManager.CreateLight("Sun");
            sun.Type = Light.LightTypes.LT_POINT;
            sun.Position = new Vector3(0, 1000, 100);
            sun.Direction = new Vector3(0, -1, 0.5f);
            sun.DiffuseColour = new ColourValue(0.98f, 0.95f, 0.9f);
            sun.SpecularColour = ColourValue.White;
            //sun.AttenuationQuadratic = 0.01f;
            //sun.AttenuationLinear = 0.1f;
            sun.CastShadows = true;

            Light sky = GV.SceneManager.CreateLight("sky");
            sky.Type = Light.LightTypes.LT_DIRECTIONAL;
            sky.Position = new Vector3(0, 2000, 0);
            sky.Direction = new Vector3(0, -1, 0);
            sky.DiffuseColour = new ColourValue(0.05f, 0.075f, 0.10f);
            sky.SpecularColour = ColourValue.Black;
            sky.CastShadows = true;

            GV.SceneManager.AmbientLight = ColourValue.Black;
            //GV.SceneManager.ShadowTechnique = ShadowTechnique.StencilAdditive;
            //GV.SceneManager.ActiveCompositorChain
        }

        public void createScene() {

            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 3500, 3500, 40, 40, true, 1, 5, 5, Vector3.UNIT_Z);

            ground = GV.SceneManager.CreateEntity("GroundEntity", "ground");
            ground.SetMaterialName("Grass");
            world = GV.SceneManager.RootSceneNode.CreateChildSceneNode();
            world.AttachObject(ground);
            world.Translate(new Vector3(0, -1, 0));

            for (int x = 0; x < 10; x++) {
                CityManager.Plots.Add(new Plot(x, x));
            }

            CityManager.CreateScene();
        }


        protected virtual bool OnFrameStarted(FrameEvent evt) {

            if (GV.Window.IsClosed)
                return false;

            try {
                GV.inputMouse.Capture();
                GV.inputKeyboard.Capture();
            } catch (NullReferenceException e) {
                Console.WriteLine(e.Message);
                return true;
            }
            MOIS.MouseState_NativePtr mouseState = GV.inputMouse.MouseState;

            if (GV.inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_SPACE) || mouseState.ButtonDown(MOIS.MouseButtonID.MB_Right)) {
                //Console.WriteLine("mouse button pressed");
                camera.Position = new Vector3(camera.Position.x + mouseState.X.rel,
                                               camera.Position.y, camera.Position.z + mouseState.Y.rel);
            }
            if (GV.inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_A)) {
                camera.Position = new Vector3(camera.Position.x + 5,
                                               camera.Position.y, camera.Position.z);
            }
            if (GV.inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_D)) {
                camera.Position = new Vector3(camera.Position.x - 5,
                                               camera.Position.y, camera.Position.z);
            }
            if (GV.inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_W)) {
                camera.Position = new Vector3(camera.Position.x,
                                               camera.Position.y, camera.Position.z + 5);
            }
            if (GV.inputKeyboard.IsKeyDown(MOIS.KeyCode.KC_S)) {
                camera.Position = new Vector3(camera.Position.x,
                                               camera.Position.y, camera.Position.z - 5);
            }

            /*timeLast = timeNow;
            timeNow = GV.Root.Timer.Milliseconds;
            timeDelta = timeNow - timeLast;*/

            if (rotating) {
                /*sceneNode.Yaw(timeDelta * 0.02710f);
                sceneNode.Pitch(timeDelta * 0.070f);
                sceneNode.Roll (timeDelta * 0.056718f);*/

                sun.Position = new Vector3(1000 * (float)System.Math.Cos(GV.Root.Timer.Milliseconds / -2000.0), 1000 * (float)System.Math.Sin(GV.Root.Timer.Milliseconds / -2000.0), -300);
                //sun.AttenuationQuadratic = Math.Max (0, sun.Position.y / 300.0f);
                //camera.Position = new Vector3 (500 * Math.Cos (GV.Root.Timer.Milliseconds / 5879.0), 500, 500 * Math.Sin (GV.Root.Timer.Milliseconds / 5879.0));
                //camera.SetAutoTracking(true, sceneNode);

            }

            CityManager.Update();
            return true;
        }


        public static void Main(string[] args) {
            MainClass main = new MainClass();
            main.init();
            main.createScene();
            Console.WriteLine("start rendering...");
            GV.Root.StartRendering();
        }
    }
}