using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI.Controls;

using Snowflake.Modules;

namespace Snowflake.GuiComponents
{
    
    public partial class ExpanderToolbar : IGuiToolbar
    {
        private bool expanded = false;
        private bool fullyHide = false;
        private bool vertical = true;
        private bool horizontal { get { return !vertical; } set { vertical = !value; } }
        private float _height;
        private float _width;
        public bool visible = false;
        public Point Location;

        public ExpanderToolbar(bool _vertical, int _boxwidth, int _boxheight, int _padding, int _expandersize)
        {
            vertical = _vertical;
            boxwidth = _boxwidth;
            boxheight = _boxheight;
            padding = _padding;
            expandersize = _expandersize;

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
        public int Height()
        {
            return ParentPanel.Height;
        }
        /// <summary>
        /// Shows (expands) the toolbar
        /// </summary>
        public void Show() { Expand(); visible = true; }
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
        public void Hide() { Contract(); visible = false; }
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
                kvp.Value.Location = new Point(0, (int)_height - expandersize - boxwidth * i - padding * (2 * i - 1));
                if (kvp.Key == "Expand") { continue; }
                ++i;
            }
        }

        public void Update(float frametime)
        {
            this.ParentPanel.Location = this.Location;
            if (vertical)
            {
                if (expanded && ParentPanel.Height < boxheight)
                {
                    _height += ((boxheight - _height) * 0.05f);
                    ParentPanel.Height = (int)_height;
                    RedoLayout();
                }
                else if (!expanded && ParentPanel.Height > (fullyHide ? 0 : expandersize))
                {
                    _height -= ((_height - (fullyHide ? 0 : expandersize)) * 0.05f);
                    ParentPanel.Height = (int)_height;
                    RedoLayout();
                }
            }
            else if (horizontal)
            {
                if (expanded && ParentPanel.Width < boxwidth)
                {
                    _width += ((boxwidth - _width) * 0.05f);
                    ParentPanel.Width = (int)_width;
                    RedoLayout();
                }
                else if (!expanded && ParentPanel.Width > (fullyHide ? 0 : expandersize))
                {
                    _width -= ((_width - (fullyHide ? 0 : expandersize)) * 0.05f);
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
