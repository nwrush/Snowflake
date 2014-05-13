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
        Label tempCurrent;
        Label tempLabel;

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
                Location = new Point(40, 215),
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
                Location = new Point(55, 232)
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
                Location = new Point(40, 250),
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
                Location = new Point(55, 267)
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
                Location = new Point(40, 285),
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
                Location = new Point(55, 302)
            };
            tempCurrent = new Label()
            {
                Text = "0°",
                AutoSize = true,
                TextStyle = new TextStyle()
                {
                    Alignment = Alignment.TopCenter,
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Subtitle"],
                },
                Location = new Point(75, 20)
            };
            tempLabel = new Label()
            {
                Text = "TEMPERATURE",
                AutoSize = true,
                TextStyle = new TextStyle()
                {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Section"]
                },
                Location = new Point(55, 100)
            };
            ParentPanel.Controls.AddRange(cloudinessBar, cloudinessLabel, fogginessBar, fogginessLabel, raininessBar, raininessLabel, tempCurrent, tempLabel);

            ParentPanel.Size = new Size(250, 400);
        }
    }
}
