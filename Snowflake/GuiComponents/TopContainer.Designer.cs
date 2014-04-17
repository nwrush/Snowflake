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
    public partial class TopContainer {
        public Panel ParentPanel;
        private PictureButton weatherIcon;
        private CheckBox zoneOption;
        private Label zoneOptionLabel;

        private Panel ToolsContainerPanel;
        private PictureButton buildButton;
        private ExpanderToolbar buildToolbar;
        private PictureButton manageButton;
        private ExpanderToolbar manageToolbar;
        private PictureButton governButton;
        private ExpanderToolbar governToolbar;

        private Panel StatsContainerPanel;
        private ProgressBar pbHappiness;
        private ProgressBar pbAffluence;
        private ProgressBar pbEnvquality;
        private Label labelMoney;
        private Label labelPopulation;
        private Label textMoney;
        private Label textPopulation;

        public void CreateGui(GUI gui) {
            //store game width and height
            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            ParentPanel = new Panel("top_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(gw, 120),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Location = new Point(0, 10),
                BorderStyle = {
                    Thickness = new Thickness(0, 1, 0, 1)
                },
                Skin = ResourceManager.Skins["BlackPanelSkin"]
            };

            weatherIcon = new PictureButton("WO_Icon") {
                Size = new Size(140, 120),
                Location = new Point(0, 0),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                BorderStyle = {
                    Thickness = new Thickness(0, 0, 1, 0)
                },
                //HitTestVisible = false,
                Picture = ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons.Sunny"],
                PictureSize = new Size(64, 64),
                PictureOffset = new Point(48, 20),
                Padding = new Thickness(0, 0, 0, 8),
                Text = "      WEATHER",
                TextStyle = new TextStyle() {
                    Font = ResourceManager.Fonts["Section"],
                    Alignment = Alignment.BottomCenter,
                    ForegroundColour = Colours.White
                }
            };

            zoneOption = new CheckBox("ZoneOption")
            {
                Skin = ResourceManager.Skins["BlackCheckBoxSkin"],
                Location = new Point(160, 20),
                Size = new Size(16, 16),

                BorderStyle = new BorderStyle()
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                }
            };
            zoneOptionLabel = new Label("ZoneOptionLabel")
            {
                Text = "Show Zones",
                AutoSize = true,
                Location = new Point(180, 20),
                TextStyle = new TextStyle()
                {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.White,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Heading"]
                }
            };

            ///

            int padding = 10;
            int boxsize = 120;
            int picsize = 96;
            Size panelSize = new Size(boxsize * 3 + padding * 6, 120);
            ToolsContainerPanel = new Panel("ToolsPanel_ParentPanel") {
                Location = new Point(gw - 350 - panelSize.Width, 0),
                Size = panelSize,
                Movable = false,
                Throwable = false,
                ResizeMode = ResizeModes.None
            };

            buildButton = new PictureButton("ToolsPanel_btnBuild") {
                Location = new Point(panelSize.Width - boxsize - padding, 0),
                Size = new Size(boxsize, boxsize),
                Text = "BUILD",
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build"],
                PictureSize = new Size(picsize, picsize),
                PictureOffset = new Point((boxsize - picsize) / 2, (boxsize - picsize) / 2)
            };
            buildButton.MouseClick += (object sender, MouseButtonEventArgs e) => {
                if (buildToolbar.visible) { buildToolbar.Hide(); }
                else { buildToolbar.Show(); }
            };

            buildToolbar = new ExpanderToolbar(true, boxsize, boxsize * 2, 3, 0)
            {
                Location = new Point(buildButton.Location.X + ToolsContainerPanel.Location.X - 0, ParentPanel.Location.Y + ParentPanel.Height)
            };
            buildToolbar.AddButton("Destroy Building", new PictureButton("TC_buttonDestBldng")
            {
                Size = new Size(120, 120),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Text = "DEMOLISH BUILDING",
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build.DestBuilding"],
                PictureOffset = new Point(32, 32),
                PictureSize = new Size(64, 64),
                ClickFunc = (object sender) =>
                {

                    buildToolbar.Hide();
                }
            });
            buildToolbar.AddButton("New Building", new PictureButton("TC_buttonNewBldng")
            {
                Size = new Size(boxsize, boxsize),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Text = "NEW BUILDING",
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build.NewBuilding"],
                PictureOffset = new Point((boxsize - 64) / 2, (boxsize - 64) / 2),
                PictureSize = new Size(64, 64)
            });
            buildToolbar.CreateGui(gui);

            ExpanderToolbar buildingsBar = new ExpanderToolbar(false, boxsize, boxsize, 3, 0)
            {
                Location = new Point(buildToolbar.Location.X + buildToolbar.Width, buildToolbar.Location.Y),
            };
            buildingsBar.AddButton("New Residential Building", new PictureButton()
            {
                Size = new Size(boxsize, boxsize),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Text = "RESIDENTIAL",
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build.NewResidential"],
                PictureOffset = new Point((boxsize - 64) / 2, (boxsize - 64) / 2),
                PictureSize = new Size(64, 64),
                ClickFunc = (object sender) =>
                {
                    CityManager.CreateBuildingOnCursor();
                    buildingsBar.Hide();
                    EventHandler hidefunc = null;
                    hidefunc = (object sender2, EventArgs e) => {
                        buildToolbar.Hide();
                        buildingsBar.FullyHidden -= hidefunc;
                    };
                    buildingsBar.FullyHidden += hidefunc;
                }
            });
            buildToolbar.AddChild("New Building", buildingsBar);

            manageButton = new PictureButton("ToolsPanel_btnManage") {
                Location = new Point(panelSize.Width - boxsize * 2 - padding * 3, 0),
                Size = new Size(boxsize, boxsize),
                Text = "MANAGE",
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Manage"],
                PictureSize = new Size(picsize, picsize),
                PictureOffset = new Point((boxsize - picsize) / 2, (boxsize - picsize) / 2)
            };
            manageButton.MouseClick += (object sender, MouseButtonEventArgs e) =>
            {
                if (manageToolbar.visible) { manageToolbar.Hide(); }
                else { manageToolbar.Show(); }
            };

            manageToolbar = new ExpanderToolbar(true, boxsize, boxsize * 1, 3, 0)
            {
                Location = new Point(manageButton.Location.X + ToolsContainerPanel.Location.X - 0, ParentPanel.Location.Y + ParentPanel.Height)
            };
            manageToolbar.AddButton("Zoning", new PictureButton()
            {
                Size = new Size(boxsize, boxsize),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Text = "ZONING",
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Manage.Zoning"],
                PictureOffset = new Point((boxsize - 96) / 2, (boxsize - 96) / 2),
                PictureSize = new Size(96, 96),
                ClickFunc = (object sender) =>
                {
                    //enter zoning draw mode
                    CityManager.BeginZoning(Haswell.Zones.Residential);
                    manageToolbar.Hide();
                }
            });
            manageToolbar.CreateGui(gui);

            governButton = new PictureButton("ToolsPanel_btnGovern") {
                Location = new Point(panelSize.Width - boxsize * 3 - padding * 5, 0),
                Size = new Size(boxsize, boxsize),
                Text = "GOVERN",
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Govern"],
                PictureSize = new Size(picsize, picsize),
                PictureOffset = new Point((boxsize - picsize) / 2, (boxsize - picsize) / 2)
            };
            governButton.MouseClick += (object sender, MouseButtonEventArgs e) =>
            {
                if (governToolbar.visible) { governToolbar.Hide(); }
                else { governToolbar.Show(); }
            };

            governToolbar = new ExpanderToolbar(true, boxsize, boxsize * 1, 3, 0)
            {
                Location = new Point(governButton.Location.X + ToolsContainerPanel.Location.X - 0, ParentPanel.Location.Y + ParentPanel.Height)
            };
            governToolbar.AddButton("Policy", new PictureButton("TC_buttonDestPolicy")
            {
                Size = new Size(boxsize, boxsize),
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Text = "CITY POLICY",
                TextStyle = new TextStyle()
                {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter,
                    Font = ResourceManager.Fonts["Section"]
                },
                Padding = new Thickness(0, 0, 0, 8),
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Govern.Policy"],
                PictureOffset = new Point((boxsize - 64) / 2, (boxsize - 64) / 2),
                PictureSize = new Size(64, 64),
                ClickFunc = (object sender) =>
                {

                    governToolbar.Hide();
                }
            });
            governToolbar.CreateGui(gui);

            ToolsContainerPanel.Controls.AddRange(buildButton, manageButton, governButton);

            ///
            int statwidth = 300;
            int barwidth = 16;
            int barpadding = 8;
            int rightpadding = 12;
            StatsContainerPanel = new Panel("Stats_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(statwidth, 120),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Opacity = 0.7f,
                Location = new Point(gw - statwidth, 0),
                ResizeThreshold = new Thickness(0),
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                BorderStyle = new BorderStyle() {
                    Thickness = new Thickness(1, 0, 0, 0)
                }
            };

            pbHappiness = new ProgressBar("Stats_Happiness_Bar") {
                Size = new Size(16, 90),
                Location = new Point(statwidth - barwidth - barpadding - rightpadding, 15),
                ProgressBarStyle = new ProgressBarStyle {
                    Orientation = Orientation.Vertical,
                    Mode = ProgressBarMode.Continuous
                },
                Skin = ResourceManager.Skins["ProgressBarVSkin"],
                ToolTipText = "Happiness"
            };

            pbAffluence = new ProgressBar("Stats_Affluence_Bar") {
                Size = new Size(16, 90),
                Location = new Point(statwidth - barwidth  * 2 - barpadding * 3 - rightpadding, 15),
                ProgressBarStyle = new ProgressBarStyle {
                    Orientation = Orientation.Vertical,
                    Mode = ProgressBarMode.Continuous
                },
                Skin = ResourceManager.Skins["ProgressBarVSkin"],
                ToolTipText = "Affluence"
            };

            pbEnvquality = new ProgressBar("Stats_Envquality_Bar") {
                Size = new Size(16, 90),
                Location = new Point(statwidth - barwidth  * 3 - barpadding * 5 - rightpadding, 15),
                ProgressBarStyle = new ProgressBarStyle {
                    Orientation = Orientation.Vertical,
                    Mode = ProgressBarMode.Continuous
                },
                Skin = ResourceManager.Skins["ProgressBarVSkin"],
                ToolTipText = "Pollution"
            };

            labelMoney = new Label("Stats_Money_Label") {
                AutoSize = true,
                Location = new Point(10, 10),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.White,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Heading"]
                },
                Text = "$0.0"
            };
            textMoney = new Label("Stats_Money_Text") {
                AutoSize = true,
                Location = new Point(statwidth - barwidth * 3 - barpadding * 6 - 120, 34),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.White,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Section"]
                },
                Text = "BUDGET"
            };

            labelPopulation = new Label("Stats_Population_Label") {
                AutoSize = true,
                Location = new Point(10, 60),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.White,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Heading"]
                },
                Text = "0"
            };
            textPopulation = new Label("Stats_Population_Text") {
                AutoSize = true,
                Location = new Point(statwidth - barwidth * 3 - barpadding * 6 - 140, 84),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.White,
                    Multiline = false,
                    Font = ResourceManager.Fonts["Section"]
                },
                Text = "POPULATION"
            };

            StatsContainerPanel.Controls.AddRange(pbHappiness, pbAffluence, pbEnvquality, labelMoney, labelPopulation, textMoney, textPopulation);

            ParentPanel.Controls.AddRange(weatherIcon, zoneOption, zoneOptionLabel, ToolsContainerPanel, StatsContainerPanel);
            gui.Controls.Add(ParentPanel);
        }
    }
}
