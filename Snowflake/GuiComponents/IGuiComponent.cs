using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.UI;

namespace Snowflake.GuiComponents {
    public interface IGuiComponent {

        /// <summary>
        /// Initializes the GUI Component using the given system
        /// </summary>
        /// <param name="system">A MiyagiSystem instance</param>
        void CreateGui(GUI gui);

        /// <summary>
        /// Updates this GUI component, allowing it to track whatever it needs to from the game data
        /// </summary>
        /// <param name="frametime">Time elapsed since last frame, in milliseconds.</param>
        void Update(float frametime);
    }
}
