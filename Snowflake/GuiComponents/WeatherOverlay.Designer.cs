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
        private Label text;

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

            Initialize();
        }

        public void Dispose() {
            icon.GUI.Controls.Remove(icon);
            icon.Dispose();
            text.GUI.Controls.Remove(text);
            text.Dispose();
        }
    }
}
