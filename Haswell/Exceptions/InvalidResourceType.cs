using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Class InvalidResourceTypeException.
    /// </summary>
    [Serializable]
    public class InvalidResourceTypeException : Exception {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceTypeException"/> class.
        /// </summary>
        public InvalidResourceTypeException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceTypeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidResourceTypeException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceTypeException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidResourceTypeException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceTypeException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected InvalidResourceTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
