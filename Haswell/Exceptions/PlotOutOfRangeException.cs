using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Class PlotOutOfRangeException.
    /// </summary>
    [Serializable]
    public class PlotOutOfRangeException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotOutOfRangeException"/> class.
        /// </summary>
        public PlotOutOfRangeException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotOutOfRangeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public PlotOutOfRangeException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotOutOfRangeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public PlotOutOfRangeException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PlotOutOfRangeException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected PlotOutOfRangeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
