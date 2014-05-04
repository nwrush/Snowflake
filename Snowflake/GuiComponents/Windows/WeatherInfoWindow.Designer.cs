using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Miyagi.UI.Controls.Layout;

using Snowflake.Modules;
using Miyagi.UI.Controls.Elements;

namespace Snowflake.GuiComponents.Windows
{
    public partial class WeatherInfoWindow
    {
        ProgressBar cloudinessBar;
        Label cloudinessLabel;
        ProgressBar fogginessBar;
        Label fogginessLabel;
        ProgressBar raininessBar;
        Label raininessLabel;

        public override void CreateGui(GUI gui)
        {
            base.CreateGui(gui);

            this.Text = "Weather Info";
            this.Visible = false;
            this.Location = new Point(100, 225);

            cloudinessBar = new ProgressBar()
            {
                ProgressBarStyle = new ProgressBarStyle()
                {
                    Orientation = Orientation.Horizontal,
                    Mode = ProgressBarMode.Continuous,
                    Offset = new Point(1, 1)
                },
                Width = 150,
                Height = 15,
                Location = new Point(20, 15),
                Skin = ResourceManager.Skins["ProgressBarHSkin"],
                Min = 0,
                Max = 1000,
                BorderStyle = new BorderStyle()
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                }
            };
            cloudinessLabel = new Label()
            {
                Text = "CLOUDINESS",
                AutoSize = true,
                TextStyle = new TextStyle()
                {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Section"]
                },
                Location = new Point(35, 32)
            };
            fogginessBar = new ProgressBar()
            {
                ProgressBarStyle = new ProgressBarStyle()
                {
                    Orientation = Orientation.Horizontal,
                    Mode = ProgressBarMode.Continuous,
                    Offset = new Point(1, 1)
                },
                Width = 150,
                Height = 15,
                Location = new Point(20, 50),
                Skin = ResourceManager.Skins["ProgressBarHSkin"],
                Min = 0,
                Max = 1000,
                BorderStyle = new BorderStyle()
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                }
            };
            fogginessLabel = new Label()
            {
                Text = "FOGGINESS",
                AutoSize = true,
                TextStyle = new TextStyle()
                {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Section"]
                },
                Location = new Point(35, 67)
            };
            raininessBar = new ProgressBar()
            {
                ProgressBarStyle = new ProgressBarStyle()
                {
                    Orientation = Orientation.Horizontal,
                    Mode = ProgressBarMode.Continuous,
                    Offset = new Point(1, 1)
                },
                Width = 150,
                Height = 15,
                Location = new Point(20, 85),
                Skin = ResourceManager.Skins["ProgressBarHSkin"],
                Min = 0,
                Max = 1000,
                BorderStyle = new BorderStyle()
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                }
            };
            raininessLabel = new Label()
            {
                Text = "PRECIPITATION RATE",
                AutoSize = true,
                TextStyle = new TextStyle()
                {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Section"]
                },
                Location = new Point(35, 102)
            };
            ParentPanel.Controls.AddRange(cloudinessBar, cloudinessLabel, fogginessBar, fogginessLabel, raininessBar, raininessLabel);
        }
    }
}
