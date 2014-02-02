using System;
using System.Collections.Generic;
using System.Text;

using Miyagi.Common;
using Miyagi.Common.Data;

using Snowflake.Modules;

namespace Snowflake.GuiComponents {
    public partial class ControlPanel : IGuiComponent {

        private bool expanded = true;
        private float _height;

        public void Initialize() {
            _height = boxheight;
        }

        public void Update(float frametime) {
            if (expanded && ParentPanel.Height < boxheight) {
                _height += ((boxheight - _height) * 0.05f);
                ParentPanel.Height = (int)_height;
                redolayout();
            }
            else if (!expanded && ParentPanel.Height > expanderheight) {
                _height -= ((_height - expanderheight) * 0.05f);
                ParentPanel.Height = (int)_height;
                redolayout();
            }
        }

        private void redolayout() {
            int height = (int)_height;
            expandButton.Location = new Point(0, height - expanderheight);

            quitButton.Location = new Point(0, height - expanderheight - boxwidth);

            saveButton.Location = new Point(0, height - expanderheight - boxwidth * 2 - padding);

            loadButton.Location = new Point(0, height - expanderheight - boxwidth * 3 - padding * 3);

            optionsButton.Location = new Point(0, height - expanderheight - boxwidth * 4 - padding * 5);
            //optionsButton.Text = optionsButton.Location.Y.ToString();
        }

        public void Dispose() {

        }

        public void Expand() {
            expanded = true;
            expandButton.Picture = ResourceManager.Skins["Control"].SubSkins["Control.Expand"];
        }

        public void Contract() {
            expanded = false;
            expandButton.Picture = ResourceManager.Skins["Control"].SubSkins["Control.Contract"];
        }
    }
}
