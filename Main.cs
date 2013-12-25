using System;
using Axiom;
using Axiom.Core;
using Axiom.Math;
using Axiom.Graphics;
using Axiom.Input;
using Axiom.Platforms.OpenTK;
using Axiom.Overlays;

namespace Snowflake {
	class MainClass {

		InputReader input;
		Camera camera;
		Viewport viewport;
		Light sun;

		SceneNode world;
		Entity ground;

		//long timeLast, timeNow, timeDelta;
		bool rotating = true;

		public void init() {
			GV.Root = new Root("test.log");

			GV.Root.RenderSystem = GV.Root.RenderSystems["OpenGL"];
			GV.Root.FrameStarted += OnFrameStarted;

			ResourceGroupManager.Instance.AddResourceLocation("media", "Folder");

			GV.Window = GV.Root.Initialize(false);
			GV.Window = GV.Root.CreateRenderWindow("Sustain", 800, 600, false);

			GV.PlatformManager = new OpenTKPlatformManager();
			GV.OverlayManager = OverlayManager.Instance;
			input = GV.PlatformManager.CreateInputReader();
			input.Initialize(GV.Window, true, true, false, false);

			TextureManager.Instance.DefaultMipmapCount = 5;
			ResourceGroupManager.Instance.InitializeAllResourceGroups();

			GV.SceneManager = GV.Root.CreateSceneManager(SceneType.Generic);

			camera = GV.SceneManager.CreateCamera("MainCamera");
			camera.Position = new Vector3(0, 500, -500);
			camera.LookAt(new Vector3(0, 0, 0));
			camera.Near = 5;
			camera.AutoAspectRatio = true;

			viewport = GV.Window.AddViewport(camera);
			viewport.BackgroundColor = ColorEx.Black;

			sun = GV.SceneManager.CreateLight("Sun");
			sun.Type = LightType.Point;
			sun.Position = new Vector3(0, 1000, 100);
			sun.Direction = new Vector3(0, -1, 0.5);
			sun.Diffuse = new ColorEx(0.98f, 0.95f, 0.9f);
			sun.Specular = ColorEx.White;
            //sun.AttenuationQuadratic = 0.01f;
            //sun.AttenuationLinear = 0.1f;
			sun.CastShadows = true;

			Light sky = GV.SceneManager.CreateLight ("sky");
			sky.Type = LightType.Directional;
			sky.Position = new Vector3(0, 2000, 0);
			sky.Direction = new Vector3(0, -1, 0);
			sky.Diffuse = new ColorEx(0.05f, 0.075f, 0.10f);
			sky.Specular = ColorEx.Black;
			sky.CastShadows = true;

			GV.SceneManager.AmbientLight = ColorEx.Black;
			//GV.SceneManager.ShadowTechnique = ShadowTechnique.StencilAdditive;
			//GV.SceneManager.ActiveCompositorChain
		}

		public void createScene ()
		{

			Plane plane = new Plane (Vector3.UnitY, 0);
			MeshManager.Instance.CreatePlane ("ground", ResourceGroupManager.DefaultResourceGroupName, plane, 3500, 3500, 40, 40, true, 1, 5, 5, Vector3.UnitZ);

			ground = GV.SceneManager.CreateEntity ("GroundEntity", "ground");
			ground.MaterialName = "Grass";
			world = GV.SceneManager.RootSceneNode.CreateChildSceneNode ();
			world.AttachObject (ground);
			world.Translate(new Vector3(0, -1, 0));

			for (int x = 0; x < 10; x++) {
				CityManager.Plots.Add (new Plot(x, x));
			}

			CityManager.CreateScene();
		}


		protected virtual void OnFrameStarted(object source, FrameEventArgs evt) {
			try {
				if (!evt.StopRendering) {
					input.Capture();
				}
			} catch (NullReferenceException e) {
				Console.WriteLine(e.Message);
				GV.Root.Shutdown();
				GV.Root.Dispose();
				return;
			}
			if (input.IsMousePressed(MouseButtons.Right)) {

			}
			if (input.IsKeyPressed(KeyCodes.Space) || input.IsMousePressed(MouseButtons.Right)) {
				//Console.WriteLine("mouse button pressed");
				camera.Position = new Vector3(camera.Position.x + input.RelativeMouseX,
											   camera.Position.y, camera.Position.z + input.RelativeMouseY);
			}
			if (input.IsKeyPressed(KeyCodes.A)) {
				camera.Position = new Vector3(camera.Position.x + 5,
											   camera.Position.y, camera.Position.z);
			}
			if (input.IsKeyPressed(KeyCodes.D)) {
				camera.Position = new Vector3(camera.Position.x - 5,
											   camera.Position.y, camera.Position.z);
			}
			if (input.IsKeyPressed(KeyCodes.W)) {
				camera.Position = new Vector3(camera.Position.x,
											   camera.Position.y, camera.Position.z + 5);
			}
			if (input.IsKeyPressed(KeyCodes.S)) {
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

				sun.Position = new Vector3(1000 * Math.Cos(GV.Root.Timer.Milliseconds / -2000.0), 1000 * Math.Sin(GV.Root.Timer.Milliseconds / -2000.0), -300);
				//sun.AttenuationQuadratic = Math.Max (0, sun.Position.y / 300.0f);
				//camera.Position = new Vector3 (500 * Math.Cos (GV.Root.Timer.Milliseconds / 5879.0), 500, 500 * Math.Sin (GV.Root.Timer.Milliseconds / 5879.0));
				//camera.SetAutoTracking(true, sceneNode);

			}

			CityManager.Update();
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