using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.GuiComponents
{
    
    public class ExpanderToolbar : IGuiComponent, IGuiToolbar
    {
        private bool expanded = true;
        private bool vertical = true;
        private bool horizontal { get { return !vertical; } set { vertical = !value; } }
        private float _height;
        private float _width;

        public ExpanderToolbar(bool vertical, int boxwidth, int boxheight, int padding, int expandersize)
        {
            //vertical
        }

        public void Initialize()
        {
            //_height = boxheight;
            //_width = boxwidth;
        }

    }
}
