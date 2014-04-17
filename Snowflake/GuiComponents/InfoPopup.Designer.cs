using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Snowflake.Modules;

using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Miyagi.Common.Data;

namespace Snowflake.GuiComponents
{
    public partial class InfoPopup
    {
        public Panel ParentPanel;

        private PictureButton mainButton;

        public void CreateGui(GUI gui)
        {
            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            ParentPanel = new Panel()
            {
                Skin = ResourceManager.Skins["ClearPanelSkin"],
                Width = gw / 3,
                Height = 48,
                BorderStyle = new BorderStyle()
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                },
                Location = new Point(gw / 3, gh - 48)
            };

            mainButton = new PictureButton()
            {
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Size = ParentPanel.Size,
                Location = new Point(0, 0),
                TextStyle = new TextStyle()
                {
                    Font = ResourceManager.Fonts["Subheading"],
                    ForegroundColour = Colours.White,
                    Multiline = true,
                    Alignment = Miyagi.Common.Alignment.MiddleLeft,
                    Offset = new Point(25, 0)
                },
                Picture = ResourceManager.Skins["Control"].SubSkins["Control.Info"],
                PictureSize = new Size(24, 24),
                PictureOffset = new Point(-10, 7),
                Padding = new Thickness(20, 2, 2, 2),
                Text = _text
            };

            ParentPanel.Controls.Add(mainButton);
            gui.Controls.Add(ParentPanel);
        }
    }
}
