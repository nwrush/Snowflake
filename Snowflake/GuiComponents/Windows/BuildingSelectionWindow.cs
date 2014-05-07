using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Snowflake.GuiComponents.Windows
{
    public partial class BuildingSelectionWindow : Window
    {
        public override void Initialize()
        {
            base.Initialize();
            this.Hide();
        }

        public void FocusPage(string name)
        {
            this.tabs.FocusPage(name);
        }
    }
}
