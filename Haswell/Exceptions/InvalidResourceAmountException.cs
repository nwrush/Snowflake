using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Exceptions
{
    /// <summary>
    /// Class InvalidResourceAmountException.
    /// </summary>
    [System.Serializable]
    public class InvalidResourceAmountException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceAmountException"/> class.
        /// </summary>
        public InvalidResourceAmountException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceAmountException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidResourceAmountException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceAmountException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public InvalidResourceAmountException(string message, System.Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidResourceAmountException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected InvalidResourceAmountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
