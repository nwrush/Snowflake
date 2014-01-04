using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

namespace DRT.Buildings {
    public class ParkBuilding : Building {

        public override void CreateEntities(SceneManager sm) {
            this.entities.Add(sm.CreateEntity(this.GetHashCode().ToString(), "park.mesh"));
            this.entities[0].SetMaterialName("Grass");
            this.entities[0].CastShadows = true;
        }
        public override void AttachNode(Mogre.SceneManager sm) {
            base.AttachNode(sm);
            this.node.SetScale(new Mogre.Vector3(40, 40, 40));
        }
    }
}
