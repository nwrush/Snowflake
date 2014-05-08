using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Exceptions
{
    /// <summary>
    /// Class BuildingCreationFailedException.
    /// </summary>
    [Serializable]
    public class BuildingCreationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingCreationFailedException"/> class.
        /// </summary>
        public BuildingCreationFailedException() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingCreationFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BuildingCreationFailedException(string message) : base(message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingCreationFailedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public BuildingCreationFailedException(string message, Exception inner) : base(message, inner) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BuildingCreationFailedException"/> class.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <param name="context">The context.</param>
        protected BuildingCreationFailedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
