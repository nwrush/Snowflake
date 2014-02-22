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
        private float _targetYaw = 0.0f;

        public void Initialize()
        {

        }

        private void CreateRenderSystem()
        {
            renderCam = CityManager.Engine.SceneMgr.CreateCamera("renderbox_cam");
            renderNode = CityManager.Engine.SceneMgr.RootSceneNode.CreateChildSceneNode();
            renderNode.AttachObject(CityManager.Engine.SceneMgr.CreateEntity(SceneManager.PrefabType.PT_CUBE));
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
            //if (_targetYaw > Mogre.Math.HALF_PI) { _targetYaw -= Mogre.Math.PI; }
            //if (_targetYaw < -Mogre.Math.HALF_PI) { _targetYaw += Mogre.Math.PI; }
            renderNode.Yaw((new Mogre.Radian(_targetYaw) - renderNode.Orientation.Yaw) * 0.05f);
            DebugPanel.ActiveInstance[1] = ("Target yaw: " + _targetYaw.ToString("0.00") + "\n Actual Yaw: " + renderNode.Orientation.Yaw.ValueRadians.ToString("0.00"));
        }

        public void Dispose() { }
    }
}
