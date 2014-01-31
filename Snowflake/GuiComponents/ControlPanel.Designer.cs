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
    public partial class ControlPanel {
        public Panel ParentPanel;
        private PictureButton saveButton;
        private PictureButton quitButton;
        private PictureButton optionsButton;
        private PictureButton loadbutton;
        private Button expandButton;

        public void CreateGui(GUI gui) {

            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            ParentPanel = new Panel("CP_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(48, 192),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Opacity = 0.7f,
                Location = new Point(gw - 48, gh - 400),
                ResizeThreshold = new Thickness(0),
                BorderStyle = {
                    Thickness = new Thickness(4, 4, 4, 4)
                },
                Skin = ResourceManager.Skins["PanelSkin"],
                AlwaysOnBottom = true
            };

            expandButton = new Button("CP_expandBtn") {
                Location = new Point(16, 0),
                Size = new Size(16, 16),
                Text = "^",
                Opacity = 0.7f,
            };

            ParentPanel.Controls.Add(expandButton);

            gui.Controls.Add(expandButton);
        }
    }
}
