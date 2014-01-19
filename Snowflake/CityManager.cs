using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

using Snowflake.Modules;
using Haswell;

namespace Snowflake {
    public class CityManager {
        

        private int MaxX, MaxY, MinX, MinY;

        private SceneNode cityNode;
        private Entity ground;
        private SceneNode world;

        private List<Renderable> cityObjects;

        /// <summary>
        /// Sets up city terrain and creates road planes.
        /// </summary>
        /// <param name="sm">Scenemanager to create scene in</param>
        public void CreateScene(SceneManager sm) {

            CreateTerrain(sm);
            CreateCity(sm);
        }

        /// <summary>
        /// Set up terrain
        /// </summary>
        private void CreateTerrain(SceneManager sm) {
           
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 3500, 3500, 40, 40, true, 1, 5, 5, Vector3.UNIT_Z);

            ground = sm.CreateEntity("GroundEntity", "ground");
            ground.SetMaterialName("Grass");
            world = sm.RootSceneNode.CreateChildSceneNode();
            world.AttachObject(ground);
            //world.Translate(new Vector3(0, -1, 0));
        }

        private void CreateCity(SceneManager sm) {
            CreateRoads(sm);
            CreateObjects(sm);
        }
        
        private void CreateRoads(SceneManager sm) {

            //Todo: initialize roads from Haswell data

            throw new NotImplementedException();
        }

        private void CreateObjects(SceneManager sm) {

            //Todo: initialize objects from Haswell data

            foreach (Renderable r in cityObjects) {
                r.Create(sm);
            }
        }

        public void Update(long frametime) {
            
            Haswell.Controller.Update(frametime);

            foreach (Renderable r in cityObjects) {
                //Check if r needs updating, and if so:
                //r.Update();
            }
        }
    }
}
