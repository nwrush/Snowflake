using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.UI;

namespace Snowflake.GuiComponents {
    public interface IGuiComponent {

        /// <summary>
        /// Creates the GUI Component controls on the given GUI instance
        /// </summary>
        /// <param name="gui">GUI system to add to</param>
        void CreateGui(GUI gui);

        /// <summary>
        /// Initializes the component, allowing it to perform startup tasks.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Updates this GUI component, allowing it to track whatever it needs to from the game data
        /// </summary>
        /// <param name="frametime">Time elapsed since last frame, in milliseconds.</param>
        void Update(float frametime);

        /// <summary>
        /// Runs tasks necessary to delete references to resource and dispose of instanced objects
        /// </summary>
        void Dispose();
    }
}
