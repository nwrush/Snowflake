using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.GuiComponents
{
    public partial class InfoPopup : IGuiComponent
    {
        private string _text;
        public InfoPopup(string text = " ")
        {
            this._text = text;
        }
        public void Initialize()
        {
            return;
        }

        public string Text
        {
            get { return this._text; }
            set { this._text = value; this.mainButton.Text = value; }
        }

        public void Update(float frametime)
        {
            return;
        }

        public void Dispose()
        {
            return;
        }
    }
}
