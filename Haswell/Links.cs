using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    /// <summary>
    /// Class Links.
    /// </summary>
    [Serializable]
    public class Links
    {
        Plot start;
        Plot end;
        ResourceType resource;

        public Links(Plot _start, Plot _end, ResourceType _resource)
        {
            this.start = _start;
            this.end = _end;
            this.resource = _resource;
        }


        /// <summary>
        /// Moves the resources.
        /// </summary>
        public void MoveResources()
        {

        }
    }
}
