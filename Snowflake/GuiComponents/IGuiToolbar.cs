using System;
using System.Collections.Generic;
using Miyagi.UI.Controls;

namespace Snowflake.GuiComponents
{
    public interface IGuiToolbar : IGuiComponent
    {
        Dictionary<string, Button> Buttons();
        void Show();
        void Hide();
        bool Visible();
    }
}
