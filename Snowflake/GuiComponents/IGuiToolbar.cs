using System;
using System.Collections.Generic;
using Miyagi.UI.Controls;

namespace Snowflake.GuiComponents
{
    public interface IGuiToolbar
    {
        public Dictionary<string, Button> Buttons { get; protected set; }
    }
}
