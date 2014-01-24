using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Exceptions {
    [System.Serializable]
    public class InvalidResourceAmountException : System.Exception {
        public InvalidResourceAmountException() { }
        public InvalidResourceAmountException(string message) : base(message) { }
        public InvalidResourceAmountException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidResourceAmountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
