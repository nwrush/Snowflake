﻿using System;

namespace Snowflake.Modules
{
    /************************************************************************/
    /* base class for program states                                        */
    /************************************************************************/
    public abstract class State
    {
        //////////////////////////////////////////////////////////////////////////

        /************************************************************************/
        /* constructor                                                          */
        /************************************************************************/
        public State()
        {
        }

        /************************************************************************/
        /* start up                                                             */
        /************************************************************************/
        public abstract bool Startup(StateManager _mgr);

        /************************************************************************/
        /* shut down                                                            */
        /************************************************************************/
        public abstract void Shutdown();

        /************************************************************************/
        /* update                                                               */
        /************************************************************************/
        public abstract void Update(long _frameTime);

    } // class

} // namespace