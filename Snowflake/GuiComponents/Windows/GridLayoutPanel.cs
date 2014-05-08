using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Miyagi.UI.Controls;

namespace Snowflake.GuiComponents.Windows
{
    public class GridLayoutPanel : Panel
    {
        private int _rows;
        private int _cols;

        public GridLayoutPanel()
            : base() {
            create();
        }
        public GridLayoutPanel(string name) : base(name) {
            create();
        }
        private void create()
        {

        }

        protected override void OnControlAdded(Miyagi.Common.Events.ValueEventArgs<Control> e)
        {
            base.OnControlAdded(e);
        }

        protected override void OnControlRemoved(Miyagi.Common.Events.ValueEventArgs<Control> e)
        {
            base.OnControlRemoved(e);
        }

        protected override void OnLayout(Miyagi.UI.Controls.Layout.LayoutEventArgs e)
        {
            base.OnLayout(e);
            
        }

        public int Rows
        {
            get { return _rows; }
        }

        public int Columns
        {
            get { return _cols; }
        }
    }
}
