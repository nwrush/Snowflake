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
    public class GameConsole {

        private int labelY;
        private Panel outputPanel;
        private Panel parentPanel;

        public void CreateGui(MiyagiSystem system) {

            var gui = new GUI();

            parentPanel = new Panel("GC_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = true,
                Size = new Size(516, 416),
                Movable = true,
                Location = new Point(10, 10),
                MinSize = new Size(4, 4),
                ResizeThreshold = new Thickness(4),
                BorderStyle = {
                    Thickness = new Thickness(4, 24, 4, 4)
                },
                Skin = ResourceManager.Skins["WindowSkin"]
            };
            this.outputPanel = new Panel("GC_OutputPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = true,
                Size = new Size(516, 360),
                Movable = false,
                ResizeMode = ResizeModes.None,
                Location = new Point(0, 0),
                MinSize = new Size(0, 0),
                BorderStyle = {
                    Thickness = new Thickness(1, 1, 1, 1)
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

            var textBox1 = new TextBox("GC_Entrybox") {
                Size = new Size(508, 32),
                Location = new Point(0, 360),
                Padding = new Thickness(9, 0, 8, 0),
                TextStyle = {
                    ForegroundColour = Colours.Black
                },
                TextBoxStyle = {
                    CaretStyle = {
                        Size = new Size(2, 16),
                        Colour = Colours.Black
                    }
                },
                Skin = ResourceManager.Skins["TextBoxSkin"],
                ClearTextOnSubmit = true
            };

            textBox1.Submit += (object sender, ValueEventArgs<string> e) => { this.AddLabel(((TextBox)sender).Text); };
            textBox1.GotFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = true; };
            textBox1.LostFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = false; };

            parentPanel.ClientSizeChanged += (object sender, EventArgs e) => {
                textBox1.Width = parentPanel.Width - parentPanel.BorderStyle.Thickness.Left - parentPanel.BorderStyle.Thickness.Right;
                textBox1.Bottom = parentPanel.Height - parentPanel.BorderStyle.Thickness.Bottom - parentPanel.BorderStyle.Thickness.Top;
                outputPanel.Width = parentPanel.Width - parentPanel.BorderStyle.Thickness.Left - parentPanel.BorderStyle.Thickness.Right;
                outputPanel.Height = parentPanel.Height - textBox1.Height - parentPanel.BorderStyle.Thickness.Bottom - parentPanel.BorderStyle.Thickness.Top;
            };
            parentPanel.Controls.Add(textBox1);
            parentPanel.Controls.Add(outputPanel);

            Console.WriteLine("Creating Console");
            gui.Controls.Add(parentPanel);

            // add the GUI to the GUIManager
            system.GUIManager.GUIs.Add(gui);

            AddLabel("TEST");
            this.Hide();
        }

        public void Show() {
            this.parentPanel.Visible = true;
        }

        public void Hide() {
            this.parentPanel.Visible = false;
        }

        public bool Visible { get { return this.parentPanel.Visible; } set { this.parentPanel.Visible = value; } }

        public void WriteLine(string text) {
            this.AddLabel(text);
        }

        private void AddLabel(string text) {
            var label = new Label {
                Location = new Point(0, labelY),
                Text = "> " + text,
                AutoSize = true,
                MaxSize = new Size(275, 300)
            };
            label.SuccessfulHitTest += (s, e) => e.Cancel = true;
            this.outputPanel.Controls.Add(label);
            this.labelY += label.Size.Height;
            this.outputPanel.ScrollToBottom();
        }

    }
}
