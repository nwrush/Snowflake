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

namespace Snowflake.GuiComponents {
    public partial class BuildingCreationWindow {

        private TabControl tabControl;
        private TabPage residentialPage;
        private TabPage commercialPage;
        private TabPage municipalPage;


        public override void CreateGui(GUI gui) {

            base.CreateGui(gui);

            ParentPanel.Size = new Size(400, 500);

            tabControl = new TabControl("BCW_tabControl") {
                AutoSize = true,
                Size = new Size(400, 500),
                Skin = ResourceManager.Skins["TabControlSkin"],
                Movable = false,
                TabBarStyle = {
                    Extent = 32,
                    Mode = TabMode.AutoSize,
                    Alignment = Alignment.MiddleCenter,
                }
                //AlwaysOnBottom = true
            };
            residentialPage = new TabPage("BCW_tabResidential") {
                Title = "Residential",
                Skin = ResourceManager.Skins["TabPageSkin"],
            };
            commercialPage = new TabPage("BCW_tabCommercial") {
                Title = "Commercial",
                Skin = ResourceManager.Skins["TabPageSkin"],
            };
            municipalPage = new TabPage("BCW_tabMunicipal") {
                Title = "Municipal",
                Skin = ResourceManager.Skins["TabPageSkin"],
            };

            /*residentialBtn = new Button("BCW_residentialBtn") {
                Size = new Size(100, 100),
                Location = new Point(10, 10),
                Skin = ResourceManager.Skins["SquareButtonSkin"],
                Text = "Residential"
            };*/
            tabControl.Controls.Add(residentialPage);
            tabControl.Controls.Add(commercialPage);
            tabControl.Controls.Add(municipalPage);

            this.Text = "Create Building";

            ParentPanel.Controls.Add(tabControl);
            ParentPanel.ResizeMode = ResizeModes.None;
            ParentPanel.VisibleChanged += (object sender, EventArgs e) => {
                tabControl.Visible = ParentPanel.Visible;
                foreach (Control c in tabControl.Controls) {
                    c.Opacity = (ParentPanel.Visible ? 1 : 0);
                    c.Visible = false;
                }
                //FieldInfo tabBar = tabControl.GetType().GetField("tabBarElement", BindingFlags.NonPublic | BindingFlags.Instance);
                //tabBar.SetValue(tabControl, ((TabBarElement)tabBar.GetValue(tabControl)));
            };
        }
    }
}
