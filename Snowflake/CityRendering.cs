using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
using Haswell;
using Haswell.Buildings;

using Resource = Haswell.Resource;

namespace Snowflake {
    /// <summary>
    /// Abstract class for a thing you can put in the city.
    /// </summary>
    public abstract class Renderable {

        //phase out later
        public const int PlotWidth = 120;
        public const int PlotHeight = 120;

        internal SceneNode node;
        internal List<Entity> entities;
        internal Vector3 scale;
        internal Quaternion rotation;
        protected Quaternion baseRotation;

        public event EventHandler Selected;
        public event EventHandler Deselected;

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
                        e.SetMaterialName("Ghosting");
                        e.CastShadows = false;
                    }
                }
                else {
                    foreach (Entity e in this.entities) {
                        e.SetMaterialName("Default");
                        e.CastShadows = true;
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

        public Renderable()
        {
            baseRotation = rotation = Quaternion.IDENTITY;
            scale = Vector3.UNIT_SCALE;
        }
        /// <summary>
        /// Create the SceneNode and Entities associated with this renderable object.
        /// When overriding, create Entities first, then call base.Create();
        /// After that, you can manipulate the node as necessary.
        /// </summary>
        /// <param name="sm">Scenemanager to add the scenenode to</param>
        /// /// <param name="cityNode">City Node to add a child to</param>
        public virtual void Create(SceneManager sm, SceneNode cityNode) {
            node = cityNode.CreateChildSceneNode();
            foreach (Entity e in this.entities) {
                node.AttachObject(e);
            }
            node.SetScale(this.scale);
            node.Orientation = this.rotation;
            isVisible = true;
        }

        /// <summary>
        /// Position this renderable on the city grid
        /// </summary>
        /// <param name="plotx">X Position in plot coords of this instance</param>
        /// <param name="ploty">Y Position in plot coords of this instance</param>
        public virtual void SetPosition(int plotx, int ploty) {
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
                    e.CastShadows = false;
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
                    e.CastShadows = true;
                }
                isGhosted = false;
            }
        }

        /// <summary>
        /// Update me
        /// </summary>
        public virtual void Update() {
        }

        public virtual void Select() {
            if (Selected != null) { Selected.Invoke(this, new EventArgs()); }
        }
        public virtual void Deselect() {
            if (Deselected != null) { Deselected.Invoke(this, new EventArgs()); }
        }

        public virtual void Dispose() {
            if (node != null)
            {
                node.ParentSceneNode.RemoveChild(node);
                node.RemoveAndDestroyAllChildren();
                ClearEntities();
                node.Dispose();
                node = null;
            }
        }

        protected virtual void ClearEntities()
        {
            foreach (Entity ent in entities)
            {
                ent.DetachFromParent();
                ent.Dispose();
            }
            entities.Clear();
        }
    }

    public class RenderableResource : Renderable {

        private Resource data;

        public RenderableResource(Resource data) : base() {
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

    public class RenderablePlot : Renderable
    {
        private Plot data;
        private SceneNode selectionBoxNode;
        private SceneNode zoneNode;

        public event EventHandler ZoneChanged;
        public event EventHandler<BuildingEventArgs> BuildingDeleted;
        public event EventHandler<BuildingEventArgs> BuildingAdded;

        public RenderablePlot(Plot data) : base()
        {
            this.data = data;
            this.entities = new List<Entity>();
            this.Name = this.data.GetType().ToString() + "_" + this.GetHashCode();

            data.ZoneChanged += this.OnZoneChanged;
            data.BuildingDeleted += this.OnBuildingDeleted;
            data.BuildingAdded += this.OnBuildingAdded;
        }

        public override void Create(SceneManager sm, SceneNode cityNode)
        {            
            base.Create(sm, cityNode);

            if (this.RenderableBuilding != null) { this.RenderableBuilding.Create(sm, cityNode); }

            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane(this.Name + "_zoneTile", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, PlotWidth, PlotHeight, 1, 1, true, 1, 1, 1, Vector3.UNIT_Z);
            Entity zoneTile = sm.CreateEntity(this.Name, this.Name + "_zoneTile");

            zoneNode = cityNode.CreateChildSceneNode();
            zoneNode.AttachObject(zoneTile);
            
            zoneTile.CastShadows = false;
            zoneNode.Translate(new Vector3(0, 100, 0));
            zoneNode.SetVisible(false);

            selectionBoxNode = cityNode.CreateChildSceneNode();
            Entity selectionBoxEnt = sm.CreateEntity("selectionbox.mesh");

            selectionBoxNode.AttachObject(selectionBoxEnt);
            selectionBoxNode.SetScale(new Vector3(473.0f, 473.0f, 473.0f));
            selectionBoxEnt.CastShadows = false;
            selectionBoxNode.SetVisible(false);

            this.SetPosition(data.X, data.Y);

            if (this.Data.Zone != Zones.Unzoned) { this.OnZoneChanged(this.Data, new EventArgs()); }
        }

        public override void SetPosition(int plotx, int ploty)
        {
            base.SetPosition(plotx, ploty);
            Vector3 plotcenter = CityManager.GetPlotCenter(plotx, ploty);
            if (zoneNode != null) { zoneNode.SetPosition(plotcenter.x, plotcenter.y + 1, plotcenter.z); }
            if (selectionBoxNode != null) { selectionBoxNode.SetPosition(plotcenter.x, plotcenter.y, plotcenter.z); }
        }

        public override void Update()
        {
            base.Update();
            if (this.RenderableBuilding != null) { this.RenderableBuilding.Update(); }
            if ((CityManager.ShowZones || CityManager.GetMouseMode() == States.MouseMode.DrawingZone) && this.data.Zone != Zones.Unzoned)
            {
                zoneNode.SetVisible(true);
            }
            else
            {
                zoneNode.SetVisible(false);
            }
        }

        private void OnZoneChanged(object sender, EventArgs e)
        {
            if (this.data.Zone == Zones.Unzoned) { zoneNode.SetVisible(false); }
            else { 

                ((Entity)zoneNode.GetAttachedObject(0)).GetSubEntity(0).SetMaterial(
                    GetZoneColoredMaterial(
                    ((Entity)zoneNode
                    .GetAttachedObject(0))
                    .GetSubEntity(0)
                    .GetMaterial(),
                    this.data.Zone));

                zoneNode.SetVisible(true);
            }

            if (this.ZoneChanged != null)
            {
                this.ZoneChanged.Invoke(sender, e);
            }
        }

        public static MaterialPtr GetZoneColoredMaterial(MaterialPtr eMat, Zones z)
        {
            //Todo: Update this to index each zone's material so that they don't have to each create their own.
            eMat = eMat.Clone(eMat.Name + "_zone_" + DateTime.Now.ToString());
            eMat.GetTechnique(0).GetPass(0).SetSceneBlending(SceneBlendType.SBT_TRANSPARENT_ALPHA);
            eMat.GetTechnique(0).GetPass(0).DepthWriteEnabled = false;
            //Switch based on zone type
            switch (z)
            {
                case Zones.Residential:
                    eMat.GetTechnique(0).GetPass(0).SetDiffuse(1.0f, 0.5f, 0.2f, 0.5f);
                    break;
                case Zones.Industrial:
                    eMat.GetTechnique(0).GetPass(0).SetDiffuse(0.5f, 0.5f, 0.5f, 0.5f);
                    break;
                case Zones.Infrastructure:
                    eMat.GetTechnique(0).GetPass(0).SetDiffuse(0.2f, 0.2f, 1.0f, 0.5f);
                    break;
                case Zones.Conservation:
                    eMat.GetTechnique(0).GetPass(0).SetDiffuse(0.2f, 1.0f, 0.2f, 0.5f);
                    break;
                default:
                    eMat.GetTechnique(0).GetPass(0).SetDiffuse(1.0f, 2.0f, 1.0f, 0.5f);
                    break;
            }
            return eMat;
        }

        private void OnBuildingAdded(object sender, BuildingEventArgs e)
        {
            if (this.BuildingAdded != null)
            {
                this.BuildingAdded.Invoke(sender, e);
            }
        }

        private void OnBuildingDeleted(object sender, BuildingEventArgs e)
        {
            this.Deselect();
            if (this.BuildingDeleted != null)
            {
                this.BuildingDeleted.Invoke(sender, e);
            }
        }

        public Plot Data 
        {
            get { return this.data; }
        }

        public RenderableBuilding RenderableBuilding
        {
            get
            {
                if (this.data.Building != null && CityManager.Buildings.ContainsKey(this.data.Building))
                {
                    return CityManager.Buildings[this.data.Building];
                }
                else { return null; }
            }
        }

        public override void Select()
        {
            base.Select();
            this.selectionBoxNode.SetVisible(true);
        }

        public override void Deselect()
        {
            base.Deselect();
            this.selectionBoxNode.SetVisible(false);
        }
    }

    public class RenderableBuilding : Renderable {

        private Building data;

        public event EventHandler Deleted;
		public RenderableBuilding(BuildingConfiguration template) : base() {
			BuildingType bt = template.BuildingType;
			switch (bt) {
				case BuildingType.Residential:
				this.data = new Residential (template);
				break;
				case BuildingType.Commercial:
				this.data = new Commercial (template);
				break;
				case BuildingType.Industrial:
				this.data = new Industrial (template);
				break;
				case BuildingType.Infrastructure:
				this.data = new Infrastructure (template);
				break;
			}

			this.entities = new List<Entity>();
			this.Name = this.data.GetType().ToString() + "_" + this.GetHashCode();

			data.Deleted += this.OnBuildingDeleted;
		}
        public RenderableBuilding(Building data) : base() {
            this.data = data;
            this.entities = new List<Entity>();
            this.Name = this.data.GetType().ToString() + "_" + this.GetHashCode();

            data.Deleted += this.OnBuildingDeleted;
        }

        public override void Create(SceneManager sm, SceneNode baseNode) {
            foreach (Entity e in GetBuildingEntities(sm, out this.scale, out this.baseRotation)) { this.entities.Add(e); }
            base.Create(sm, baseNode);

            if (Data.Parent != null) { this.SetPosition(Data.Parent.X, Data.Parent.Y); }
            if (Data is Road) { node.Translate(0, 2, 0); }
        }

        public virtual List<Entity> GetBuildingEntities(SceneManager sm, out Vector3 scale, out Quaternion rotation) {
            List<Entity> entList = new List<Entity>();
            scale = new Vector3(1, 1, 1);
            rotation = Quaternion.IDENTITY;
            if (this.Data is Haswell.Buildings.Commercial) {
                entList.Add(sm.CreateEntity("skyscraper1.mesh"));
                scale = new Vector3(80.0f, 80.0f, 80.0f);
            }
            else if (this.Data is Haswell.Buildings.Residential) {
                entList.Add(sm.CreateEntity(this.Name + "_" + this.GetHashCode() + "_house", "residential_house.mesh"));
                scale = new Vector3(15.0f, 15.0f, 15.0f);
                rotation = Vector3.UNIT_Z.GetRotationTo(Vector3.UNIT_X);
            }
            else if (this.Data is Haswell.Buildings.Industrial)
            {
                entList.Add(sm.CreateEntity("powerplant_coal.mesh"));
                scale = new Vector3(34.0f, 34.0f, 34.0f);
            }
            else if (this.Data is Haswell.Buildings.Infrastructure)
            {
                entList.Add(sm.CreateEntity("infrastructure_townhall.mesh"));
                scale = new Vector3(34.0f, 34.0f, 34.0f);
                baseRotation = new Quaternion(Mogre.Math.PI, Vector3.UNIT_Y);
            }
            return entList;
        }

        public override void Update()
        {
            base.Update();

            Mogre.Degree ang = new Mogre.Degree((int)this.data.Facing);
            this.node.Orientation = baseRotation * new Quaternion(ang, Vector3.UNIT_Y);
        }

        /// <summary>
        /// Returns the Haswell object providing this renderable's data.
        /// </summary>
        /// <returns>The Haswell object providing this renderable's data.</returns>
        public Building Data {
            get { return this.data; }
        }

        private void OnBuildingDeleted(object sender, EventArgs e)
        {
            this.Deselect();
            this.Dispose();
            Building b = this.data;
            this.data = null;
            if (this.Deleted != null)
            {
                this.Deleted.Invoke(sender, new EventArgs());
            }
            CityManager.Buildings.Remove(b);
        }
    }

    public class RenderableRoad : RenderableBuilding
    {
        private Entity straight;
        private Entity turn;
        private Entity tBend;
        private Entity fourWay;

        private int _dirty;

        public RenderableRoad(Road data) : base(data) { 
            data.Parent.AdjacentBuildingChanged += (object sender, BuildingEventArgs e) => {
                _dirty = 4;
            };
            data.WeeklyUpdate += (object sender, EventArgs e) =>
            {
                _dirty = 1;
            };
        }

        public override void Create(SceneManager sm, SceneNode baseNode)
        {
            this.entities = GetBuildingEntities(sm, out this.scale, out this.baseRotation);
            base.Create(sm, baseNode);

            UpdateModel();
        }
        public override List<Entity> GetBuildingEntities(SceneManager sm, out Vector3 scale, out Quaternion rotation)
        {
            List<Entity> entList = new List<Entity>();
            scale = new Vector3(1, 1, 1);
            rotation = Quaternion.IDENTITY;
            
            Dictionary<Direction, Building> neighbors = new Dictionary<Direction, Building>();
            neighbors = this.Data.GetAdjacentBuildings();
            
            fourWay = sm.CreateEntity("road_4-way.mesh");
            fourWay.CastShadows = false;
            fourWay.Visible = false;
            entList.Add(fourWay);
            
            tBend = sm.CreateEntity("road_t-bend.mesh");
            tBend.CastShadows = false;
            tBend.Visible = false;
            entList.Add(tBend);

            turn = sm.CreateEntity("road_curve.mesh");
            turn.CastShadows = false;
            turn.Visible = false;
            entList.Add(turn);

            straight = sm.CreateEntity("road_straight.mesh");
            straight.CastShadows = false;
            straight.Visible = false;
            entList.Add(straight);

            scale = new Vector3(34.0f, 34.0f, 34.0f);

            return entList;
        }

        public override void Update()
        {
            base.Update();

            if (_dirty == 0)
            {
                UpdateModel();
                _dirty = -1;
            }
            else if (_dirty > 0)
            {
                _dirty -= 1;
            }
        }

        public void UpdateModel()
        {
            Dictionary<Direction, Building> neighbors = new Dictionary<Direction, Building>();
            neighbors = this.Data.GetAdjacentBuildings();

            if (neighbors.Count == 4)
            {
                fourWay.Visible = true;
                tBend.Visible = straight.Visible = turn.Visible = false;
            }
            else if (neighbors.Count == 3)
            {
                tBend.Visible = true;
                fourWay.Visible = straight.Visible = turn.Visible = false;

                if (!neighbors.ContainsKey(Direction.North))
                {
                    
                }
                else if (!neighbors.ContainsKey(Direction.East))
                {
                    baseRotation = new Quaternion(-Mogre.Math.HALF_PI, Vector3.UNIT_Y);
                }
                else if (!neighbors.ContainsKey(Direction.South))
                {
                    baseRotation = new Quaternion(Mogre.Math.PI, Vector3.UNIT_Y);
                }
                else if (!neighbors.ContainsKey(Direction.West))
                {
                    baseRotation = new Quaternion(Mogre.Math.HALF_PI, Vector3.UNIT_Y);
                }
            }
            else if (neighbors.Count == 2)
            {
                if (isStraight(neighbors.Keys.First(), neighbors.Keys.Last()))
                {
                    if (neighbors.Keys.First() == Direction.North || neighbors.Keys.First() == Direction.South)
                    {
                        baseRotation = new Quaternion(Mogre.Math.HALF_PI, Vector3.UNIT_Y);
                    }

                    straight.Visible = true;
                    fourWay.Visible = turn.Visible = tBend.Visible = false;
                }
                else
                {
                    if (neighbors.ContainsKey(Direction.North) && neighbors.ContainsKey(Direction.East))
                    {
                        baseRotation = new Quaternion(Mogre.Math.HALF_PI, Vector3.UNIT_Y);
                    }
                    else if (neighbors.ContainsKey(Direction.East) && neighbors.ContainsKey(Direction.South))
                    {
                        //baseRotation = new Quaternion(Mogre.Math.PI, Vector3.UNIT_Y);
                    }
                    else if (neighbors.ContainsKey(Direction.South) && neighbors.ContainsKey(Direction.West))
                    {
                        baseRotation = new Quaternion(-Mogre.Math.HALF_PI, Vector3.UNIT_Y);
                    }
                    else if (neighbors.ContainsKey(Direction.West) && neighbors.ContainsKey(Direction.North))
                    {
                        baseRotation = new Quaternion(Mogre.Math.PI, Vector3.UNIT_Y);
                    }

                    turn.Visible = true;
                    fourWay.Visible = straight.Visible = tBend.Visible = false;
                }
            }
            else if (neighbors.Count == 1)
            {
                if (neighbors.Keys.First() == Direction.North || neighbors.Keys.First() == Direction.South)
                {
                    baseRotation = new Quaternion(Mogre.Math.HALF_PI, Vector3.UNIT_Y);
                }

                straight.Visible = true;
                fourWay.Visible = turn.Visible = tBend.Visible = false;
            }
            else
            {
                straight.Visible = true;
                fourWay.Visible = turn.Visible = tBend.Visible = false;
            }
        }

        private bool isStraight(Direction d1, Direction d2)
        {
            return (d1 == Direction.North && d2 == Direction.South) ||
                (d1 == Direction.South && d2 == Direction.North) ||
                (d1 == Direction.East && d2 == Direction.West) ||
                (d1 == Direction.West && d2 == Direction.East);
        }
    }

    public class RenderablePipe : Renderable {

        private Links data;

        public RenderablePipe(Links data) : base() {
            this.data = data;
        }

        public override void Create(SceneManager sm, SceneNode cityNode) {
            foreach (Entity e in GetPipeEntities(this.data)) { this.entities.Add(e); }
            base.Create(sm, cityNode);
        }

        public static List<Entity> GetPipeEntities(Links b) {
            throw new NotImplementedException();
        }
    }
}
