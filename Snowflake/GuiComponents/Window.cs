using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

namespace Snowflake.GuiComponents {
    public abstract partial class Window : IGuiComponent {

        public bool HasCloseButton = true;

        public virtual void Initialize() {

        }

        /// <summary>
        /// Checks if the specified point overlaps this Window's area
        /// </summary>
        /// <param name="p">Point to check</param>
        /// <returns>Whether or not the point overlaps this Window's area</returns>
        public bool HitTest(Point p) {
            return this.ParentPanel.HitTest(p);
        }

        /// <summary>
        /// Whether or not this window is visible
        /// </summary>
        public bool Visible {
            get { return this.ParentPanel.Visible; }
            set { 
                this.ParentPanel.Visible = this.WindowLabel.Visible = value;
                this.CloseButton.Visible = HasCloseButton && value;
            }
        }

        public virtual void Show() {
            this.Visible = true;
        }

        public virtual void Hide() {
            this.Visible = false;
        }

        public string Text {
            get { return this.WindowLabel.Text; }
            set { this.WindowLabel.Text = value; }
        }

        public virtual void Update(float _frametime) {

        }
    }
}
