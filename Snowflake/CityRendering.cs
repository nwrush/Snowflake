using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
using Haswell;

using Resource = Haswell.Resource;

namespace Snowflake {
    /// <summary>
    /// Abstract class for a thing you can put in the city.
    /// </summary>
    public abstract class Renderable {

        //phase out later
        public const int PlotWidth = 120;
        public const int PlotHeight = 120;

        protected SceneNode node;
        protected List<Entity> entities;

        /// <summary>
        /// The name of this resource (and its child scene node)
        /// </summary>
        public string Name;
        public int PlotX { get; private set; }
        public int PlotY { get; private set; }

        /// <summary>
        /// Create the SceneNode and Entities associated with this renderable object.
        /// When overriding, create Entities first, then call base.Create();
        /// After that, you can manipulate the node as necessary.
        /// </summary>
        /// <param name="sm">Scenemanager to add the scenenode to</param>
        public virtual void Create(SceneManager sm) {
            node = sm.RootSceneNode.CreateChildSceneNode(this.Name);
            foreach (Entity e in this.entities) {
                node.AttachObject(e);
            }
        }

        /// <summary>
        /// Position this renderable on the city grid
        /// </summary>
        /// <param name="plotx">X Position in plot coords of this instance</param>
        /// <param name="ploty">Y Position in plot coords of this instance</param>
        public void SetPosition(int plotx, int ploty) {
            PlotX = plotx;
            PlotY = ploty;
            node.Translate(new Vector3(plotx * PlotWidth, 0, ploty * PlotHeight));
        }

        /// <summary>
        /// Gets this renderable's scene node
        /// </summary>
        /// <returns></returns>
        public SceneNode GetSceneNode() {
            return this.node;
        }
    }

    public class RenderableResource : Renderable {

        private Resource data;

        public RenderableResource(Resource data) {
            this.data = data;
        }

        public override void Create(SceneManager sm) {
            foreach (Entity e in GetResourceEntities(this.data)) { this.entities.Add(e); }
            base.Create(sm);
        }

        public static List<Entity> GetResourceEntities(Resource r) {
            throw new NotImplementedException();
        }
    }

    public class RenderableBuilding : Renderable {

        private Building data;

        public RenderableBuilding(Building data) {
            this.data = data;
        }

        public override void  Create(SceneManager sm) {
            foreach (Entity e in GetBuildingEntities(this.data, sm)) { this.entities.Add(e); }
            base.Create(sm);
            this.node.Scale(new Vector3(PlotWidth / 12.0f, 0, PlotHeight / 12.0f));
        }

        public static List<Entity> GetBuildingEntities(Building b, SceneManager sm) {
            List<Entity> entList = new List<Entity>();
            entList.Add(sm.CreateEntity(b.GetType().ToString() + "_" + b.GetHashCode(), "skyscraperBox001.mesh"));
            return entList;
        }
    }

    public class RenderablePipe : Renderable {

        private Pipe data;

        public RenderablePipe(Pipe data) {
            this.data = data;
        }

        public override void Create(SceneManager sm) {
            foreach (Entity e in GetPipeEntities(this.data)) { this.entities.Add(e); }
            base.Create(sm);
        }

        public static List<Entity> GetPipeEntities(Pipe b) {
            throw new NotImplementedException();
        }
    }
}
