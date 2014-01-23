using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class BuildingEventArgs : EventArgs {

        public Building Building { get; private set; }
        public BuildingEventArgs(Building b) {
            this.Building = b;
        }
    }
}
