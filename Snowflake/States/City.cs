using System;

using Mogre;

using Snowflake.Modules;
using Snowflake.Buildings;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

using Vector3 = Mogre.Vector3;

namespace Snowflake.States
{
  /*************************************************************************************/
  /* program state for rendering the city (pretty comments courtesy of the quick start */
  /*************************************************************************************/
  public class CityState : State
  {
    //////////////////////////////////////////////////////////////////////////
    private StateManager mStateMgr;
    private WeatherManager mWeatherMgr;

    private Entity ground;
    private SceneNode world;

    private Modules.GameConsole GameConsole;

    public static float Time = 0.0f;

    /************************************************************************/
    /* constructor                                                          */
    /************************************************************************/
    public CityState()
    {
      mStateMgr = null;
      mWeatherMgr = null;
    }

    /************************************************************************/
    /* start up                                                             */
    /************************************************************************/
    public override bool Startup( StateManager _mgr )
    {
        // store reference to the state manager
        mStateMgr = _mgr;

        // get reference to the ogre manager
        OgreManager engine = mStateMgr.Engine;

        mWeatherMgr = new WeatherManager();

        createScene(engine);
        createUI();

      // OK
      return true;
    }

    //Set up camera and world meshes (consider moving to separate class as per Nikko's advice?)
    public void createScene(OgreManager engine) {

        engine.SceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;

        engine.Camera.Position = new Vector3(0, 500, -500);
        engine.Camera.LookAt(new Vector3(0, 0, 0));
        engine.Camera.NearClipDistance = 5;
        engine.Camera.FarClipDistance = 2048;
        engine.Camera.AutoAspectRatio = true;

        mWeatherMgr.CreateScene(engine.SceneMgr);

        Plane plane = new Plane(Vector3.UNIT_Y, 0);
        MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 3500, 3500, 40, 40, true, 1, 5, 5, Vector3.UNIT_Z);

        ground = engine.SceneMgr.CreateEntity("GroundEntity", "ground");
        ground.SetMaterialName("Grass");
        world = engine.SceneMgr.RootSceneNode.CreateChildSceneNode();
        world.AttachObject(ground);
        world.Translate(new Vector3(0, -1, 0));

        for (int x = 0; x < 10; x++) {
            CityManager.Plots.Add(new Plot(x, x));
        }

        //test custom model
        Plot p = new Plot(1, 3, true);
        p.AddBuilding(new ParkBuilding());
        CityManager.Plots.Add(p);

        CityManager.CreateScene(engine.SceneMgr);

        GameConsole = new Modules.GameConsole();
    }

    //Set up overlays for user interface
    public void createUI() {
        GameConsole.CreateGui(this.mStateMgr.GuiSystem);
    }

    /************************************************************************/
    /* shut down                                                            */
    /************************************************************************/
    public override void Shutdown()
    {
      
    }

    /************************************************************************/
    /* update                                                               */
    /************************************************************************/
    public override void Update( long _frameTime )
    {
        // check if the state was initialized before
        if (mStateMgr == null)
            return;

        // get reference to the ogre manager
        OgreManager engine = mStateMgr.Engine;

        //If we're not typing into a form or something...
        if (!StateManager.SupressGameControl) {
            //Mouse drag control
            if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_SPACE) || mStateMgr.Input.IsMouseButtonDown(MOIS.MouseButtonID.MB_Right)) {
                //Console.WriteLine("mouse button pressed");
                engine.Camera.Position = new Vector3(engine.Camera.Position.x + mStateMgr.Input.MouseMoveX,
                                               engine.Camera.Position.y, engine.Camera.Position.z + mStateMgr.Input.MouseMoveY);
                mStateMgr.GuiSystem.GUIManager.Cursor.SetActiveMode(CursorMode.ResizeTop);
            }
            //Mouse click - 3D selection
            if (mStateMgr.Input.IsMouseButtonDown(MOIS.MouseButtonID.MB_Left)) {
                GetSelectionOrigin(new Point(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY), mStateMgr.Engine.Camera);
            }

            //WASD Control
            if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_A)) {
                engine.Camera.Position = new Vector3(engine.Camera.Position.x + 5,
                                               engine.Camera.Position.y, engine.Camera.Position.z);
            }
            if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_D)) {
                engine.Camera.Position = new Vector3(engine.Camera.Position.x - 5,
                                               engine.Camera.Position.y, engine.Camera.Position.z);
            }
            if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_W)) {
                engine.Camera.Position = new Vector3(engine.Camera.Position.x,
                                               engine.Camera.Position.y, engine.Camera.Position.z + 5);
            }
            if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_S)) {
                engine.Camera.Position = new Vector3(engine.Camera.Position.x,
                                               engine.Camera.Position.y, engine.Camera.Position.z - 5);
            }

            //Toggle the console with `
            if (mStateMgr.Input.WasKeyPressed(MOIS.KeyCode.KC_GRAVE)) {
                GameConsole.Visible = !GameConsole.Visible;
            }
        }

        CityManager.Update();
        mWeatherMgr.Update();

        // check if the escape key was pressed
        if (mStateMgr.Input.WasKeyPressed(MOIS.KeyCode.KC_ESCAPE)) {
            // quit the application
            mStateMgr.RequestShutdown();
        }
    }

    //returns the Point of selection in 3D space, given the 2D click point on the screen and a reference frustum
    public Vector3 GetSelectionOrigin(Point p, Frustum cam) {
        //Store w and h because long variable names
        int w = mStateMgr.Engine.Window.GetViewport(0).ActualWidth;
        int h = mStateMgr.Engine.Window.GetViewport(0).ActualHeight;
        
        //get p relative to center of screen, as a number from -1 to 1
        PointF rel = new PointF((p.X - (int)(w * 0.5)) / (w * 0.5f), (p.Y - (int)(h * 0.5)) / (h * 0.5f));
    }

  } // class

} // namespace
