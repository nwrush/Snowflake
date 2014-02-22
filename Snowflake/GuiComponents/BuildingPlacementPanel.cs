using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

namespace Snowflake.GuiComponents
{
    public partial class BuildingPlacementPanel : IGuiComponent
    {
        private Camera renderCam;
        private SceneNode renderNode;
        private RenderableBuilding renderBldg;

        public void Initialize()
        {

        }

        private void CreateRenderSystem()
        {
            renderCam = CityManager.Engine.SceneMgr.CreateCamera("renderbox_cam");
            renderNode = CityManager.Engine.SceneMgr.RootSceneNode.CreateChildSceneNode();
            renderNode.AttachObject(CityManager.Engine.SceneMgr.CreateEntity(SceneManager.PrefabType.PT_CUBE));
            renderNode.Pitch(45);
            renderNode.Yaw(45);
            renderNode.Translate(new Vector3(1000000, 0, 0));
            renderCam.SetPosition(200 + 1000000, 200, 200);
            renderCam.LookAt(1000000, 0, 0);
        }

        /// <summary>
        /// Returns whether or not the context menu is currently visible
        /// </summary>
        public bool Visible
        {
            get { return this.ParentPanel.Visible; }
            set
            {
                ParentPanel.Visible = value;
                ParentPanel.Enabled = value;
            }
        }

        public void Update(float frametime)
        {

        }

        public void Dispose() { }
    }
}
