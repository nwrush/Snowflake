using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

using Snowflake.Modules;

namespace Snowflake.GuiComponents
{
    public partial class BuildingPlacementPanel
    {
        private Panel ParentPanel;
        private PictureButton RotateLeft;
        private PictureButton RotateRight;
        private RenderBox Renderbox;

        public void CreateGui(GUI gui) {

            CreateRenderSystem();

            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            ParentPanel = new Panel("BPP_Parent")
            {
                Width = 158,
                Height = 170,
                BorderStyle = new BorderStyle() {
                    Thickness = new Thickness(4, 16, 4, 4)
                },
                ResizeMode = ResizeModes.None,
                Skin = ResourceManager.Skins["ClearPanelSkin"],
                Location = new Point(20, gh - 150 - 20),
                Movable = true
            };
            Renderbox = new RenderBox("BPP_RenderBox")
            {
                Width = 130,
                Height = 130,
                Location = new Point(10, 10),
                Camera = renderCam
            };
            ParentPanel.Controls.Add(Renderbox);

            gui.Controls.Add(ParentPanel);
        }
    }
}
