using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Miyagi.Common.Resources;
using Miyagi.Common.Data;

namespace Snowflake.GuiComponents.Styles
{
    public class GridLayoutStyle
    {
        public Skin CellSkin;
        public Thickness CellMargin;
        public Thickness CellPadding;
        public Thickness CellBorder;
        public Size CellSize;
        public bool AllowCellResize = true;
        public GridLayoutSizeMode SizeMode = GridLayoutSizeMode.AutoFlow;

        public GridLayoutStyle()
        {
            CellMargin = new Thickness(0, 0, 0, 0);
            CellPadding = new Thickness(0, 0, 0, 0);
            CellBorder = new Thickness(0, 0, 0, 0);
            CellSize = new Size(16, 16);
        }

    }

    public enum GridLayoutSizeMode
    {
        AutoFlow,
        FixedRows,
        FixedColumns,
        Fixed
    }
}
