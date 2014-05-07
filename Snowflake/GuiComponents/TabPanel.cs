using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Snowflake.Modules;

using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Miyagi.Common.Data;

namespace Snowflake.GuiComponents
{
    public class TabPanel : Panel
    {
        public Dictionary<Panel, string> TabPages { get; private set; }
        public Dictionary<string, Button> TabButtons { get; private set; }
        public TabBarStyle TabStyle;
        public Panel ActiveTab;

        public TabPanel()
            : base() {
            create();
        }
        public TabPanel(string name) : base(name) {
            create();
        }
        private void create()
        {
            TabButtons = new Dictionary<string, Button>();
            TabPages = new Dictionary<Panel, string>();
            TabStyle = new TabBarStyle()
            {
                Extent = 16
            };
        }

        protected override void OnControlAdded(Miyagi.Common.Events.ValueEventArgs<Control> e)
        {
            if (e.Data is Panel && TabPages.ContainsKey((Panel)e.Data))
            {
                base.OnControlAdded(e);
                AddButtonForTabPage((Panel)e.Data);
                this.PerformLayout();
            }
            else if (e.Data is Button && TabButtons.ContainsValue((Button)e.Data))
            {
                base.OnControlAdded(e);
            }
            else
            {
                throw new InvalidOperationException("Only panels may be added to tab panels!");
            }
        }

        public virtual void AddPage(Panel p, string title)
        {
            TabPages[p] = title;
            this.Controls.Add(p);
            p.ResizeMode = Miyagi.UI.ResizeModes.None;
            p.ResizeThreshold = new Thickness(0, 0, 0, 0);
        }

        public virtual void FocusPage(string name)
        {
            var pages = TabPages.Where(item => item.Value == name);
            foreach (KeyValuePair<Panel, string> kvp in pages)
            {
                FocusPage(kvp.Key);
                break;
            }
        }
        public virtual void FocusPage(Panel p)
        {
            if (TabPages.ContainsKey(p))
            {
                p.BringToFront();
            }
        }

        protected virtual void AddButtonForTabPage(Panel p)
        {
            Button b = new Button(p.Name + "_button")
            {
                Text = TabPages[p],
                Height = TabStyle.Extent,
                Width = (int)(this.ClientSize.Width / (TabButtons.Count + 1.0)),
                Left = 0,
                TextStyle = new TextStyle()
                {
                    ForegroundColour = TabStyle.ForegroundColour,
                    Font = TabStyle.Font,
                    Alignment = TabStyle.Alignment
                },
                Skin = ResourceManager.Skins["ButtonSkin"]
            };
            b.Click += (object sender, EventArgs e) =>
            {
                this.FocusPage(p);
            };
            TabButtons[TabPages[p]] = b;
            this.Controls.Add(b);
        }

        protected override void OnLayout(Miyagi.UI.Controls.Layout.LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (this.IsDisposed) { return; }

            int tabwidth = 100;
            int tabcount = TabPages.Count;
            switch (TabStyle.Mode)
            {
                case Miyagi.UI.TabMode.FixedSize:
                    tabwidth = TabStyle.Extent * 3;
                    break;
                case Miyagi.UI.TabMode.Fill:
                    tabwidth = tabcount > 0 ? this.ClientSize.Width / tabcount : 100;
                    break;
                case Miyagi.UI.TabMode.AutoSize:
                    if (TabStyle.Extent * 3 * tabcount < this.ClientSize.Width)
                    {
                        tabwidth = TabStyle.Extent * 3;
                    }
                    else
                    {
                        tabwidth = this.ClientSize.Width / tabcount;
                    }
                    break;
            }
            int i = 0;
            foreach (Panel p in this.TabPages.Keys)
            {
                if (p.IsDisposed) { continue; }
                p.Width = this.ClientSize.Width;
                p.Height = this.ClientSize.Height - this.TabStyle.Extent;
                p.Top = TabStyle.Extent;

                Button b = TabButtons[this.TabPages[p]];
                b.TextStyle.Font = TabStyle.Font;
                
                if (p == ActiveTab) { b.TextStyle.ForegroundColour = TabStyle.SelectColour; }
                else if (p.IsMouseOver) { b.TextStyle.ForegroundColour = TabStyle.HoverColour; }
                else { b.TextStyle.ForegroundColour = TabStyle.ForegroundColour; }

                b.Width = tabwidth;
                b.Height = TabStyle.Extent;
                b.Left = i * tabwidth;
                ++i;
            }
        }

        protected override void DoResize(double widthFactor, double heightFactor)
        {
            base.DoResize(widthFactor, heightFactor);
        }
    }
}
