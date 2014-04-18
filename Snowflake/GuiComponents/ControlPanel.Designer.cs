using System;
using System.Collections.Generic;
using System.Text;using Miyagi.Common;
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
        private PictureButton loadButton;
        private PictureButton expandButton;

        private int boxwidth = 48;
        private int expandersize = 32;
        private int padding = 3;
        private int boxheight;

        public void CreateGui(GUI gui) {

            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            boxheight = expandersize + boxwidth * 4 + padding * 6;

            ParentPanel = new Panel("CP_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(boxwidth, boxheight),
                Movable = false,
                ResizeMode = ResizeModes.None,
                MinSize = new Size(boxwidth, expandersize),
                MaxSize = new Size(boxwidth, boxheight),
                Location = new Point(gw - boxwidth, 130),
                ResizeThreshold = new Thickness(0),
                BorderStyle = {
                    Thickness = new Thickness(1, 0, 0, 1)
                },
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                AlwaysOnBottom = true,
                VScrollBarStyle = new ScrollBarStyle() {
                    Extent = 0
                }
            };
            
            expandButton = new PictureButton("CP_expandBtn") {
                Location = new Point(0, boxheight - expandersize),
                Size = new Size(boxwidth, expandersize),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Font = ResourceManager.Fonts["Heading"],
                    Alignment = Alignment.TopCenter
                },
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                BorderStyle = new BorderStyle() {
                    Thickness = new Thickness(0, 1, 0, 0)
                },
                Picture = ResourceManager.Skins["Control"].SubSkins["Control.Expand"],
                PictureSize = new Size(16, 16),
                PictureOffset = new Point((boxwidth - 16) / 2, (expandersize - 16) / 2)
            };
            expandButton.MouseClick += (object sender, MouseButtonEventArgs e) => {
                if (this.expanded) { this.Contract(); }
                else { this.Expand(); }
            };

            quitButton = new PictureButton("CP_quitBtn") {
                Location = new Point(0, boxheight - expandersize - boxwidth),
                Size = new Size(boxwidth, boxwidth),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Control"].SubSkins["Control.Exit"]
            };

            saveButton = new PictureButton("CP_saveBtn") {
                Location = new Point(0, boxheight - expandersize - boxwidth * 2 - padding),
                Size = new Size(boxwidth, boxwidth),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Control"].SubSkins["Control.Save"]
            };

            loadButton = new PictureButton("CP_loadBtn") {
                Location = new Point(0, boxheight - expandersize - boxwidth * 3 - padding * 3),
                Size = new Size(boxwidth, boxwidth),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Control"].SubSkins["Control.Load"]
            };

            optionsButton = new PictureButton("CP_optionsBtn") {
                Location = new Point(0, boxheight - expandersize - boxwidth * 4 - padding * 5),
                Size = new Size(boxwidth, boxwidth),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                //TextStyle = new TextStyle() { ForegroundColour = Colours.White }
                Picture = ResourceManager.Skins["Control"].SubSkins["Control.Options"]
            };

            ParentPanel.Controls.AddRange(expandButton, quitButton, saveButton, loadButton, optionsButton);

            gui.Controls.Add(ParentPanel);

            this.Initialize();
            quitButton.Click += (object sender, EventArgs e) =>
            {
                CityManager.Quit();
            };
        }
    }
}
