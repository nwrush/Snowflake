using System;
using Axiom;
using Axiom.Core;
using Axiom.Math;
using Axiom.Graphics;
using Axiom.Input;
using Axiom.Platforms.OpenTK;

namespace Snowflake
{
	class MainClass
	{

        Root root;
        RenderWindow window;
        SceneManager sceneManager;
		OpenTKPlatformManager platformManager;

		InputReader input;
        Camera camera;
        Viewport viewport;
        Light light;

        SceneNode sceneNode;
		SceneNode world;
        Entity entity;
		Entity ground;

        //long timeLast, timeNow, timeDelta;
		bool rotating = true;

        public void init()
        {
            root = new Root("test.log");

            root.RenderSystem = root.RenderSystems["OpenGL"];
            root.FrameStarted += OnFrameStarted;

			ResourceGroupManager.Instance.AddResourceLocation("media", "Folder");

            window = root.Initialize(false);
            window = root.CreateRenderWindow( "TestRenderWindow", 800, 600, false);

			platformManager = new OpenTKPlatformManager();
			input  = platformManager.CreateInputReader();
			input.Initialize(window, true, true, false, false);

			TextureManager.Instance.DefaultMipmapCount = 5;
			ResourceGroupManager.Instance.InitializeAllResourceGroups();

            sceneManager = root.CreateSceneManager(SceneType.Generic);

            camera = sceneManager.CreateCamera("TestCamera");
            camera.Position = new Vector3(0, 500, -500);
            camera.LookAt(new Vector3(0, 0, 0));
            camera.Near = 5;
            camera.AutoAspectRatio = true;

            viewport = window.AddViewport(camera);
            viewport.BackgroundColor = ColorEx.Black;

            light = sceneManager.CreateLight("light1");
            light.Type = LightType.Point;
			light.Position = new Vector3(0, 500, 100);
			light.Direction = new Vector3(0, -1, 0.5);
            light.Diffuse = ColorEx.White;
            light.Specular = ColorEx.White;
			light.CastShadows = true;

			sceneManager.AmbientLight = ColorEx.Black;
			//sceneManager.ShadowTechnique = ShadowTechnique.StencilAdditive;
			//sceneManager.ActiveCompositorChain
        }

        public void createScene()
        {
            entity = sceneManager.CreateEntity("TestEntity", PrefabEntity.Cube);
			entity.CastShadows = true;

            sceneNode = sceneManager.RootSceneNode.CreateChildSceneNode();
            sceneNode.AttachObject(entity);
			sceneNode.Translate(new Vector3(120, 0, 0));
            sceneNode.Yaw(45);
			//sceneNode.Pitch(45);

			Entity ent = sceneManager.CreateEntity("ninja", "ninja.mesh");
			ent.CastShadows = true;
			sceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(ent);

			Plane plane = new Plane(Vector3.UnitY, 0);
			MeshManager.Instance.CreatePlane("ground", ResourceGroupManager.DefaultResourceGroupName, plane, 3500, 3500, 40, 40, true, 1, 5, 5, Vector3.UnitZ);

			ground = sceneManager.CreateEntity("GroundEntity", "ground");
			ground.MaterialName = "Examples/Grass";
			world = sceneManager.RootSceneNode.CreateChildSceneNode();
			world.AttachObject(ground);
			world.Translate(new Vector3(0, -75, 0));
        }


        protected virtual void OnFrameStarted (object source, FrameEventArgs evt)
		{
			try {
				if (!evt.StopRendering) {
					input.Capture ();
				}
			} catch (NullReferenceException e) {
				Console.WriteLine (e.Message);
				root.Shutdown ();
				root.Dispose ();
				return;
			}
			if (input.IsMousePressed (MouseButtons.Right)) {

			}
			if (input.IsKeyPressed (KeyCodes.Space)) {
				Console.WriteLine ("mouse button pressed");
				camera.Position = new Vector3 (camera.Position.x + input.RelativeMouseX, 
				                               camera.Position.y, camera.Position.z + input.RelativeMouseY);
			}
			if (input.IsKeyPressed (KeyCodes.A)) {
				camera.Position = new Vector3 (camera.Position.x + 5, 
											   camera.Position.y, camera.Position.z);
			} 
			if (input.IsKeyPressed (KeyCodes.D)) {
				camera.Position = new Vector3 (camera.Position.x - 5, 
											   camera.Position.y, camera.Position.z);
			} 
			if (input.IsKeyPressed (KeyCodes.W)) {
				camera.Position = new Vector3 (camera.Position.x, 
											   camera.Position.y, camera.Position.z + 5);
			} 
			if (input.IsKeyPressed (KeyCodes.S)) {
				camera.Position = new Vector3 (camera.Position.x, 
											   camera.Position.y, camera.Position.z - 5);
			}

			/*timeLast = timeNow;
			timeNow = root.Timer.Milliseconds;
			timeDelta = timeNow - timeLast;*/

			if (rotating) {
				/*sceneNode.Yaw(timeDelta * 0.02710f);
				sceneNode.Pitch(timeDelta * 0.070f);
				sceneNode.Roll (timeDelta * 0.056718f);*/
		
				light.Position = new Vector3 (300 * Math.Cos (root.Timer.Milliseconds / -2000.0), 100, 300 * Math.Sin (root.Timer.Milliseconds / -2000.0));
				//camera.Position = new Vector3 (500 * Math.Cos (root.Timer.Milliseconds / 5879.0), 500, 500 * Math.Sin (root.Timer.Milliseconds / 5879.0));
				//camera.SetAutoTracking(true, sceneNode);
			}
		}


        public static void Main (string[] args)
		{
			MainClass main = new MainClass ();
			main.init ();
			main.createScene ();
			Console.WriteLine ("start rendering...");
			main.root.StartRendering ();
        }
    }
}