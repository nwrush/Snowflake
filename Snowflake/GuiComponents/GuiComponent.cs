using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.UI;

namespace Snowflake.GuiComponents {
    public abstract class GuiComponent {

        protected GUI gui;

        /// <summary>
        /// Initializes the GUI Component using the given system
        /// </summary>
        /// <param name="system">A MiyagiSystem instance</param>
        public virtual void CreateGui(MiyagiSystem system) {
            this.gui = new GUI();
        }
    }
}
