using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Exceptions {
    [Serializable]
    public class PlotOutOfRangeException : Exception {
        public PlotOutOfRangeException() { }
        public PlotOutOfRangeException(string message) : base(message) { }
        public PlotOutOfRangeException(string message, Exception inner) : base(message, inner) { }
        protected PlotOutOfRangeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
