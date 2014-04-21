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
            UpdateOptions(frametime);
            UpdateAction(frametime);
        }

        private void UpdateAction(float frametime)
        {
            switch (CityManager.GetMouseMode())
            {
                case States.MouseMode.Selection:
                    setCurrentAction("Selecting");
                    break;
                case States.MouseMode.PlacingBuilding:
                    setCurrentAction("Placing Building");
                    break;
                case States.MouseMode.DrawingZone:
                    setCurrentAction("Drawing Zone: " + CityManager.scratchZoneType.ToString());
                    break;
                case States.MouseMode.DeletingZone:
                    setCurrentAction("Clearing Zone");
                    break;
                case States.MouseMode.DrawingRoad:
                    setCurrentAction("Drawing Road");
                    break;
                default:
                    setCurrentAction(" ");
                    break;
            }
            if (CityManager.GetMouseMode() == States.MouseMode.DrawingZone)
            {
                toggleZoneTypeLeft.Visible =
                    toggleZoneTypeRight.Visible =
                    toggleZoneTypeLeft.HitTestVisible =
                    toggleZoneTypeRight.HitTestVisible = 
                    actionPanel.HitTestVisible = true;
                toggleZoneTypeRight.Left = 50 + currentActionLabel.Width + 3;
            }
            else
            {
                toggleZoneTypeLeft.Visible =
                    toggleZoneTypeRight.Visible =
                    toggleZoneTypeLeft.HitTestVisible =
                    toggleZoneTypeRight.HitTestVisible = 
                    actionPanel.HitTestVisible = false;
            }
            actionPanel.Width = currentActionLabel.Width + 90;
        }

        private void setCurrentAction(string text)
        {
            currentActionLabel.Width = currentActionLabelShadow.Width = 0;
            currentActionLabel.Text = text;
            currentActionLabelShadow.Text = text;
        }

        private void UpdateOptions(float frametime)
        {
            if (zoneOption.Checked != CityManager.ShowZones) { zoneOption.Checked = CityManager.ShowZones; }
        }

        private void UpdateChildren(float frametime)
        {
            ParentPanel.Update();
            buildToolbar.Update(frametime);
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
