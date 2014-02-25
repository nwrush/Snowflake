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
    public abstract partial class Window {

        protected Panel ParentPanel;
        protected Button CloseButton;
        protected Label WindowLabel;

        public virtual void CreateGui(GUI gui) {
            ParentPanel = new Panel(this.GetType().ToString() + "_" + this.GetHashCode() + "_ParentPanel") {
                TabStop = false,
                TabIndex = 0,
                Throwable = true,
                Size = new Size(200, 200),
                Movable = true,
                Location = new Point(200, 200),
                MinSize = new Size(4, 4),
                ResizeThreshold = new Thickness(4),
                BorderStyle = {
                    Thickness = new Thickness(4, 24, 4, 4)
                },
                Skin = ResourceManager.Skins["WindowSkin"],
                TextStyle = new TextStyle {
                    Alignment = Alignment.TopCenter
                }
            };
            WindowLabel = new Label(this.GetType().ToString() + "_" + this.GetHashCode() + "_WindowLabel") {
                Location = new Point(204, 202),
                TextStyle = new TextStyle() { 
                    ForegroundColour = Colours.White
                },
                MaxSize = new Size(ParentPanel.Width - 4, ParentPanel.BorderStyle.Thickness.Top),
                AutoSize = true,
                HitTestVisible = false
            };
            CloseButton = new Button(this.GetType().ToString() + "_" + this.GetHashCode() + "_CloseButton") {
                Location = ParentPanel.Location + new Point(ParentPanel.Size.Width - 24, 0),
                Size = new Size(24, 24),
                Text = "X",
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.MiddleCenter
                },
                Skin = ResourceManager.Skins["BlackPanelSkin"]
            };
            CloseButton.SetBackgroundTexture(CloseButton.Skin.SubSkins["BlackPanelSkin0"]);

            CloseButton.MouseHover += (object sender, MouseEventArgs e) => {
                CloseButton.SetBackgroundTexture(CloseButton.Skin.SubSkins["BlackPanelSkin40"]);
            };
            CloseButton.MouseLeave += (object sender, MouseEventArgs e) => {
                CloseButton.SetBackgroundTexture(CloseButton.Skin.SubSkins["BlackPanelSkin0"]);
            };
            CloseButton.Click += (object sender, EventArgs e) => {
                this.Hide();
            };

            ParentPanel.ClientSizeChanged += (object sender, EventArgs e) => {
                foreach (Control c in ParentPanel.Controls) {
                    c.MaxSize = new Size(ParentPanel.Width, ParentPanel.Height);
                }
                WindowLabel.MaxSize = new Size(ParentPanel.Width - 28, ParentPanel.BorderStyle.Thickness.Top);

                WindowLabel.Location = ParentPanel.Location + new Point(4, 2);
                CloseButton.Location = ParentPanel.Location + new Point(ParentPanel.Size.Width - 24, 0);
                keepWindowSorting();
            };
            ParentPanel.LocationChanged += (object sender, ChangedValueEventArgs<Point> e) => {
                WindowLabel.Location = e.NewValue + new Point(4, 2);
                CloseButton.Location = e.NewValue + new Point(ParentPanel.Size.Width - 24, 0);
                keepWindowSorting();
            };

            ParentPanel.ControlAdded += (object sender, ValueEventArgs<Control> e) => {
                
                e.Data.Click += (object sender2, EventArgs e2) => { keepWindowSorting(); };
                e.Data.MouseDown += (object sender2, MouseButtonEventArgs e2) => { keepWindowSorting(); };
                e.Data.MouseHeld += (object sender2, MouseButtonEventArgs e2) => { keepWindowSorting(); };
            };

            Console.WriteLine("Creating Console");
            gui.Controls.Add(ParentPanel);
            gui.Controls.Add(WindowLabel);
            gui.Controls.Add(CloseButton);

            Initialize();
        }

        public virtual void Dispose() {
            if (ParentPanel.GUI != null)
            {
                ParentPanel.GUI.Controls.Remove(ParentPanel);
            }
                foreach (Control c in ParentPanel.Controls)
                {
                    c.Dispose();
                }
                try { ParentPanel.Dispose(); }
                catch (NullReferenceException e)
                {
                    DebugPanel.ActiveInstance[2] = e.Message;
                }
        }

        private void keepWindowSorting() {
            WindowLabel.BringToFront();
            CloseButton.BringToFront();
        }
    }
}
