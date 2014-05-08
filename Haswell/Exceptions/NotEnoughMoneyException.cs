using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Exceptions
{
    /// <summary>
    /// Class NotEnoughMoneyException.
    /// </summary>
    [Serializable]
    public class NotEnoughMoneyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotEnoughMoneyException"/> class.
        /// </summary>
        public NotEnoughMoneyException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NotEnoughMoneyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NotEnoughMoneyException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NotEnoughMoneyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public NotEnoughMoneyException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="NotEnoughMoneyException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected NotEnoughMoneyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
