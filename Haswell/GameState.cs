using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class GameState {
        Plot[] grid;
        Dictionary<Resource, ResourceVal> resourceDict;

        public GameState(long gameTime) {
            grid = MakeGrid(null);
        }
        private static Plot[] MakeGrid(List<Plot> grid){
            try {
                Plot[] gird = null;
                foreach (Plot p in grid) {

                }
                return gird;
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            return null;
        }
    }
}
