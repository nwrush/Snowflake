using System;

using Mogre;

using Snowflake.Modules;

namespace Snowflake.States
{
  /*************************************************************************************/
  /* program state for rendering the city (pretty comments courtesy of the quick start */
  /*************************************************************************************/
  public class City : State
  {
    //////////////////////////////////////////////////////////////////////////
    private StateManager mStateMgr;

    private Entity ground;
    private SceneNode world;

    private Light sun;
    private Light sky;

    public static float Time = 0.0f;

    /************************************************************************/
    /* constructor                                                          */
    /************************************************************************/
    public City()
    {
      mStateMgr = null;
      
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

        createScene(engine);

      // OK
      return true;
    }

    public void createScene(OgreManager engine) {

        engine.Camera.Position = new Vector3(0, 500, -500);
        engine.Camera.LookAt(new Vector3(0, 0, 0));
        engine.Camera.NearClipDistance = 5;
        engine.Camera.FarClipDistance = 2048;
        engine.Camera.AutoAspectRatio = true;

        sun = engine.SceneMgr.CreateLight("Sun");
        sun.Type = Light.LightTypes.LT_POINT;
        sun.Position = new Vector3(0, 1000, 100);
        sun.Direction = new Vector3(0, -1, 0.5f);
        sun.DiffuseColour = new ColourValue(0.98f, 0.95f, 0.9f);
        sun.SpecularColour = ColourValue.White;
        //sun.AttenuationQuadratic = 0.01f;
        //sun.AttenuationLinear = 0.1f;
        sun.CastShadows = true;

        sky = engine.SceneMgr.CreateLight("sky");
        sky.Type = Light.LightTypes.LT_DIRECTIONAL;
        sky.Position = new Vector3(0, 2000, 0);
        sky.Direction = new Vector3(0, -1, 0);
        sky.DiffuseColour = new ColourValue(0.05f, 0.075f, 0.10f);
        sky.SpecularColour = ColourValue.Black;
        sky.CastShadows = true;

        engine.SceneMgr.RootSceneNode.AttachObject(sun);
        engine.SceneMgr.RootSceneNode.AttachObject(sky);

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
