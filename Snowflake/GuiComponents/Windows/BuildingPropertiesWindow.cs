using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Haswell;

namespace Snowflake.GuiComponents.Windows
{
    public partial class BuildingPropertiesWindow : Window
    {

        private Building data;


        public BuildingPropertiesWindow(Building b) : base() 
        {
            SetTrackedBuilding(b);
        }

        public override void Initialize()
        {
            base.Initialize();

            this.CloseButton.Click += (object sender, EventArgs e) =>
            {
                this.Dispose();
            };
        }

        public void SetTrackedBuilding(Building b)
        {
            data = b;

            //Assign event handlers for when this window is closed and if the building gets deleted
            //(These panels will be newly created each time you click "properties" on a building, so
            //there's no need to keep them around in memory)
            b.Deleted += (object sender, BuildingEventArgs e) =>
            {
                this.Dispose();
            };
        }
        /// <summary>
        /// Update controls with the newly available information 
        /// <remarks>Assumes CreateGui has been called.</remarks>
        /// </summary>
        public void UpdateBuildingInfo()
        {
            if (data != null)
            {
                ParentPanel.Text = data.GetType().ToString().Substring(data.GetType().ToString().LastIndexOf('.') + 1).ToUpper();
            }
        }
    }
}
