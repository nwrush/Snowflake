using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark {
    class Bird : Animal {
        public Bird() : base("Bird", 1f, 3) { }
        public Bird(float health) : base("Bird", health, 3) { }
        public Bird(string name, float health, uint rank) : base(name, health, rank) { }

        public override void Update() {
            base.Update();
        }
    }
}
