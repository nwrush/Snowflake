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

        private Button residentialBtn;
        private Button commercialBtn;
        private Button municipalBtn;

        private Panel residentialPanel;
        private Panel commercialPanel;
        private Panel municipalPanel;

        private const int TABWIDTH = 96;
        private const int TABHEIGHT = 32;

        public override void CreateGui(GUI gui) {

            base.CreateGui(gui);

            ParentPanel.Size = new Size(400, 500);

            residentialBtn = new Button("BCW_btnResidential") {
                Location = new Point(0, 0),
                Size = new Size(TABWIDTH, TABHEIGHT),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.MiddleCenter
                },
                Text = "Residential",
                Skin = ResourceManager.Skins["ButtonSkin"],
            };
            commercialBtn = new Button("BCW_btnCommercial") {
                Location = new Point(TABWIDTH, 0),
                Size = new Size(TABWIDTH, TABHEIGHT),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.MiddleCenter
                },
                Text = "Commercial",
                Skin = ResourceManager.Skins["ButtonSkin"],
            };
            municipalBtn = new Button("BCW_btnMunicipal") {
                Location = new Point(TABWIDTH * 2, 0),
                Size = new Size(TABWIDTH, TABHEIGHT),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.MiddleCenter
                },
                Text = "Municipal",
                Skin = ResourceManager.Skins["ButtonSkin"],
            };

            residentialPanel = new Panel("BCW_panelResidential") {
                Location = new Point(0, 32),
                Size = new Size(400, 468),
                Skin = ResourceManager.Skins["PanelSkin"]
            };
            residentialBtn.Click += (object sender, EventArgs e) => { HidePanels(); ShowPanel(residentialPanel); };

            commercialPanel = new Panel("BCW_panelCommercial") {
                Location = new Point(0, 32),
                Size = new Size(400, 468),
                Skin = ResourceManager.Skins["PanelSkin"]
            };
            commercialBtn.Click += (object sender, EventArgs e) => { HidePanels(); ShowPanel(commercialPanel); };

            municipalPanel = new Panel("BCW_panelMunicipal") {
                Location = new Point(0, 32),
                Size = new Size(400, 468),
                Skin = ResourceManager.Skins["PanelSkin"]
            };
            municipalBtn.Click += (object sender, EventArgs e) => { HidePanels(); ShowPanel(municipalPanel); };

            ParentPanel.Controls.AddRange(new Control[] { residentialBtn, commercialBtn, municipalBtn, residentialPanel, commercialPanel, municipalPanel });

            this.Text = "Create Building";
        }

        private void HidePanels() {
            residentialPanel.Visible = commercialPanel.Visible = municipalPanel.Visible = false;
            residentialPanel.HitTestVisible = commercialPanel.HitTestVisible = municipalPanel.HitTestVisible = false;
        }
        private void ShowPanel(Panel p) {
            p.Visible = p.HitTestVisible = true;
        }
    }
}
