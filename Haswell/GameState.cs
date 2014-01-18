using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class GameState {
        Plot[,] grid;
        Dictionary<Resource, ResourceVal> resourceDict;

        public GameState(long gameTime) {
            this.grid = Haswell.getActive().Grid.ToGrid();
        }
    }
}
