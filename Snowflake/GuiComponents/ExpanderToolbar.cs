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
        public bool visible = false;
        public Point Location;

        public event EventHandler Hidden;
        public event EventHandler FullyHidden;
        public event EventHandler Shown;
        public event EventHandler FullyShown;

        public ExpanderToolbar(bool _vertical, int _boxwidth, int _boxheight, int _padding, int _expandersize)
        {
            vertical = _vertical;
            boxwidth = _boxwidth;
            boxheight = _boxheight;
            padding = _padding;
            expandersize = _expandersize;
            if (expandersize <= 0) { fullyHide = true; }

            buttons = new Dictionary<string, Miyagi.UI.Controls.Button>();
        }
        public ExpanderToolbar() : this(true, 48, 48, 3, 32)  { }

        public void Initialize()
        {
            _height = 0;
            _width = 0;
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
        public void Show() { Expand(); visible = true; if (Shown != null) { Shown.Invoke(this, new EventArgs()); } }
        /// <summary>
        /// Expands the toolbar
        /// </summary>
        public void Expand()
        {
            expanded = true;
            if (buttons.ContainsKey("Expand")) { ((PictureButton)buttons["Expand"]).Picture = ResourceManager.Skins["Control"].SubSkins["Control.Expand"]; }
        }
        /// <summary>
        /// Hides (contracts) the toolbar
        /// </summary>
        public void Hide() { Contract(); visible = false; if (Hidden != null) { Hidden.Invoke(this, new EventArgs()); } }
        /// <summary>
        /// Contracts the toolbar
        /// </summary>
        public void Contract()
        {
            expanded = false;
            if (buttons.ContainsKey("Expand")) { ((PictureButton)buttons["Expand"]).Picture = ResourceManager.Skins["Control"].SubSkins["Control.Contract"]; }
        }
        /// <summary>
        /// Returns whether or not the toolbar is visible
        /// </summary>
        /// <returns>Bool: is the toolbar visible?</returns>
        public bool Visible() { return visible; }

        protected void RedoLayout()
        {
            int i = 1;
            foreach (KeyValuePair<string, Button> kvp in buttons)
            {
                if (vertical) { kvp.Value.Location = new Point(0, (int)_height - expandersize - boxwidth * i - padding * (2 * i - 1)); }
                else if (horizontal) { kvp.Value.Location = new Point((int)_width - expandersize - boxheight * i - padding * (2 * i - 1), 0); }
                if (kvp.Key == "Expand") { continue; }
                ++i;
            }
            if (horizontal) {
                if (fullyHide && ParentPanel.Width <= 2)
                {
                    ParentPanel.BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle()
                    {
                        Thickness = new Miyagi.Common.Data.Thickness(0, 1, 0, 1)
                    };
                }
                else {
                    ParentPanel.BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle()
                    {
                        Thickness = new Miyagi.Common.Data.Thickness(0, 1, 1, 1)
                    };
                }
            }
        }
        public void Update(float frametime)
        {
            this.ParentPanel.Location = this.Location;
            if (vertical)
            {
                if (expanded && ParentPanel.Height < boxheight)
                {
                    _height = Math.Min(_height + 4, boxheight); //((boxheight - _height) * 0.05f);
                    if (_height >= boxheight)
                    {
                        if (FullyShown != null) { FullyShown.Invoke(this, new EventArgs()); }
                    }
                    ParentPanel.Height = (int)_height;
                    RedoLayout();
                }
                else if (!expanded && ParentPanel.Height > (fullyHide ? 0 : expandersize))
                {
                    _height = Math.Max(_height - 4, 0); //((_height - (fullyHide ? 0 : expandersize)) * 0.05f);
                    if ((int)_height <= (fullyHide ? 0 : expandersize))
                    {
                        if (FullyHidden != null) { FullyHidden.Invoke(this, new EventArgs()); }
                    }
                    ParentPanel.Height = (int)_height;
                    RedoLayout();
                }
            }
            else if (horizontal)
            {
                if (expanded && ParentPanel.Width < boxwidth)
                {
                    _width = Math.Min(_width + 4, boxwidth); //((boxwidth - _width) * 0.05f);
                    if (_width >= boxwidth)
                    {
                        if (FullyShown != null) { FullyShown.Invoke(this, new EventArgs()); }
                    }
                    ParentPanel.Width = (int)_width;
                    RedoLayout();
                }
                else if (!expanded && ParentPanel.Width > (fullyHide ? 0 : expandersize))
                {
                    _width = Math.Max(_width - 4, 0); // ((_width - (fullyHide ? 0 : expandersize)) * 0.05f);
                    if ((int)_width <= (fullyHide ? 0 : expandersize))
                    {
                        if (FullyHidden != null) { FullyHidden.Invoke(this, new EventArgs()); }
                    }
                    ParentPanel.Width = (int)_width;
                    RedoLayout();
                }
            }
        }

        public void Dispose()
        {

        }
    }
}
