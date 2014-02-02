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
        protected Vector3 scale;

        public event EventHandler Selected;

        private bool isGhosted;
        private bool isVisible;
        private bool isVirtual;
        /// <summary>
        /// Whether or not this is a virtual renderable that has yet to be placed on the city grid.
        /// </summary>
        public bool IsVirtual {
            get { return this.isVirtual; }
            set {
                this.isVirtual = value;
                if (value) {
                    foreach (Entity e in this.entities) {
                        e.SetMaterialName("cursor_01_-_Default");
                    }
                }
                else {
                    foreach (Entity e in this.entities) {
                        e.SetMaterialName("");
                    }
                }
            }
        }

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
        /// /// <param name="cityNode">City Node to add a child to</param>
        public virtual void Create(SceneManager sm, SceneNode cityNode) {
            node = cityNode.CreateChildSceneNode(this.Name);
            foreach (Entity e in this.entities) {
                node.AttachObject(e);
            }
            node.Scale(this.scale);
            isVisible = true;
        }

        /// <summary>
        /// Position this renderable on the city grid
        /// </summary>
        /// <param name="plotx">X Position in plot coords of this instance</param>
        /// <param name="ploty">Y Position in plot coords of this instance</param>
        public void SetPosition(int plotx, int ploty) {
            PlotX = plotx;
            PlotY = ploty;
            Vector3 plotcenter = CityManager.GetPlotCenter(plotx, ploty);
            if (node != null) { node.SetPosition(plotcenter.x, plotcenter.y, plotcenter.z); }
        }

        public void Place() {
            this.IsVirtual = false;
            this.Unghost();
        }

        /// <summary>
        /// Gets this renderable's scene node
        /// </summary>
        /// <returns></returns>
        public SceneNode GetSceneNode() {
            return this.node;
        }
        /// <summary>
        /// Sets the visibility of this object
        /// </summary>
        public bool Visible {
            get { return this.isVisible; }
            set { this.node.SetVisible(value); this.isVisible = value; }
        }
        /// <summary>
        /// Makes this object visible
        /// </summary>
        public void Show() {
            this.Visible = true;
        }
        /// <summary>
        /// Makes this object inivisble
        /// </summary>
        public void Hide() {
            this.Visible = false;
        }

        /// <summary>
        /// Make this object transparent
        /// Warning: assumes the Diffuse value has no opacity set!
        /// </summary>
        public void Ghost() {
            if (!isGhosted) {
                foreach (Entity e in this.entities) {
                    MaterialPtr eMat = e.GetSubEntity(0).GetMaterial();
                    eMat.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
                    eMat.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;
                    //ColourValue c = eMat.GetTechnique(0).GetPass(0).Diffuse;
                    //eMat.GetTechnique(0).GetPass(0).SetDiffuse(c.r, c.g, c.b, 0.5f);
                    e.GetSubEntity(0).SetMaterial(eMat);
                }
                isGhosted = true;
            }
        }
        /// <summary>
        /// Make this object opaque
        /// Warning: assumes the Diffuse value has no opacity set!
        /// </summary>
        public void Unghost() {
            if (isGhosted) {
                foreach (Entity e in this.entities) {
                    MaterialPtr eMat = e.GetSubEntity(0).GetMaterial();
                    eMat.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendType.SBT_REPLACE);
                    eMat.GetTechnique(0).GetPass(0).DepthWriteEnabled = true;
                    //ColourValue c = eMat.GetTechnique(0).GetPass(0).Diffuse;
                    //eMat.GetTechnique(0).GetPass(0).SetDiffuse(c.r, c.g, c.b, 1.0f);
                    e.GetSubEntity(0).SetMaterial(eMat);
                }
                isGhosted = false;
            }
        }

        /// <summary>
        /// Update me
        /// </summary>
        public virtual void Update() {
        }

        public void Select() {
            Selected.Invoke(this, new EventArgs());
        }

        public virtual void Dispose() {
            foreach (Entity e in entities) {
                e.Dispose();
            }
            entities.Clear();
            node.Dispose();
            node = null;
        }
    }

    public class RenderableResource : Renderable {

        private Resource data;

        public RenderableResource(Resource data) {
            this.data = data;
        }

        public override void Create(SceneManager sm, SceneNode cityNode) {
            foreach (Entity e in GetResourceEntities(this.data)) { this.entities.Add(e); }
            base.Create(sm, cityNode);
        }

        public static List<Entity> GetResourceEntities(Resource r) {
            throw new NotImplementedException();
        }
    }

    public class RenderableBuilding : Renderable {

        private Building data;

        public RenderableBuilding(Building data) {
            this.data = data;
            this.entities = new List<Entity>();
            this.Name = this.data.GetType().ToString() + "_" + this.GetHashCode();
        }

        public override void Create(SceneManager sm, SceneNode cityNode) {
            foreach (Entity e in GetBuildingEntities(this.data, sm, out this.scale)) { this.entities.Add(e); }
            base.Create(sm, cityNode);
            if (this.data.Parent != null) { this.SetPosition(this.data.Parent.X, this.data.Parent.Y); }
        }

        public static List<Entity> GetBuildingEntities(Building b, SceneManager sm, out Vector3 scale) {
            List<Entity> entList = new List<Entity>();
            scale = new Vector3(1, 1, 1);
;            if (b is Haswell.Buildings.Commercial) {
                entList.Add(sm.CreateEntity(b.GetType().ToString() + "_" + b.GetHashCode(), "skyscraper1.mesh"));
                scale = new Vector3(80.0f, 80.0f, 80.0f);
            }
            else if (b is Haswell.Buildings.Residential) {
                entList.Add(sm.CreateEntity(b.GetType().ToString() + "_" + b.GetHashCode(), "residential1.mesh"));
                scale = new Vector3(20.0f, 20.0f, 20.0f);
            }
            return entList;
        }

        /// <summary>
        /// Returns the Haswell object providing this renderable's data.
        /// </summary>
        /// <returns>The Haswell object providing this renderable's data.</returns>
        public Building GetData() {
            return this.data;
        }
    }

    public class RenderablePipe : Renderable {

        private Pipe data;

        public RenderablePipe(Pipe data) {
            this.data = data;
        }

        public override void Create(SceneManager sm, SceneNode cityNode) {
            foreach (Entity e in GetPipeEntities(this.data)) { this.entities.Add(e); }
            base.Create(sm, cityNode);
        }

        public static List<Entity> GetPipeEntities(Pipe b) {
            throw new NotImplementedException();
        }
    }
}
