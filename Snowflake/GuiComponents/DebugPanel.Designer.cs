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

namespace Snowflake.GuiComponents {
    public partial class DebugPanel {
        private Label fps;
        private Label debugText;
        private Panel parentPanel;
        public void CreateGui(GUI gui) {

            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            parentPanel = new Panel("DP_parent") {
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                Size = new Size(200, gh - 230),
                Location = new Point(gw - 220, 130),
                ResizeMode = ResizeModes.None
            };
            fps = new Label("DP_fps") {
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White
                },
                Location = new Point(5, 5),
                MaxSize = new Size(190, 45),
                AutoSize = true
            };
            debugText = new Label("DP_debugtext") {
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                },
                Location = new Point(5, 50),
                MaxSize = new Size(190, 45),
                AutoSize = true,
                Text = "Debug: "
            };

            parentPanel.SetBackgroundTexture(parentPanel.Skin.SubSkins["BlackPanelSkin40"]);

            parentPanel.Controls.Add(fps);
            parentPanel.Controls.Add(debugText);
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
