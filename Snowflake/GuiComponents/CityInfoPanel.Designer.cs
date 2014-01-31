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
    public partial class CityInfoPanel {
        private Panel ParentPanel;
        private Label timeLabel;
        private Label timeLabelShadow;
        private Label cityLabel;

        public void CreateGui(GUI gui) {

            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            ParentPanel = new Panel("CIP_parent") {
                //Skin = ResourceManager.Skins["BlackPanelSkin"],
                Size = new Size(300, 100),
                Location = new Point(gw - 310, gh - 160),
                ResizeMode = ResizeModes.None,
                BorderStyle = new BorderStyle() {
                    Thickness = new Thickness(0, 0, 1, 1)
                },
                Throwable = false,
                Movable = false
            };
            //ParentPanel.SetBackgroundTexture(ParentPanel.Skin.SubSkins["BlackPanelSkin40"]);

            timeLabel = new Label("CIP_LabelTime") {
                Location = new Point(15, ParentPanel.Size.Height - 40),
                AutoSize = true,
                TextStyle = {
                    ForegroundColour = Colours.White,
                    Font = ResourceManager.Fonts["Lane"],
                    Alignment = Alignment.TopCenter
                },
                Text = "<Undefined>"
            };
            timeLabelShadow = new Label("CIP_LabelTimeShadow") {
                Location = new Point(17, ParentPanel.Size.Height - 38),
                AutoSize = true,
                TextStyle = {
                    ForegroundColour = Colours.DarkGrey,
                    Font = ResourceManager.Fonts["Lane"],
                    Alignment = Alignment.TopCenter
                },
                Text = timeLabel.Text
            };

            cityLabel = new Label("CIP_LabelCity") {
                Location = new Point(15, 15),
                AutoSize = true,
                TextStyle = {
                    ForegroundColour = Colours.White,
                    Font = ResourceManager.Fonts["Subtitle"],
                    Alignment = Alignment.TopCenter
                },
                Text = "<Undefined>"
            };
            ParentPanel.Controls.Add(cityLabel);
            ParentPanel.Controls.Add(timeLabel);
            ParentPanel.Controls.Add(timeLabelShadow);

            gui.Controls.Add(ParentPanel);
        }
    }
}
