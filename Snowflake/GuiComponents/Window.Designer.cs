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
    public abstract partial class Window {

        protected Panel ParentPanel;

        public virtual void CreateGui(GUI gui) {
            ParentPanel = new Panel(this.GetType().ToString() + "_" + this.GetHashCode() + "_ParentPanel") {
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

            ParentPanel.ClientSizeChanged += (object sender, EventArgs e) => {
                foreach (Control c in ParentPanel.Controls) {
                    c.MaxSize = new Size(ParentPanel.Width, ParentPanel.Height);
                }
            };

            Console.WriteLine("Creating Console");
            gui.Controls.Add(ParentPanel);

            Initialize();
        }

        public virtual void Dispose() {
            ParentPanel.GUI.Controls.Remove(ParentPanel);
            foreach (Control c in ParentPanel.Controls) {
                c.Dispose();
            }
            ParentPanel.Dispose();
        }
    }
}
