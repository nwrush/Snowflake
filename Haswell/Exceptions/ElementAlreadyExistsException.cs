using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    [Serializable]
    public class ElementAlreadyExistsException : Exception {
        public ElementAlreadyExistsException() { }
        public ElementAlreadyExistsException(string message) : base(message) { }
        public ElementAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        protected ElementAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
