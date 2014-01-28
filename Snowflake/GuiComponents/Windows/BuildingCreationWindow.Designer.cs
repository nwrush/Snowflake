using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Miyagi.UI.Controls.Layout;

using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public partial class BuildingCreationWindow {

        private Button residentialBtn;

        public override void CreateGui(GUI gui) {

            base.CreateGui(gui);

            ParentPanel.Size = new Size(400, 200);

            residentialBtn = new Button("BVW_residentialBtn") {
                Size = new Size(100, 100),
                Location = new Point(10, 10),
                Skin = ResourceManager.Skins["SquareButtonSkin"],
                Cursor = "Drag"
            };

            this.Text = "Create Building";

            ParentPanel.Controls.Add(residentialBtn);
        }
    }
}
