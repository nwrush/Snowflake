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
    //Menu for holding tools and stuff
    public class ToolPanel {
        private Panel parentPanel;

        public void CreateGui(MiyagiSystem system) {

            var gui = new GUI();

            //store game width and height
            int gw = system.RenderManager.MainViewport.Size.Width;
            int gh = system.RenderManager.MainViewport.Size.Height;
            
            parentPanel = new Panel("Tools_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(600, 100),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Opacity = 0.7f,
                Location = new Point((gw - 600) / 2, (gh - 120)),
                ResizeThreshold = new Thickness(0),
                BorderStyle = {
                    Thickness = new Thickness(4, 4, 4, 4)
                },
                Skin = ResourceManager.Skins["PanelSkin"]
            };
            Button button1 = new Button("Tools_Button1") {
                TabStop = false,
                TabIndex = 0,
                Size = new Size(80, 80),
                Location = new Point(10, 10),
                Skin = ResourceManager.Skins["SquareButtonSkin"]
            };
            PictureBox picBox = new PictureBox("Tools_Button1_Img") {
                TabStop = false,
                TabIndex = 0,
                Size = new Size(64, 64),
                Location = new Point(8, 8)
            };

            parentPanel.Controls.Add(button1);

            Console.WriteLine("Creating Tools Menu");
            gui.Controls.Add(parentPanel);

            // add the GUI to the GUIManager
            system.GUIManager.GUIs.Add(gui);
        }
    }
}
