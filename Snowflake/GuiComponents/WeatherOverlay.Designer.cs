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
    public partial class WeatherOverlay {
        private Panel icon;

        public void CreateGui(GUI gui) {

            icon = new Panel("WO_Icon") {
                Size = new Size(64, 64),
                Location = new Point(20, 20),
                ResizeMode = ResizeModes.None
            };
            icon.SetBackgroundTexture(ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons.Sunny"]);

            gui.Controls.Add(icon);

            Initialize();
        }

        public void Dispose() {
            icon.GUI.Controls.Remove(icon);
            icon.Dispose();
        }
    }
}
