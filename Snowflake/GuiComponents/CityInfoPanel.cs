using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;

namespace Snowflake.GuiComponents {
    public partial class CityInfoPanel : IGuiComponent {

        private string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public void Initialize() {
            
        }

        public void Update(float frametime) {
            UpdateTimeLabel(Haswell.Controller.CurrentTime);
            cityLabel.Text = CityManager.CityName;
            //timeLabel.Location = new Point((ParentPanel.Width - timeLabel.Width - 10), timeLabel.Location.X);
            //cityLabel.Location = new Point((ParentPanel.Width - cityLabel.Width - 10), timeLabel.Location.X);
        }

        /// <summary>
        /// Update this overlay's time label with the new datetime.
        /// </summary>
        /// <param name="newTime">The new date and time</param>
        private void UpdateTimeLabel(DateTime newTime) {
            this.timeLabelShadow.Text = this.timeLabel.Text = newTime.DayOfWeek + ", " + newTime.Day + " " + Months[newTime.Month - 1] + ", " + newTime.Year; // + " - " + newTime.Hour + ":" + (newTime.Minute < 10 ? ("0" + newTime.Minute.ToString()) : newTime.Minute.ToString());
        }

        public void Dispose() {
            timeLabel.GUI.Controls.Remove(timeLabel);
            timeLabel.Dispose();
        }
    }
}
