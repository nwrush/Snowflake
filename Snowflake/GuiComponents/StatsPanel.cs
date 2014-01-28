using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

using Snowflake.Modules;

using Haswell;

namespace Snowflake.GuiComponents {
    //Menu for holding tools and stuff
    public partial class StatsPanel : IGuiComponent {

        public void Initialize() {

        }

        public void Update(float frametime) {
            //Update progress bar values with tracked stats from CityManager.

            pbHappiness.Value = 50;
            pbAffluence.Value = 67;
            pbEnvquality.Value = 23;

            labelMoney.Text = "$4,375.64";
            labelPopulation.Text = "Population: 82";

            Dictionary<Resource.Type, int> resources = Haswell.Controller.City.Resources;
            labelMoney.Text = resources[Resource.Type.Money].ToString("C");
        }
    }
}
