using System;

using Snowflake.Modules;

using Miyagi;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

namespace Snowflake.GuiComponents {
    public class ContextMenu : IGuiComponent {

        private Panel parentPanel;

        public void CreateGui(GUI gui) {

            parentPanel = new Panel("DP_parent") {
                Skin = ResourceManager.Skins["PanelSkin"],
                Size = new Size(200, 100),
                Location = new Point(0, 0),
                ResizeMode = ResizeModes.None
            };

            gui.Controls.Add(parentPanel);
        }
    }
}
