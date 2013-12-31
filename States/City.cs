using System;

using Mogre;

using Snowflake.Modules;
using Snowflake.Buildings;

namespace Snowflake.States
{
  /*************************************************************************************/
  /* program state for rendering the city (pretty comments courtesy of the quick start */
  /*************************************************************************************/
  public class City : State
  {
    //////////////////////////////////////////////////////////////////////////
    private StateManager mStateMgr;
    private WeatherManager mWeatherMgr;

    private Entity ground;
    private SceneNode world;

    public static float Time = 0.0f;

    /************************************************************************/
    /* constructor                                                          */
    /************************************************************************/
    public City()
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

      // OK
      return true;
    }

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
        
        if (mStateMgr.Input.IsKeyDown(MOIS.KeyCode.KC_SPACE) || mStateMgr.Input.IsMouseButtonDown(MOIS.MouseButtonID.MB_Right)) {
            //Console.WriteLine("mouse button pressed");
            engine.Camera.Position = new Vector3(engine.Camera.Position.x + mStateMgr.Input.MouseMoveX,
                                           engine.Camera.Position.y, engine.Camera.Position.z + mStateMgr.Input.MouseMoveY);
        }
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

        Time += 0.1f;
        sun.Position = new Vector3(1000 * (float)System.Math.Cos(Time / -20.0), 1000 * (float)System.Math.Sin(Time / -20.0), -300);

        CityManager.Update();

        // check if the escape key was pressed
        if (mStateMgr.Input.WasKeyPressed(MOIS.KeyCode.KC_ESCAPE)) {
            // quit the application
            mStateMgr.RequestShutdown();
        }
    }

  } // class

} // namespace
