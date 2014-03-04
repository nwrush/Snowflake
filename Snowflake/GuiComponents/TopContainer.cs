using System;
using System.Collections.Generic;
using System.Text;
using Haswell;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public partial class TopContainer : IGuiComponent {
        public void Initialize() {

        }

        public void Update(float frametime) {
            UpdateStats(frametime);
            UpdateWeather(frametime);
            UpdateChildren(frametime);
        }

        private void UpdateChildren(float frametime)
        {
            ParentPanel.Update();
            buildToolbar.Update(frametime);
            buildingsBar.Update(frametime);
            governToolbar.Update(frametime);
            manageToolbar.Update(frametime);
        }

        private void UpdateStats(float frametime) {
            //Update progress bar values with tracked stats from CityManager.

            pbHappiness.Value = 50;
            pbAffluence.Value = 67;
            pbEnvquality.Value = 23;

            labelMoney.Text = CityManager.ActiveCity.Resources[ResourceType.Money].ToString("c");
            labelPopulation.Text = CityManager.ActiveCity.Resources[ResourceType.Population].ToString();

            textMoney.Location = new Point(labelMoney.Width - (int)(labelMoney.Width * 0.25), textMoney.Location.Y);
            textPopulation.Location = new Point(labelPopulation.Width - (int)(labelPopulation.Width * 0.25), textPopulation.Location.Y);
        }

        private void UpdateWeather(float frametime) {
            SetWeatherIcon(CityManager.WeatherMgr.CurrentWeather);
        }

        /// <summary>
        /// Set the weather icon to the specified Weather condition
        /// Time of day is automatically compensated for
        /// </summary>
        /// <param name="weather">Weather to set the icon to </param>
        public void SetWeatherIcon(Weather weather) {
            if (ResourceManager.Skins["WeatherIcons"].IsSubSkinDefined("WeatherIcons." + weather.ToString())) {
                weatherIcon.Picture = ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons." + weather.ToString()];
            }
        }

        public void Dispose() {
            weatherIcon.GUI.Controls.Remove(weatherIcon);
            weatherIcon.Dispose();
        }

    }
}
