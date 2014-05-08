using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    /// <summary>
    /// Class ElementNotFoundException.
    /// </summary>
    [Serializable]
    public class ElementNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
        /// </summary>
        public ElementNotFoundException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ElementNotFoundException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ElementNotFoundException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected ElementNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
