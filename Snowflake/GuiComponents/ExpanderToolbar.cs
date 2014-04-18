using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI.Controls;

using Snowflake.Modules;

namespace Snowflake.GuiComponents
{
    
    public partial class ExpanderToolbar
    {
        private bool expanded = false;
        private bool fullyHide = false;
        private bool vertical = true;
        private bool horizontal { get { return !vertical; } set { vertical = !value; } }
        private float _height;
        private float _width;
        private int _basex;
        private int _basey;
        public bool visible = false;
        public bool fullyHidden = true;
        public bool reverse = false;
        public Point Location;

        public event EventHandler Hidden;
        public event EventHandler FullyHidden;
        public event EventHandler Shown;
        public event EventHandler FullyShown;

        public ExpanderToolbar(bool _vertical, int _boxwidth, int _boxheight, int _padding, int _expandersize, bool _reverse = false)
        {
            vertical = _vertical;
            boxwidth = _boxwidth;
            boxheight = _boxheight;
            padding = _padding;
            expandersize = _expandersize;
            reverse = _reverse;
            if (expandersize <= 0) { fullyHide = true; }

            Buttons = new Dictionary<string, Miyagi.UI.Controls.Button>();
            Children = new Dictionary<string, ExpanderToolbar>();
        }
        public ExpanderToolbar() : this(true, 48, 48, 3, 32)  { }

        public void Initialize()
        {
            _height = 0;
            _width = 0;
            _basex = Location.X;
            _basey = Location.Y;
            if (vertical) { ParentPanel.Height = (fullyHide ? 0 : expandersize); }
            if (horizontal) { ParentPanel.Width = (fullyHide ? 0 : expandersize); }
            RedoLayout();
        }
        public int Height
        {
            get { return ParentPanel.Height; }
        }
        public int Width
        {
            get { return ParentPanel.Width; }
        }
        /// <summary>
        /// Shows (expands) the toolbar
        /// </summary>
        public void Show() { Expand(); if (Shown != null) { Shown.Invoke(this, new EventArgs()); } }
        /// <summary>
        /// Expands the toolbar
        /// </summary>
        public void Expand()
        {
            visible = true;
            expanded = true;
            fullyHidden = false;
            if (Buttons.ContainsKey("Expand")) { ((PictureButton)Buttons["Expand"]).Picture = ResourceManager.Skins["Control"].SubSkins["Control.Expand"]; }
        }
        /// <summary>
        /// Hides (contracts) the toolbar
        /// </summary>
        public void Hide() { Contract(); if (Hidden != null) { Hidden.Invoke(this, new EventArgs()); } }
        /// <summary>
        /// Contracts the toolbar
        /// </summary>
        public void Contract()
        {
            expanded = false;
            visible = false; 
            if (Buttons.ContainsKey("Expand")) { ((PictureButton)Buttons["Expand"]).Picture = ResourceManager.Skins["Control"].SubSkins["Control.Contract"]; }
            foreach (ExpanderToolbar child in this.Children.Values)
            {
                child.Contract();
            }
        }
        /// <summary>
        /// Adds a button
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="button">The button instance to add</param>
        /// <param name="child">An optional child toolbar to associate with the button</param>
        public void AddButton(string name, Button button, ExpanderToolbar child = null)
        {
            if (child != null)
            {
                Children.Add(name, child);
                child.CreateGui(ParentPanel.GUI);
                button.Click += (object sender, EventArgs e) =>
                {
                    if (child.visible) { child.Hide(); }
                    else { child.Show(); }
                };
            }
            //ParentPanel.Controls.Add(button);
            Buttons.Add(name, button);
        }
        /// <summary>
        /// Adds a child toolbar
        /// </summary>
        /// <param name="name">Name of the child toolbar</param>
        /// <param name="child">The ExpanderToolbar instance to set as child</param>
        public void AddChild(string name, ExpanderToolbar child)
        {
            Children.Add(name, child);
            child.CreateGui(ParentPanel.GUI);
            if (Buttons.ContainsKey(name))
            {
                Buttons[name].Click += (object sender, EventArgs e) =>
                {
                    if (child.visible) { child.Hide(); }
                    else { child.Show(); }
                };
            }
        }

