using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    /// <summary>
    /// Class ElementAlreadyExistsException.
    /// </summary>
    [Serializable]
    public class ElementAlreadyExistsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAlreadyExistsException"/> class.
        /// </summary>
        public ElementAlreadyExistsException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ElementAlreadyExistsException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public ElementAlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected ElementAlreadyExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
