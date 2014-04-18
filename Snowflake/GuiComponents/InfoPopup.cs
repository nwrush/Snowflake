using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.GuiComponents
{
    public partial class InfoPopup : IGuiComponent
    {
        private string _text;
        public float TimeToLive;
        public InfoPopup(string text = " ")
        {
            this._text = text;
        }
        public void Initialize()
        {
            return;
        }

        public int Y
        {
            get { return this.ParentPanel.Top; }
            set { this.ParentPanel.Top = value; }
        }

        public string Text
        {
            get { return this._text; }
            set { this._text = value; this.mainButton.Text = value; }
        }

        public void Update(float frametime)
        {
            TimeToLive -= frametime;
            return;
        }

        public void Dispose()
        {
            ParentPanel.Controls.Clear();
            ParentPanel.Dispose();
        }
    }
}
