using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Class Links.
    /// </summary>
    public class Links {
        /// <summary>
        /// The start
        /// </summary>
        Plot start;
        /// <summary>
        /// The end
        /// </summary>
        Plot end;
        /// <summary>
        /// The resource
        /// </summary>
        ResourceType resource;
        /// <summary>
        /// The speed
        /// </summary>
        float speed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Links"/> class.
        /// </summary>
        /// <param name="_start">The _start.</param>
        /// <param name="_end">The _end.</param>
        /// <param name="_resource">The _resource.</param>
        /// <param name="speed">The speed.</param>
        public Links(Plot _start, Plot _end, ResourceType _resource, LinkSpeed speed) {
            this.start = _start;
            this.end = _end;
            this.resource = _resource;
            this.speed = Links.GetSpeed(speed);
        }
        /// <summary>
        /// Gets the speed as a float the link will move resources
        /// </summary>
        /// <param name="speed">The LinkSpeed specified</param>
        /// <returns>The speed of the link as a float</returns>
        private static float GetSpeed(LinkSpeed speed) {
            switch (speed) {
                case (LinkSpeed.All):
                    return 1.0f;
                case (LinkSpeed.Fast):
                    return 0.75f;
                case (LinkSpeed.Medium):
                    return 0.5f;
                case (LinkSpeed.Slow):
                    return 0.25f;
                default:
                    return 0.0f;
            }
        }

        /// <summary>
        /// Moves the resources.
        /// </summary>
        public void MoveResources() {
            float resourcesToMove = this.start.Resources[resource] * this.speed;
            this.start.Resources[resource] -= resourcesToMove;
            this.end.Resources[resource] += resourcesToMove;
        }
    }
    //Todo: Enum for link types?
    /// <summary>
    /// Enum LinkSpeed
    /// </summary>
    public enum LinkSpeed {
        /// <summary>
        /// The slow
        /// </summary>
        Slow,
        /// <summary>
        /// The medium
        /// </summary>
        Medium,
        /// <summary>
        /// The fast
        /// </summary>
        Fast,
        /// <summary>
        /// All
        /// </summary>
        All
    };
}
