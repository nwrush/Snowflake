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

using Haswell;

namespace Snowflake.GuiComponents {
    //Menu for holding tools and stuff
    public class StatsPanel : IGuiComponent {
        private Panel parentPanel;
        private ProgressBar pbHappiness;
        private ProgressBar pbAffluence;
        private ProgressBar pbEnvquality;
        private Label labelMoney;
        private Label labelPopulation;

        public void CreateGui(GUI gui) {

            //store game width and height
            int gw = gui.MiyagiSystem.RenderManager.MainViewport.Size.Width;
            int gh = gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;

            parentPanel = new Panel("Stats_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(300, 110),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Opacity = 0.7f,
                Location = new Point(gw - 300 - 10, 10),
                ResizeThreshold = new Thickness(0),
                BorderStyle = {
                    Thickness = new Thickness(4, 4, 4, 4)
                },
                Skin = ResourceManager.Skins["PanelSkin"]
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
                    ForegroundColour = Colours.Black,
                    Multiline = false
                },
                Text = "$"
            };

            labelPopulation = new Label("Stats_Population_Label") {
                AutoSize = true,
                Location = new Point(98, 30),
                TextStyle = new TextStyle() {
                    Alignment = Alignment.TopLeft,
                    ForegroundColour = Colours.Black,
                    Multiline = false
                },
                Text = "Population: "
            };

            parentPanel.Controls.Add(pbHappiness);
            parentPanel.Controls.Add(pbAffluence);
            parentPanel.Controls.Add(pbEnvquality);
            parentPanel.Controls.Add(labelMoney);
            parentPanel.Controls.Add(labelPopulation);

            Console.WriteLine("Creating Tools Menu");
            gui.Controls.Add(parentPanel);
        }

        public void Update(float frametime) {
            //Update progress bar values with tracked stats from CityManager.

            pbHappiness.Value = 50;
            pbAffluence.Value = 67;
            pbEnvquality.Value = 23;

            labelMoney.Text = "$4,375.64";
            labelPopulation.Text = "Population: 82";

            Dictionary<Resource.Type, int> resources = Haswell.Controller.City.Resources;
            labelMoney.Text = resources[Resource.Type.Money].ToString("C");
        }
    }
}
