using System;
using System.Collections.Generic;
using System.Linq;

using Snowflake.Modules;

using Miyagi;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

using Haswell;

using Snowflake.GuiComponents.Windows;

namespace Snowflake.GuiComponents {
    public partial class ContextMenu : IGuiComponent {

        private Dictionary<string, ContextMenuItem> items;
        private static int i;

        public ContextMenu() {
            items = new Dictionary<string, ContextMenuItem>();
        }

        public void Initialize() {
            this.Visible = false;

            this.AddButton("Properties", (object sender, EventArgs e) =>
            {
                int i = 0;
                foreach (Building b in CityManager.GetSelectedBuildings())
                {
                    BuildingPropertiesWindow bpw = new BuildingPropertiesWindow(b);
                    bpw.CreateGui(this.parentPanel.GUI);
                    bpw.Location += new Point(i * 24, i * 24);
                    ++i;
                }
            });
        }

        public void ShowControl(string name)
        {
            if (items.ContainsKey(name)) { items[name].Visible = true; }
        }
        public void HideControl(string name)
        {
            if (items.ContainsKey(name)) { items[name].Visible = false; }
        }

        /// <summary>
        /// Add a control to this context menu (usually a button)
        /// </summary>
        /// <param name="c">The control to add to this menu</param>
        public void AddControl(Control c, string name) {
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

            items.Add(name, new ContextMenuItem() { Control = c, Name = name });
            parentPanel.Controls.Add(c);

            if (c is Button) { c.Click += contextMenuClick; }
        }

        /// <summary>
        /// Remove a control from this context menu
        /// </summary>
        /// <param name="c">Control to remove</param>
        public void RemoveControl(Control c) {
            if (items.ContainsKey(c.Name))
            {
                c.Dispose();
                parentPanel.Controls.Remove(c);
                parentPanel.Height = parentPanel.Height - c.Height;
                if (parentPanel.Height <= 0)
                {
                    parentPanel.Height = 200;
                }
                items.Remove(c.Name);
                c.Click -= contextMenuClick;
            }
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
            this.AddControl(b, text);
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

    class ContextMenuItem {
        public string Name;
        public bool Visible { get { return Control.Visible; } set { Control.Visible = value; } }
        public Control Control;
    }
}
