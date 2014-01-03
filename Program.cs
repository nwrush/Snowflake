using System;

using Mogre;

using Snowflake.Modules;
using Snowflake.States;

namespace Snowflake
{
    public class Program
    {
        //////////////////////////////////////////////////////////////////////////s
        private static OgreManager mEngine;
        private static StateManager mStateMgr;

        //temp window width and height variables - figure out a way to dynamically set with a config menu
        public const int WINDOW_WIDTH = 1366;
        public const int WINDOW_HEIGHT = 768;

        /************************************************************************/
        /* program starts here                                                  */
        /************************************************************************/
        [STAThread]
        static void Main()
        {
            // create Ogre manager
            mEngine = new OgreManager();

            // create state manager
            mStateMgr = new StateManager(mEngine);

            // create main program
            Program prg = new Program();

            // try to initialize Ogre and the state manager
            if (mEngine.Startup() && mStateMgr.Startup(typeof(City)))
            {
                // create objects in scene
                prg.CreateScene();

                // run engine main loop until the window is closed
                while (!mEngine.Window.IsClosed)
                {
                    // update the objects in the scene
                    prg.UpdateScene();

                    // update Ogre and render the current frame
                    mEngine.Update();

                    // check if state manager is in shutting down state
                    if (mStateMgr.ShuttingDown)
                    {
                        // destroy the window to shut down the application
                        mEngine.Window.Destroy();
                    }
                }

                // remove objects from scene
                prg.RemoveScene();
            }

            // shut down state manager
            mStateMgr.Shutdown();

            // shutdown Ogre
            mEngine.Shutdown();
        }

        /************************************************************************/
        /* constructor                                                          */
        /************************************************************************/
        public Program()
        {
        }

        /************************************************************************/
        /* create a scene to render                                             */
        /************************************************************************/
        public void CreateScene()
        {
            // set a dark ambient light
            mEngine.SceneMgr.AmbientLight = new ColourValue(0.1f, 0.1f, 0.1f);
        }

        /************************************************************************/
        /* update objects in the scene                                          */
        /************************************************************************/
        public void UpdateScene()
        {
            // update the state manager, this will automatically update the active state
            mStateMgr.Update(0);
        }

        /************************************************************************/
        /*                                                                      */
        /************************************************************************/
        public void RemoveScene()
        {
        }

    } // class

} // namespace
