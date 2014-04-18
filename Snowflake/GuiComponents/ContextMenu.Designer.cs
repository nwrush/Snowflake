using System;
using System.Collections.Generic;

using Snowflake.Modules;

using Miyagi;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;
namespace Snowflake.GuiComponents {
    public partial class ContextMenu {

        private Panel parentPanel;

        public void CreateGui(GUI gui) {

            parentPanel = new Panel("DP_parent") {
                Skin = ResourceManager.Skins["PanelSkin"],
                Size = new Size(150, 200),
                Location = new Point(0, 0),
                ResizeMode = ResizeModes.None
            };

            gui.Controls.Add(parentPanel);

            Initialize();
        }

        public void Dispose() {
            parentPanel.GUI.Controls.Remove(parentPanel);
            foreach (Control c in parentPanel.Controls) {
                c.Dispose();
            }
            parentPanel.Dispose();
        }
    }
}
