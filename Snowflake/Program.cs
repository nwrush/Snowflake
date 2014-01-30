using System;
using System.Diagnostics;

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

        public const int MAJOR_VERSION = 0;
        public const int MINOR_VERSION = 2;

        public const bool USE_FSAA = false;

        public const bool BOOT_INTO_GAME = false;

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
            if (mEngine.Startup() && mStateMgr.Startup((BOOT_INTO_GAME ? typeof(GameLoopState) : typeof(MenuState))))
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
        private float _frametime = 0;
        private Stopwatch _stopwatch;
        public void UpdateScene()
        {
            if (_stopwatch == null) { _stopwatch = new Stopwatch(); }
            _stopwatch.Start();
            // update the state manager, this will automatically update the active state
            mStateMgr.Update(_frametime);

            _stopwatch.Stop();
            _frametime = _stopwatch.ElapsedTicks / 500.0f;
            _stopwatch.Reset();
        }

        /************************************************************************/
        /*                                                                      */
        /************************************************************************/
        public void RemoveScene()
        {
        }

    } // class

} // namespace
