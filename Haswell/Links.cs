using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    abstract class Links {
        Plot start;
        Plot end;
        ResourceType resource;
        float spd;

        public Links(Plot _start, Plot _end,ResourceType _resource,float _speed) {
            this.start = _start;
            this.end = _end;
            this.resource = _resource;
            this.spd = _speed;
        }
    }
}
