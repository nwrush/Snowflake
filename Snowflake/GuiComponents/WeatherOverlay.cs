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
    public class WeatherOverlay : GuiComponent {

        private Panel icon;

        public override void CreateGui(MiyagiSystem system) {

            base.CreateGui(system);

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

        /// <summary>
        /// Set the weather icon to the specified Weather condition
        /// Time of day is automatically compensated for
        /// </summary>
        /// <param name="weather">Weather to set the icon to </param>
        public void SetWeatherIcon(Weather weather) {
            Console.WriteLine(weather.ToString());
            if (ResourceManager.Skins["WeatherIcons"].IsSubSkinDefined("WeatherIcons."+weather.ToString())) {
                icon.SetBackgroundTexture(ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons."+weather.ToString()]);
            }
        }

        public void UpdateTimeLabel() {
            
        }
    }
}
