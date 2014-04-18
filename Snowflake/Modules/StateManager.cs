using System;
using System.Reflection;
using System.IO;

using Miyagi.Common;

namespace Snowflake.Modules
{
    /************************************************************************/
    /* state manager for program states                                     */
    /************************************************************************/
    public class StateManager
    {
        //////////////////////////////////////////////////////////////////////////
        private OgreManager mEngine;
        private MoisManager mInput;
        private MiyagiSystem mGuiSystem; //might make a wrapper for this later

        private Action _nextStateCallback;

        //////////////////////////////////////////////////////////////////////////
        private State mCurrentState;
        private Type mNewState;
        private bool mShutdown;

        public static bool SupressGameControl = false;

        // reference to the Ogre engine manager //////////////////////////////////
        public OgreManager Engine
        {
            get { return mEngine; }
        }

        // reference to the MOIS input manager ///////////////////////////////////
        public MoisManager Input
        {
            get { return mInput; }
        }

        public MiyagiSystem GuiSystem {
            get { return mGuiSystem; }
        }

        // flag to indicate that state manager is in shutdown state //////////////
        public bool ShuttingDown
        {
            get { return mShutdown; }
        }

        /************************************************************************/
        /* constructor                                                          */
        /************************************************************************/
        public StateManager(OgreManager _engine)
        {
            mEngine = _engine;
            mInput = new MoisManager();
            mGuiSystem = new MiyagiSystem("Mogre");
            mCurrentState = null;
            mNewState = null;
            mShutdown = false;
        }

        /************************************************************************/
        /* start up and initialize the first state                              */
        /************************************************************************/
        public bool Startup(Type _firstState)
        {
            // make sure that the state manager is not shutting down immediately
            mShutdown = false;

            // try to initialize the MOIS input manager
            if (!mInput.Startup(mEngine.WindowHandle, (int)mEngine.Window.Width, (int)mEngine.Window.Height))
                return false;

            //try to initialize the Miyagi Mogre plugin and then create relevant skin information things
            const string PluginPath = @".\";
            this.mGuiSystem.PluginManager.LoadPlugin(Path.Combine(PluginPath, "Miyagi.Plugin.Input.Mois.dll"), this.mInput.Keyboard, this.mInput.Mouse);

            ResourceManager.Create(mGuiSystem);

            // can't start up the state manager again if it's already running
            if (mCurrentState != null || mNewState != null)
                return false;

            // initialize with first state
            if (!RequestStateChange(_firstState))
                return false;

            // OK
            return true;
        }

        /************************************************************************/
        /* shut down                                                            */
        /************************************************************************/
        public void Shutdown()
        {
            // shutdown the MOIS input manager
            mInput.Shutdown();

            mGuiSystem.Dispose();

            // if a state is active, shut down the state to clean up
            if (mCurrentState != null)
                SwitchToNewState(null);

            // make sure any pending state change request is reset
            mNewState = null;
        }

        /************************************************************************/
        /* update                                                               */
        /************************************************************************/
        public void Update(float _frameTime)
        {
            // update the MOIS input manager
            mInput.Update();

            // check if a state change was requested
            if (mNewState != null)
            {
                State newState = null;

                // use reflection to get new state class default constructor
                ConstructorInfo constructor = mNewState.GetConstructor(Type.EmptyTypes);

                // try to create an object from the requested state class
                if (constructor != null)
                    newState = (State)constructor.Invoke(null);

                // switch to the new state if an object of the requested state class could be created
                if (newState != null)
                    SwitchToNewState(newState);

                // reset state change request until next state change is requested
                mNewState = null;

                // if the state has changed, clear all input states for this frame
                mInput.Clear();
            }

            //Update the GUI
            mGuiSystem.Update();

            // if a state is active, update the active state
            if (mCurrentState != null)
                mCurrentState.Update(_frameTime);
        }

        /************************************************************************/
        /* set next state that should be switched to, returns false if invalid  */
        /************************************************************************/
        public bool RequestStateChange(Type _newState, Action callback = null)
        {
            // new state class must be derived from base class "State"
            if (_newState == null || !_newState.IsSubclassOf(typeof(State)))
                return false;

            // don't change the state if the requested state class matches the current state
            if (mCurrentState != null && mCurrentState.GetType() == _newState)
                return false;

            if (callback != null) { _nextStateCallback = callback; }
            // store type of new state class to request a state change
            mNewState = _newState;

            // OK
            return true;
        }

        /************************************************************************/
        /* request to shutdown the application                                  */
        /************************************************************************/
        public void RequestShutdown()
        {
            // request the state manager to shut down and end the application
            mShutdown = true;
        }

        //////////////////////////////////////////////////////////////////////////
        // internal functions ////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        /************************************************************************/
        /* change from one state to another state                               */
        /************************************************************************/
        private void SwitchToNewState(State _newState)
        {
            // if a state is active, shut it down
            if (mCurrentState != null)
                mCurrentState.Shutdown();

            // switch to the new state, might be null if no new state should be activated
            mCurrentState = _newState;

            // if a state is active, start it up
            if (mCurrentState != null) {
                mCurrentState.Startup(this);
                if (_nextStateCallback != null) {
                    _nextStateCallback.Invoke();
                    _nextStateCallback = null;
                }
            }
        }

    } // class

} // namespace
