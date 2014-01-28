using System;
using System.Collections.Generic;

using Snowflake.Modules;

using Miyagi;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

namespace Snowflake.GuiComponents {
    public partial class ContextMenu : IGuiComponent {

        private List<Control> items;
        private static int i;

        public ContextMenu() {
            items = new List<Control>();
        }

        public void Initialize() {
            this.Visible = false;
        }

        /// <summary>
        /// Add a control to this context menu (usually a button)
        /// </summary>
        /// <param name="c">The control to add to this menu</param>
        public void AddControl(Control c) {
            int height = 0;
            foreach (Control ctrl in parentPanel.Controls) { height += ctrl.Height; }
            if (c.Width > parentPanel.Width) {
                parentPanel.Width = c.Width;
                foreach (Control ctrl in parentPanel.Controls) { ctrl.Width = c.Width; }
            }
            else {
                c.Width = parentPanel.Width;
            }
            c.Top = height;
            parentPanel.Height = height + c.Height;

            items.Add(c);
            parentPanel.Controls.Add(c);

            if (c is Button) { c.Click += contextMenuClick; }
        }

        /// <summary>
        /// Remove a control from this context menu
        /// </summary>
        /// <param name="c">Control to remove</param>
        public void RemoveControl(Control c) {
            parentPanel.Controls.Remove(c);
            parentPanel.Height -= c.Height;
            items.Remove(c);
            c.Click -= contextMenuClick;
        }

        /// <summary>
        /// Adds a button with the specified text and click event to this context menu
        /// </summary>
        /// <param name="text">String to display on the button</param>
        /// <param name="clickEvent">Event handler for button click</param>
        public void AddButton(string text, EventHandler clickEvent) {
            Button b = new Button("CM_btn" + (++i)) {
                Skin = ResourceManager.Skins["ContextButtonSkin"],
                Size = new Size(parentPanel.Width, 25),
                Text = text,
                Padding = new Thickness(5, 0, 5, 0)
            };
            b.Click += clickEvent;
            this.AddControl(b);
        }

        /// <summary>
        /// Removes the button with the specified label from this context menu
        /// </summary>
        /// <param name="text">String displayed on the desired button</param>
        /// <returns>Whether or not the operation was successful</returns>
        public bool RemoveButton(string text) {
            foreach (Control c in parentPanel.Controls) {
                if (c is Button && ((Button)c).Text == text) {
                    RemoveControl(c);
                    return true;
                }
            }
            return false;
        }

        private void contextMenuClick(object sender, EventArgs e) { parentPanel.Visible = false; }

        public void Update(float frametime) {

        }

        /// <summary>
        /// Returns whether or not the context menu is currently visible
        /// </summary>
        public bool Visible {
            get { return this.parentPanel.Visible; }
            set { 
                parentPanel.Visible = value;
                parentPanel.Enabled = value;
            }
        }

        public Point Location {
            get { return this.parentPanel.Location; }
            set { this.parentPanel.Location = value; }
        }

        public bool HitTest(Point p) {
            return parentPanel.HitTest(p);
        }
    }
}
