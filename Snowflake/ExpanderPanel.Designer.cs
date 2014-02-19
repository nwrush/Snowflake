using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

using Snowflake.Modules;

namespace Snowflake
{
    public partial class ExpanderToolbar
    {
        private Panel ParentPanel;
        public Dictionary<string, Button> Buttons { get; protected set; }
    }
}
