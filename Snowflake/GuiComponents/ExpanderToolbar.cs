using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    }
}
