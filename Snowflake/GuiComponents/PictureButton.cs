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
        private Texture _pictureHover;
        private Size _pictureSize;
        private Point _pictureOffset;
        public Action<object> ClickFunc;

        public PictureButton()
            : base() {
            create();
        }
        public PictureButton(string name) : base(name) {
            create();
        }
        private void create()
        {
            this.ParentChanged += (object sender, EventArgs e) => { CreatePictureBox(); this.Parent.Controls.Add(this.picturePanel); };
            _pictureSize = this.Size;
            this.Click += (object sender, EventArgs e) => { if (ClickFunc != null) { ClickFunc.Invoke(sender); }  };
        }

        private void CreatePictureBox() {
            //create the child panel element to display the texture on
            this.picturePanel = new Panel() {
                Size = (_pictureSize.Width == 0 && _pictureSize.Height == 0 ? this.Size : _pictureSize),
                Location = this.Location + new Point(this.Padding.Left, this.Padding.Top) + _pictureOffset,
                Movable = false,
                Throwable = false,
                ResizeMode = ResizeModes.None,
                HitTestVisible = false,
            };

            //bind event handlers for button hover and leave
            if (this._picture != null) { this.picturePanel.SetBackgroundTexture(this._picture); }
            this.MouseEnter += (object sender, MouseEventArgs e) => {
                if (_pictureHover != null) { this.picturePanel.SetBackgroundTexture(this._pictureHover); }
            };
            this.MouseLeave += (object sender, MouseEventArgs e) => {
                if (_picture != null) { this.picturePanel.SetBackgroundTexture(this._picture); }
            };
            this.LocationChanged += (object sender, ChangedValueEventArgs<Point> e) => {
                if (_picture != null) {
                    picturePanel.Location = e.NewValue + new Point(this.Padding.Left, this.Padding.Top) + _pictureOffset;
                }
            };
        }

        public Texture Picture {
            get { return this._picture; }
            set { 
                this._picture = value;
                if (this.picturePanel != null) { this.picturePanel.SetBackgroundTexture(value); }
            }
        }
        public Texture PictureHover {
            get { return this._pictureHover; }
            set {
                this._pictureHover = value;
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
