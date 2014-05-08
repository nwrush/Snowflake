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
        Skin CellSkin;
        Thickness CellMargin;
        Thickness CellPadding;
        Thickness CellBorder;
        bool AllowCellResize = true;

    }

    public enum GridLayoutSizeMode
    {
        AutoFlow,
        FixedRows,
        FixedColumns,
        Fixed
    }
}
