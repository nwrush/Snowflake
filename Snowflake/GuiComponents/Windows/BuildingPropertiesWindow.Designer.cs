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
    public partial class BuildingPropertiesWindow
    {
        private Label typeLabel;
        public override void CreateGui(GUI gui)
        {
            base.CreateGui(gui);

            ParentPanel.Size = new Size(400, 500);
            ParentPanel.TextStyle = new TextStyle()
            {
                Alignment = Alignment.TopRight,
                ForegroundColour = new Colour(255, 192, 192, 192),
                Font = ResourceManager.Fonts["Subtitle"]
            };

            UpdateBuildingInfo();
        }
    }
}
