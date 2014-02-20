using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;

using Snowflake.Modules;

namespace Snowflake.GuiComponents
{
    
    public partial class ExpanderToolbar : IGuiComponent, IGuiToolbar
    {
        private bool expanded = true;
        private bool vertical = true;
        private bool horizontal { get { return !vertical; } set { vertical = !value; } }
        private float _height;
        private float _width;

        public ExpanderToolbar(bool _vertical, int _boxwidth, int _boxheight, int _padding, int _expandersize)
        {
            vertical = _vertical;
            boxwidth = _boxwidth;
            boxheight = _boxheight;
            padding = _padding;
            expandersize = _expandersize;
        }
        //public ExpanderToolbar()

        public void Initialize()
        {
            _height = boxheight;
            _width = boxwidth;
        }

        public void Expand()
        {
            expanded = true;
            ((PictureButton)Buttons["Expand"]).Picture = ResourceManager.Skins["Control"].SubSkins["Control.Expand"];
        }

        public void Contract()
        {
            expanded = false;
            ((PictureButton)Buttons["Expand"]).Picture = ResourceManager.Skins["Control"].SubSkins["Control.Contract"];
        }

    }
}
