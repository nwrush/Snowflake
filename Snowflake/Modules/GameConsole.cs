using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

namespace Snowflake.Modules {
    public class GameConsole {

        private int labelY;
        private Panel panel1;

        public void CreateGui(MiyagiSystem system) {

            var gui = new GUI();

            this.panel1 = new Panel("Panel1") {
                TabStop = false,
                TabIndex = 0,
                Throwable = true,
                Size = new Size(768, 300),
                Location = new Point(0, 0),
                MinSize = new Size(0, 0),
                ResizeThreshold = new Thickness(0),
                BorderStyle = {
                    Thickness = new Thickness(8, 16, 8, 8)
                },
                HScrollBarStyle = {
                    Extent = 16,
                    ThumbStyle = {
                        BorderStyle = {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                VScrollBarStyle = {
                    Extent = 16,
                    ThumbStyle = {
                        BorderStyle = {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                Skin = ResourceManager.Skins["PanelSkin"]
            };

            var textBox1 = new TextBox("TextBox1") {
                Size = new Size(768, 32),
                Location = new Point(0, 300),
                Padding = new Thickness(9, 0, 8, 0),
                TextStyle = {
                    ForegroundColour = Colours.White
                },
                TextBoxStyle = {
                    CaretStyle = {
                        Size = new Size(2, 16),
                        Colour = Colours.White
                    }
                },
                Skin = ResourceManager.Skins["ButtonSkin"],
                ClearTextOnSubmit = true
            };
            textBox1.Submit += this.TextBox1Submit;

            Console.WriteLine("Adding textbox and panel to gui");
            gui.Controls.Add(textBox1);
            gui.Controls.Add(panel1);

            // add the GUI to the GUIManager
            system.GUIManager.GUIs.Add(gui);

            AddLabel("> TEST");
        }

        private void AddLabel(string text) {
            var label = new Label {
                Location = new Point(0, labelY),
                Text = text,
                AutoSize = true,
                MaxSize = new Size(275, 300)
            };
            label.SuccessfulHitTest += (s, e) => e.Cancel = true;
            this.panel1.Controls.Add(label);
            this.labelY += label.Size.Height;
            this.panel1.ScrollToBottom();
        }

        private void TextBox1Submit(object sender, Miyagi.Common.Events.ValueEventArgs<string> e) {
            this.AddLabel(((TextBox)sender).Text);
        }


    }
}
