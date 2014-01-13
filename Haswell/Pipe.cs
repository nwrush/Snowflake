using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    struct Pipe{
        Plot start;
        Plot[] end;
        Resource type;
        float speed;

        public Pipe(Plot s, Plot e, Resource r, float spe) {
            this.start = s;
            this.end = new Plot[1];
            this.end[0] = e;
            this.type=r;
            this.speed=spe;
        }
        public Pipe(Plot s, Plot[] e, Resource r, float spe) {
            this.start = s;
            this.end = e;
            this.type = r;
            this.speed = spe;
        }
    }
}
