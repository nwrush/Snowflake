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
        protected Dictionary<string, Button> Buttons;
        protected Dictionary<string, ExpanderToolbar> Children;
        Button expandButton;
        public string Skin = "BlackPanelSkin";

        private int boxwidth;
        private int expandersize;
        private int padding;
        private int boxheight;

        public virtual void CreateGui(GUI gui)
        {
            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            expandButton = new PictureButton(this.GetType() + "_expandBtn")
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
                PictureOffset = new Point((boxwidth - 16) / 2, (expandersize - 16) / 2),
                Visible = expandersize > 0
            };
            expandButton.MouseClick += (object sender, MouseButtonEventArgs e) =>
            {
                if (this.expanded) { this.Contract(); }
                else { this.Expand(); }
            };

            ParentPanel = new Panel(this.GetType().ToString() + "_" + this.GetHashCode().ToString() + "_Parent")
            {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(boxwidth, boxheight),
                Movable = false,
                ResizeMode = ResizeModes.None,
                MinSize = (vertical ? new Size(boxwidth, expandersize) : new Size(expandersize, boxheight)),
                //MaxSize = new Size(boxwidth, boxheight),
                ResizeThreshold = new Thickness(0),
                BorderStyle =
                {
                    Thickness = (vertical ? new Thickness(1, 0, 1, 1) : new Thickness(0, 2, 2, 2))
                },
                Skin = ResourceManager.Skins[Skin],
                AlwaysOnBottom = true,
                VScrollBarStyle = new ScrollBarStyle()
                {
                    Extent = 0
                },
                AlwaysOnTop = true
            };

            ParentPanel.Controls.Add(expandButton);
            foreach (Button b in Buttons.Values)
            {
                ParentPanel.Controls.Add(b);
            }
            gui.Controls.Add(ParentPanel);

            this.Initialize();
        }
    }
}
