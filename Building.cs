using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Axiom;
using Axiom.Core;

namespace Snowflake {
    public abstract class Building {

        public Plot Plot;

        protected List<Entity> entities;
        protected SceneNode node;
        public string Name;

        public Building() {
            CreateEntities();
            AttachNode();
        }

        public virtual void CreateEntities() {
            this.entities.Add(GV.SceneManager.CreateEntity(this.Name, PrefabEntity.Cube));
        }
        public virtual void AttachNode() {
            foreach (Entity e in this.entities) {
                node.AttachObject(e);
            }
        }

        public virtual void Update() {
            ConsumeResources();
            ProduceResources();
            ProducePollution();
        }
        public virtual void ConsumeResources() {
            //Override this method for resource consumption
            //For example, a PowerBuilding would check if the parent plot has any Resources of the relevant type of fuel (for example Coal) 
            //and Deplete accordingly, then manipulate some internal variables and output Electricity in the ProduceResources method and
            //Smog in the ProducePollution method.
        }
    }
}
