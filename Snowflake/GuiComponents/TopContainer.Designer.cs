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
        private Panel weatherIcon; //Todo: turn into a button to display the weather

        private Panel ToolsContainerPanel;
        private PictureButton buildButton;
        private PictureButton manageButton;
        private PictureButton governButton;

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

            weatherIcon = new Panel("WO_Icon") {
                Size = new Size(64, 64),
                Location = new Point(20, 25),
                ResizeMode = ResizeModes.None
            };
            weatherIcon.SetBackgroundTexture(ResourceManager.Skins["WeatherIcons"].SubSkins["WeatherIcons.Sunny"]);

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
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build"],
                PictureSize = new Size(picsize, picsize),
                PictureOffset = new Point((boxsize - picsize) / 2, (boxsize - picsize) / 2)
            };
            buildButton.MouseClick += (object sender, MouseButtonEventArgs e) => {
                CityManager.CreateBuildingOnCursor();
            };

            manageButton = new PictureButton("ToolsPanel_btnManage") {
                Location = new Point(panelSize.Width - boxsize * 2 - padding * 3, 0),
                Size = new Size(boxsize, boxsize),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Manage"],
                PictureSize = new Size(picsize, picsize),
                PictureOffset = new Point((boxsize - picsize) / 2, (boxsize - picsize) / 2)
            };
            governButton = new PictureButton("ToolsPanel_btnGovern") {
                Location = new Point(panelSize.Width - boxsize * 3 - padding * 5, 0),
                Size = new Size(boxsize, boxsize),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                Skin = ResourceManager.Skins["ClearButtonSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Govern"],
                PictureSize = new Size(picsize, picsize),
                PictureOffset = new Point((boxsize - picsize) / 2, (boxsize - picsize) / 2)
            };

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

            ParentPanel.Controls.AddRange(weatherIcon, ToolsContainerPanel, StatsContainerPanel);
            gui.Controls.Add(ParentPanel);
        }
    }
}
