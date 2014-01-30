using System;
using System.Collections.Generic;
using System.Text;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.Common.Resources;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public class PictureButton : Button {

        private Panel picturePanel;
        private Texture _picture;
        private Size _pictureSize;
        private Point _pictureOffset;

        public PictureButton()
            : base() {
                this.ParentChanged += (object sender, EventArgs e) => { CreatePictureBox(); this.Parent.Controls.Add(this.picturePanel); };
                _pictureSize = this.Size;
        }
        public PictureButton(string name) : base(name) {
            this.ParentChanged += (object sender, EventArgs e) => { CreatePictureBox(); this.Parent.Controls.Add(this.picturePanel); };
            _pictureSize = this.Size;
        }

        private void CreatePictureBox() {
            this.picturePanel = new Panel() {
                Size = _pictureSize,
                Location = this.Location + new Point(this.Padding.Left, this.Padding.Top) + _pictureOffset,
                Movable = false,
                Throwable = false,
                ResizeMode = ResizeModes.None,
                HitTestVisible = false,
            };
            if (this._picture != null) { this.picturePanel.SetBackgroundTexture(this._picture); }
        }

        public Texture Picture {
            get { return this._picture; }
            set { 
                this._picture = value;
                if (this.picturePanel != null) { this.picturePanel.SetBackgroundTexture(value); }
            }
        }

        public Size PictureSize {
            get { return this._pictureSize; }
            set { 
                this._pictureSize = value;
                if (this.picturePanel != null) { this.picturePanel.Size = value; }
            }
        }

        public Point PictureOffset {
            get { return this._pictureOffset; }
            set {
                this._pictureOffset = value;
                if (this.picturePanel != null) { this.picturePanel.Location = this.Location + new Point(this.Padding.Left, this.Padding.Top) + value; }
            }
        }
    }
}
