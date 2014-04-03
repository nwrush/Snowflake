using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    class Links {
        Plot start;
        Plot end;
        ResourceType resource;
        float speed;

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

        public void MoveResources() {
            float resourcesToMove = this.start.Resources[resource] * this.speed;
            this.start.Resources[resource] -= resourcesToMove;
            this.end.Resources[resource] += resourcesToMove;
        }
    }
    //Todo: Enum for link types?
    public enum LinkSpeed {
        Slow,
        Medium,
        Fast,
        All
    };
}
//3 edgey 5 me