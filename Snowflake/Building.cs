using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

namespace Snowflake {
    public abstract class Building {

        public Plot Plot;

        protected List<Entity> entities;
        protected SceneNode node;
        public string Name;

        public Building() {
            this.entities = new List<Entity>();
            this.Name = this.GetHashCode().ToString();
        }

        //Initialize this building - create the entities, attach them to the scene node, and then move the building into its position on the grid.
        public virtual void Initialize(SceneManager sm) {
            CreateEntities(sm);
            AttachNode(sm);
            TranslateIntoPosition();
        }

        //Create the entities for this building - load models, materials, etc and position them.
        public virtual void CreateEntities(SceneManager sm) {
            this.entities.Add(sm.CreateEntity(this.GetHashCode().ToString(), "skyscraperBox001.mesh"));
            this.entities[0].CastShadows = true;
        }
        
        //Create the scene node for this building which all of the entities will be attached to.
        public virtual void AttachNode(SceneManager sm) {
            if (node == null) {
                node = sm.RootSceneNode.CreateChildSceneNode();
            }
            foreach (Entity e in this.entities) {
                node.AttachObject(e);
            }
            node.Scale(new Vector3(30, 30, 30));
        }

        //Move the building into its position on the grid, determined by its parent plot's x and y values and the standard width and height of the plot.
        protected virtual void TranslateIntoPosition() {
            if (this.Plot != null) {
                node.Translate(new Vector3(this.Plot.PlotX * Plot.Width, 0, this.Plot.PlotY * Plot.Height));
            }
        }
        public virtual void Select() {
            //Code to run when this building is selected by the player
        }
        public virtual void Deselect() {
            //Code to run when this building is deselected by the player
        }

        public virtual void Update() {
            //ConsumeResources();
            //ProduceResources();
            //ProducePollution();
        }
        public virtual void ConsumeResources() {
            throw new NotImplementedException();
            //Override this method for resource consumption
            //For example, a PowerBuilding would check if the parent plot has any Resource of the relevant type of fuel (for example Coal) 
            //and Deplete accordingly, then manipulate some internal variables and output Electricity in the ProduceResources method and
            //Smog in the ProducePollution method.
        }
        public virtual void ProduceResources() {
            throw new NotImplementedException();
            //Override this method for resource production
        }
        public virtual void ProducePollution() {
            throw new NotImplementedException();
        }


    }
}
