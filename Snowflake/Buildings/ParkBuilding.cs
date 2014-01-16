using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

namespace Snowflake.Buildings {
    public class ParkBuilding : Building {

        public override void CreateEntities(SceneManager sm) {
            this.entities.Add(sm.CreateEntity(this.GetHashCode().ToString(), "sel_Box001.mesh"));
            this.entities[0].CastShadows = false;
        }
        public override void AttachNode(Mogre.SceneManager sm) {
            base.AttachNode(sm);
            this.node.SetScale(new Mogre.Vector3(420, 420, 420));
        }
    }
}
