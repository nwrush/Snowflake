using System;
using System.Collections.Generic;
using System.Text;

using Axiom;
using Axiom.Core;
using Axiom.Math;

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

		public virtual void Initialize() {
            CreateEntities();
            AttachNode();
			TranslateIntoPosition();
		}

        public virtual void CreateEntities() {
            this.entities.Add(GV.SceneManager.CreateEntity(this.Name, PrefabEntity.Cube));
        }
        public virtual void AttachNode ()
		{
			if (node == null) {
				node = GV.SceneManager.RootSceneNode.CreateChildSceneNode();
			}
            foreach (Entity e in this.entities) {
                node.AttachObject(e);
            }
        }
		protected virtual void TranslateIntoPosition ()
		{
			if (this.Plot != null) {
				node.Translate (new Vector3 (this.Plot.PlotX * Plot.Width, 0, this.Plot.PlotY * Plot.Height));
			}
		}

        public virtual void Update() {
            //ConsumeResources();
            //ProduceResources();
            //ProducePollution();
        }
        public virtual void ConsumeResources() {
			throw new NotImplementedException ();
            //Override this method for resource consumption
            //For example, a PowerBuilding would check if the parent plot has any Resources of the relevant type of fuel (for example Coal) 
            //and Deplete accordingly, then manipulate some internal variables and output Electricity in the ProduceResources method and
            //Smog in the ProducePollution method.
        }
		public virtual void ProduceResources ()
		{
			throw new NotImplementedException ();
			//Override this method for resource production
		}		
		public virtual void ProducePollution ()
		{
			throw new NotImplementedException ();
		}


    }
}
