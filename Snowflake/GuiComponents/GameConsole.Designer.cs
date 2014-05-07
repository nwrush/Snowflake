using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Miyagi.UI.Controls.Layout;

using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public partial class GameConsole {
        private int labelY;
        private Panel outputPanel;
        private TextBox entryBox;

        public override void CreateGui(GUI gui) {
            base.CreateGui(gui);

            this.HasCloseButton = true;
            this.Text = "Console";

            this.ParentPanel.Size = new Size(516, 416);

            this.outputPanel = new Panel("GC_OutputPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = false,
                Size = new Size(508, 360),
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
                },
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
                DefocusOnSubmit = false,
            };

            entryBox.Submit += (object sender, ValueEventArgs<string> e) => { this.Command(((TextBox)sender).Text); };
            entryBox.GotFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = true; };
            entryBox.LostFocus += (object sender, EventArgs e) => { StateManager.SupressGameControl = false; };
            entryBox.AutoCompleteSource = this.commands.Keys;

            ParentPanel.ClientSizeChanged += (object sender, EventArgs e) => {
                entryBox.Width = ParentPanel.Width - ParentPanel.BorderStyle.Thickness.Left - ParentPanel.BorderStyle.Thickness.Right;
                entryBox.Bottom = ParentPanel.Height - ParentPanel.BorderStyle.Thickness.Bottom - ParentPanel.BorderStyle.Thickness.Top;
                outputPanel.Width = ParentPanel.Width - ParentPanel.BorderStyle.Thickness.Left - ParentPanel.BorderStyle.Thickness.Right;
                outputPanel.Height = ParentPanel.Height - entryBox.Height - ParentPanel.BorderStyle.Thickness.Bottom - ParentPanel.BorderStyle.Thickness.Top;

                foreach (Control c in outputPanel.Controls) {
                    c.MaxSize = new Size(outputPanel.Width, outputPanel.Height);
                }
            };
            ParentPanel.Controls.Add(entryBox);
            ParentPanel.Controls.Add(outputPanel);
        }
    }
}
