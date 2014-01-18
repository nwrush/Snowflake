using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre; 

namespace Snowflake {
    public abstract class CityRenderable {

        //phase out later
        public const int PlotWidth = 120;
        public const int PlotHeight = 120;

        public SceneNode node { get; private set; }
        private List<Entity> entities;

        public string Name;
        public int PlotX { get; private set; }
        public int PlotY { get; private set; }

        public virtual void Create(SceneManager sm) {
            node = sm.RootSceneNode.CreateChildSceneNode(this.Name);
            foreach (Entity e in this.entities) {
                node.AttachObject(e);
            }
        }

        public void SetPosition(int plotx, int ploty) {
            PlotX = plotx;
            PlotY = ploty;
            node
        }
    }

    
}
