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
    public partial class GameConsole {
        private int labelY;
        private Panel outputPanel;
        public Panel parentPanel { get; private set; }
        private TextBox entryBox;

        public void CreateGui(GUI gui) {

            parentPanel = new Panel("GC_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = true,
                Size = new Size(516, 416),
                Movable = true,
                Location = new Point(200, 100),
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
                Skin = ResourceManager.Skins["PanelSkin"],
                TextStyle = new TextStyle() {
                    Font = ResourceManager.Fonts["Courier"],
                    ForegroundColour = Colours.Black,
                    Multiline = true,
                    Alignment = Alignment.TopLeft
                }

            };

            entryBox = new TextBox("GC_Entrybox") {
                Size = new Size(508, 32),
                Location = new Point(0, 360),
                Padding = new Thickness(9, 0, 8, 0),
                TextStyle = {
                    ForegroundColour = Colours.Black,
                    Font = ResourceManager.Fonts["Courier"]
                },
                TextBoxStyle = {
                    CaretStyle = {
                        Size = new Size(2, 16),
                        Colour = Colours.Black,
                    }
                },
                BorderStyle = {
                    Thickness = new Thickness(2, 2, 2, 2),
                },
                Skin = ResourceManager.Skins["PanelSkin"],
                ClearTextOnSubmit = true,
                DefocusOnSubmit = false
            };

            entryBox.Submit += (object sender, ValueEventArgs<string> e) => { this.Command(((TextBox)sender).Text); };
            entryBox.GotFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = true; };
            entryBox.LostFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = false; };
            entryBox.AutoCompleteSource = this.commands.Keys;

            parentPanel.ClientSizeChanged += (object sender, EventArgs e) => {
                entryBox.Width = parentPanel.Width - parentPanel.BorderStyle.Thickness.Left - parentPanel.BorderStyle.Thickness.Right;
                entryBox.Bottom = parentPanel.Height - parentPanel.BorderStyle.Thickness.Bottom - parentPanel.BorderStyle.Thickness.Top;
                outputPanel.Width = parentPanel.Width - parentPanel.BorderStyle.Thickness.Left - parentPanel.BorderStyle.Thickness.Right;
                outputPanel.Height = parentPanel.Height - entryBox.Height - parentPanel.BorderStyle.Thickness.Bottom - parentPanel.BorderStyle.Thickness.Top;

                foreach (Control c in outputPanel.Controls) {
                    c.MaxSize = new Size(outputPanel.Width, outputPanel.Height);
                }
            };
            parentPanel.Controls.Add(entryBox);
            parentPanel.Controls.Add(outputPanel);

            Console.WriteLine("Creating Console");
            gui.Controls.Add(parentPanel);

            Initialize();
        }

        public void Dispose() {
            parentPanel.GUI.Controls.Remove(parentPanel);
            foreach (Control c in parentPanel.Controls) {
                c.Dispose();
            }
            parentPanel.Dispose();
        }
    }
}
