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
        public Styles.GridLayoutStyle GridLayoutStyle;

        public GridLayoutPanel()
            : base() {
            create();
        }
        public GridLayoutPanel(string name) : base(name) {
            create();
        }
        private void create()
        {
            GridLayoutStyle = new Styles.GridLayoutStyle();
        }

        protected override void OnControlAdded(Miyagi.Common.Events.ValueEventArgs<Control> e)
        {
            base.OnControlAdded(e);
            this.PerformLayout();
        }

        protected override void OnControlRemoved(Miyagi.Common.Events.ValueEventArgs<Control> e)
        {
            base.OnControlRemoved(e);
            this.PerformLayout();
        }

        protected override void OnLayout(Miyagi.UI.Controls.Layout.LayoutEventArgs e)
        {
            base.OnLayout(e);

            //Ignore the Grid Layout Style for now because implementing things that have been promised is hard
            _cols = Math.Max(this.Width / GridLayoutStyle.CellSize.Width, 1);
            _rows = this.Controls.Count / _cols;
            int r = 0;
            int c = 0;
            foreach (Control con in this.Controls)
            {
                con.Width = GridLayoutStyle.CellSize.Width;
                con.Height = GridLayoutStyle.CellSize.Height;
                con.Left = c * GridLayoutStyle.CellSize.Width;
                con.Top = r * GridLayoutStyle.CellSize.Height;
                if (con is SkinnedControl)
                {
                    ((SkinnedControl)con).Skin = GridLayoutStyle.CellSkin;
                }
                ++c;
                if (c % _cols == 0)
                {
                    c = 0;
                    ++r;
                }
            }
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