        public Button GetButton(string name) { return this.Buttons[name]; }
        public ExpanderToolbar GetChild(string name) { return this.Children[name]; }

        protected void RedoLayout()
        {
            int i = reverse ? 0 : 1;
            foreach (KeyValuePair<string, Button> kvp in Buttons)
            {
                if (vertical) {
                    if (reverse)
                    {
                        kvp.Value.Location = new Point(0, boxwidth * i + padding * (2 * i - 1));
                    }
                    else
                    {
                        kvp.Value.Location = new Point(0, (int)_height - expandersize - boxwidth * i - padding * (2 * i - 1));
                    }
                }
                else if (horizontal) {
                    if (reverse)
                    {
                        kvp.Value.Location = new Point(boxheight * i + padding * (2 * i - 1), 0);
                    }
                    else
                    {
                        kvp.Value.Location = new Point((int)_width - expandersize - boxheight * i - padding * (2 * i - 1), 0);
                    }
                }
                if (kvp.Key == "Expand") { continue; }
                ++i;
            }
            if (horizontal) {
                if (fullyHide && ParentPanel.Width <= 2)
                {
                    ParentPanel.BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle()
                    {
                        Thickness = new Miyagi.Common.Data.Thickness(0, 0, 0, 0)
                    };
                }
                else {
                    ParentPanel.BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle()
                    {
                        Thickness = new Miyagi.Common.Data.Thickness(reverse ? 1 : 0, 0, 0, 1)
                    };
                }
            }
        }
        private bool childrenContracted()
        {
            foreach (ExpanderToolbar child in this.Children.Values)
            {
                if (!child.fullyHidden) { return false; }
            }
            return true;
        }
        public void Update(float frametime)
        {
            this.ParentPanel.Location = this.Location;
            if (vertical)
            {
                if (expanded && ParentPanel.Height < boxheight)
                {
                    _height = Math.Min(_height + 4 * frametime, boxheight); //((boxheight - _height) * 0.05f);
                    if (_height >= boxheight)
                    {
                        if (FullyShown != null) { FullyShown.Invoke(this, new EventArgs()); }
                    }
                    ParentPanel.Height = (int)_height;
                    RedoLayout();
                }
                else if (!expanded && ParentPanel.Height > (fullyHide ? 0 : expandersize) && childrenContracted())
                {
                    _height = Math.Max(_height - 4 * frametime, 0); //((_height - (fullyHide ? 0 : expandersize)) * 0.05f);
                    if ((int)_height <= (fullyHide ? 0 : expandersize))
                    {
                        if (FullyHidden != null) { FullyHidden.Invoke(this, new EventArgs()); }
                        fullyHidden = true;
                    }
                    ParentPanel.Height = (int)_height;
                    RedoLayout();
                }
            }
            else if (horizontal)
            {
                if (expanded && ParentPanel.Width < boxwidth)
                {
                    _width = Math.Min(_width + 4 * frametime, boxwidth); //((boxwidth - _width) * 0.05f);
                    if (_width >= boxwidth)
                    {
                        if (FullyShown != null) { FullyShown.Invoke(this, new EventArgs()); }
                    }
                    ParentPanel.Width = (int)_width;
                    RedoLayout();
                }
                else if (!expanded && ParentPanel.Width > (fullyHide ? 0 : expandersize) && childrenContracted())
                {
                    _width = Math.Max(_width - 4 * frametime, 0); // ((_width - (fullyHide ? 0 : expandersize)) * 0.05f);
                    if ((int)_width <= (fullyHide ? 0 : expandersize))
                    {
                        if (FullyHidden != null) { FullyHidden.Invoke(this, new EventArgs()); }
                        fullyHidden = true;
                    }
                    ParentPanel.Width = (int)_width;
                    RedoLayout();
                }
                if (reverse) { ParentPanel.Left = (int)(_basex - ParentPanel.Width); }
            }

            foreach (ExpanderToolbar child in this.Children.Values)
            {
                child.Update(frametime);
            }
        }

        public void Dispose()
        {

        }
    }
}
