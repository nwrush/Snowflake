using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    public class Commercial : Building {

        public Commercial():base(Zone.Type.Commercial) {

        }
        public Commercial(Dictionary<Resource.Type, int> r)
            : base(r, Zone.Type.Commercial) {

        }
    }
}
