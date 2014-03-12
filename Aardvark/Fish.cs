using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark {
    class Fish:Animal {
        /// <summary>
        /// Create a default fish with the default parameters
        /// </summary>
        public Fish()
            : base("Fish", 1f, 1) {
            
        }
        /// <summary>
        /// Create a default fish with the given health
        /// </summary>
        /// <param name="health">The health of the fish to create</param>
        public Fish(float health)
            : base("Fish", health, 1) {

        }

        public Fish(string name, float health, uint rank) : base(name, health, rank) { }

        public override void Update() {
            base.Update();
        }
    }
}
