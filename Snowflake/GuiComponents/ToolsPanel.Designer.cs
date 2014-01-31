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
    public partial class ToolsPanel {
        private Panel ParentPanel;
        private PictureButton buildButton;
        private PictureButton manageButton;
        private PictureButton governButton;

        public void CreateGui(GUI gui) {
            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            int padding = 20;
            Size panelSize = new Size(100 * 3 + padding * 6, 120);
            ParentPanel = new Panel("ToolsPanel_ParentPanel") {
                Location = new Point(gw - 350 - panelSize.Width, 0),
                Size = panelSize,
                Movable = false,
                Throwable = false,
                ResizeMode = ResizeModes.None
            };

            buildButton = new PictureButton("ToolsPanel_btnBuild") {
                Location = new Point (panelSize.Width - 100 - padding, 10),
                Size = new Size(100, 100),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                PictureSize = new Size(80, 80),
                PictureOffset = new Point(18, 18),
                //Skin = ResourceManager.Skins["BlackPanelSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build"],
                PictureHover = ResourceManager.Skins["Tools"].SubSkins["Tools.Build.MouseEnter"]
            };
            manageButton = new PictureButton("ToolsPanel_btnManage") {
                Location = new Point(panelSize.Width - 200 - padding * 3, 10),
                Size = new Size(100, 100),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                PictureSize = new Size(80, 80),
                PictureOffset = new Point(18, 18),
                //Skin = ResourceManager.Skins["BlackPanelSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build"],
                PictureHover = ResourceManager.Skins["Tools"].SubSkins["Tools.Build.MouseEnter"]
            };
            governButton = new PictureButton("ToolsPanel_btnGovern") {
                Location = new Point(panelSize.Width - 300 - padding * 5, 10),
                Size = new Size(100, 100),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                PictureSize = new Size(80, 80),
                PictureOffset = new Point(18, 18),
                //Skin = ResourceManager.Skins["BlackPanelSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Govern"],
                PictureHover = ResourceManager.Skins["Tools"].SubSkins["Tools.Govern.MouseEnter"]
            };

            ParentPanel.Controls.AddRange(buildButton, manageButton, governButton);
            gui.Controls.Add(ParentPanel);
        }
    }
}
