using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;

using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public class WeatherOverlay {

        private Panel icon;

        public void CreateGui(MiyagiSystem system) {

            var gui = new GUI();

            icon = new Panel("WO_Icon") {
                Size = new Size(64, 64),
                Location = new Point(50, 40),
                ResizeMode = ResizeModes.None
            };
            icon.SetBackgroundTexture(ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons.Sunny"]);

            Label text = new Label("WO_Label") {
                Location = new Point(10, 10),
                AutoSize = true,
                TextStyle = {
                    ForegroundColour = Colours.White,
                    Font = ResourceManager.Fonts["Expressway"]
                },
                Text = "Monday, January 6th, 2014"
            };

            gui.Controls.Add(icon);
            gui.Controls.Add(text);

            // add the GUI to the GUIManager
            system.GUIManager.GUIs.Add(gui);
        }

        public void SetWeatherIcon(Weather weather) {
            Console.WriteLine(weather.ToString());
            if (ResourceManager.Skins["WeatherIcons"].IsSubSkinDefined("WeatherIcons."+weather.ToString())) {
                icon.SetBackgroundTexture(ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons."+weather.ToString()]);
            }
        }
    }
}
