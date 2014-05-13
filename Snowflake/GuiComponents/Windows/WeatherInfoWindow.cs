using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Haswell;

namespace Snowflake.GuiComponents.Windows
{
    public partial class WeatherInfoWindow : Window
    {

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(float _frametime)
        {
            base.Update(_frametime);
            UpdateInfo(_frametime);
        }

        public void UpdateInfo(float _frametime)
        {
            cloudinessBar.Value = (int)(Haswell.Controller.City.Weather.GetCloudiness() * 1000);
            fogginessBar.Value = (int)(Haswell.Controller.City.Weather.GetFogginess() * 1000);
            raininessBar.Value = (int)(Haswell.Controller.City.Weather.GetPrecipitationRate() * 1000);
            
            float temp = Haswell.Controller.City.Weather.GetTemperature();
            temp = temp * 72 + 32;
            tempCurrent.Text = temp.ToString("G2") + "°F";
        }
    }
}
