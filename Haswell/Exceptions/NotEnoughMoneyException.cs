using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Exceptions {
    [Serializable]
    public class NotEnoughMoneyException : Exception {
        public NotEnoughMoneyException() { }
        public NotEnoughMoneyException(string message) : base(message) { }
        public NotEnoughMoneyException(string message, Exception inner) : base(message, inner) { }
        protected NotEnoughMoneyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
