using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    public class TimeEventArgs : EventArgs
    { 
        DateTime CurrentTime;

        public TimeEventArgs(DateTime time) {
            CurrentTime = time;
        }
    }
}
