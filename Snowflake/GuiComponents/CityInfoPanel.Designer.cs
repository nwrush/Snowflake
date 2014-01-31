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

namespace Snowflake.GuiComponents {
    public partial class CityInfoPanel {
        private Panel ParentPanel;

        public void CreateGui(GUI gui) {

            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            ParentPanel = new Panel("CIP_parent") {
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                Size = new Size(300, 150),
                Location = new Point(gh - 160, gw - 310),
                ResizeMode = ResizeModes.None,
                BorderStyle = new BorderStyle() {
                    Thickness = new Thickness(0, 0, 1, 1)
                },
                Throwable = false,
                Movable = false,
                HitTestVisible = false
            };

            gui.Controls.Add(ParentPanel);
        }
    }
}
