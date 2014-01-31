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

            int padding = 20;
            Size panelSize = new Size(100 * 3 + padding * 6, 120);
            ToolsContainerPanel = new Panel("ToolsPanel_ParentPanel") {
                Location = new Point(gw - 350 - panelSize.Width, 0),
                Size = panelSize,
                Movable = false,
                Throwable = false,
                ResizeMode = ResizeModes.None
            };

            buildButton = new PictureButton("ToolsPanel_btnBuild") {
                Location = new Point(panelSize.Width - 100 - padding, 0),
                Size = new Size(100, 100),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                PictureSize = new Size(80, 80),
                PictureOffset = new Point(10, 10),
                //Skin = ResourceManager.Skins["BlackPanelSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build"],
                PictureHover = ResourceManager.Skins["Tools"].SubSkins["Tools.Build.MouseEnter"]
            };
            manageButton = new PictureButton("ToolsPanel_btnManage") {
                Location = new Point(panelSize.Width - 200 - padding * 3, 0),
                Size = new Size(100, 100),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                PictureSize = new Size(80, 80),
                PictureOffset = new Point(10, 10),
                //Skin = ResourceManager.Skins["BlackPanelSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Build"],
                PictureHover = ResourceManager.Skins["Tools"].SubSkins["Tools.Build.MouseEnter"]
            };
            governButton = new PictureButton("ToolsPanel_btnGovern") {
                Location = new Point(panelSize.Width - 300 - padding * 5, 0),
                Size = new Size(100, 100),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.BottomCenter
                },
                PictureSize = new Size(80, 80),
                PictureOffset = new Point(10, 10),
                //Skin = ResourceManager.Skins["BlackPanelSkin"],
                Picture = ResourceManager.Skins["Tools"].SubSkins["Tools.Govern"],
                PictureHover = ResourceManager.Skins["Tools"].SubSkins["Tools.Govern.MouseEnter"]
            };

            ToolsContainerPanel.Controls.AddRange(buildButton, manageButton, governButton);

            ///

            StatsContainerPanel = new Panel("Stats_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(300, 110),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Opacity = 0.7f,
                Location = new Point(gw - 300 - 10, 10),
                ResizeThreshold = new Thickness(0)
                /*BorderStyle = {
                    Thickness = new Thickness(4, 4, 4, 4)
                },*/
                //Skin = ResourceManager.Skins["PanelSkin"]
            };

            pbHappiness = new ProgressBar("Stats_Happiness_Bar") {
                Size = new Size(16, 80),
                Location = new Point(10, 10),
                ProgressBarStyle = new ProgressBarStyle {
                    Orientation = Orientation.Vertical,
                    Mode = ProgressBarMode.Continuous
                },
                Skin = ResourceManager.Skins["ProgressBarVSkin"],
                ToolTipText = "Happiness"
            };

            pbAffluence = new ProgressBar("Stats_Affluence_Bar") {
                Size = new Size(16, 80),
                Location = new Point(36, 10),
                ProgressBarStyle = new ProgressBarStyle {
                    Orientation = Orientation.Vertical,
                    Mode = ProgressBarMode.Continuous
                },
                Skin = ResourceManager.Skins["ProgressBarVSkin"],
                ToolTipText = "Affluence"
            };

            pbEnvquality = new ProgressBar("Stats_Envquality_Bar") {
                Size = new Size(16, 80),
                Location = new Point(62, 10),
                ProgressBarStyle = new ProgressBarStyle {
                    Orientation = Orientation.Vertical,
                    Mode = ProgressBarMode.Continuous
                },
                Skin = ResourceManager.Skins["ProgressBarVSkin"],
                ToolTipText = "Pollution"
            };

            labelMoney = new Label("Stats_Money_Label") {
                AutoSize = true,
                Location = new Point(98, 10),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.White,
                    Multiline = false
                },
                Text = "$"
            };

            labelPopulation = new Label("Stats_Population_Label") {
                AutoSize = true,
                Location = new Point(98, 30),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.White,
                    Multiline = false
                },
                Text = "Population: "
            };

            StatsContainerPanel.Controls.AddRange(pbHappiness, pbAffluence, pbEnvquality, labelMoney, labelPopulation);

            ParentPanel.Controls.AddRange(weatherIcon, ToolsContainerPanel, StatsContainerPanel);
            gui.Controls.Add(ParentPanel);
        }
    }
}
