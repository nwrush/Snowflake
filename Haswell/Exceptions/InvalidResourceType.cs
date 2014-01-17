using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    [Serializable]
    public class InvalidResourceTypeException : Exception {
        public InvalidResourceTypeException() { }
        public InvalidResourceTypeException(string message) : base(message) { }
        public InvalidResourceTypeException(string message, Exception inner) : base(message, inner) { }
        protected InvalidResourceTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
