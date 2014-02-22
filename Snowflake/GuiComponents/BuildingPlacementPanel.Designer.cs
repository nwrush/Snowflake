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

        public void CreateGui(GUI gui) {
            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            ParentPanel = new Panel("BPP_Parent")
            {

            };
        }
    }
}
