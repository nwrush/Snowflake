using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

using Snowflake.Modules;

namespace Snowflake.GuiComponents
{
    public partial class ExpanderToolbar
    {
        private Panel ParentPanel;
        public Dictionary<string, Button> Buttons { get; protected set; }

        private int boxwidth;
        private int expandersize;
        private int padding;
        private int boxheight;

        public virtual void CreateGui(GUI gui)
        {
            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            Button expandButton = new PictureButton(this.GetType() + "_expandBtn")
            {
                Location = new Point(0, boxheight - expandersize),
                Size = new Size(boxwidth, expandersize),
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Font = ResourceManager.Fonts["Heading"],
                    Alignment = Alignment.TopCenter
                },
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                BorderStyle = new BorderStyle()
                {
                    Thickness = new Thickness(0, 1, 0, 0)
                },
                Picture = ResourceManager.Skins["Control"].SubSkins["Control.Expand"],
                PictureSize = new Size(16, 16),
                PictureOffset = new Point((boxwidth - 16) / 2, (expandersize - 16) / 2)
            };
            expandButton.MouseClick += (object sender, MouseButtonEventArgs e) =>
            {
                if (this.expanded) { this.Contract(); }
                else { this.Expand(); }
            };

            ParentPanel = new Panel(this.GetType().ToString() + "_Parent")
            {
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
                BorderStyle =
                {
                    Thickness = new Thickness(1, 0, 0, 1)
                },
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                AlwaysOnBottom = true,
                VScrollBarStyle = new ScrollBarStyle()
                {
                    Extent = 0
                }
            };
        }
    }
}
