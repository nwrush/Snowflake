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
    public class WeatherOverlay : IGuiComponent {

        private Panel icon;
        private Label text;

        //Todo: replace with actual localization.
        private string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public void CreateGui(GUI gui) {

            icon = new Panel("WO_Icon") {
                Size = new Size(64, 64),
                Location = new Point(50, 40),
                ResizeMode = ResizeModes.None
            };
            icon.SetBackgroundTexture(ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons.Sunny"]);

            text = new Label("WO_Label") {
                Location = new Point(10, 10),
                AutoSize = true,
                TextStyle = {
                    ForegroundColour = Colours.White,
                    Font = ResourceManager.Fonts["Expressway"]
                },
                Text = "<Undefined>"
            };

            gui.Controls.Add(icon);
            gui.Controls.Add(text);
        }

        public void Update(float frametime) {

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

        /// <summary>
        /// Update this overlay's time label with the new datetime.
        /// </summary>
        /// <param name="newTime">The new date and time</param>
        public void UpdateTimeLabel(DateTime newTime) {
            this.text.Text = newTime.DayOfWeek + ", " + newTime.Day + " " + Months[newTime.Month - 1] + ", " + newTime.Year + " - " + newTime.Hour + ":" + (newTime.Minute < 10 ? ("0" + newTime.Minute.ToString()) : newTime.Minute.ToString());
        }
    }
}
