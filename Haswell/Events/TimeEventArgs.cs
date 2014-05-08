using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    /// <summary>
    /// Class TimeEventArgs.
    /// </summary>
    public class TimeEventArgs : EventArgs
    {
        /// <summary>
        /// The current time
        /// </summary>
        DateTime CurrentTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeEventArgs"/> class.
        /// </summary>
        /// <param name="time">The time.</param>
        public TimeEventArgs(DateTime time)
        {
            CurrentTime = time;
        }

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time
        {
            get
            {
                return this.CurrentTime;
            }
        }
    }
}
