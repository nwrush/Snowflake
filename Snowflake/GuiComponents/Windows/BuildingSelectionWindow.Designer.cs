using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Snowflake.Modules;

using Haswell.Buildings;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Miyagi.UI;

namespace Snowflake.GuiComponents.Windows
{
    public partial class BuildingSelectionWindow
    {
        private TabPanel tabs;
        private Panel residentialPanel;
        private Panel commercialPanel;
        private Panel industrialPanel;
        private Panel infrastructurePanel;

        public override void CreateGui(GUI gui)
        {
            base.CreateGui(gui);

            ParentPanel.Size = new Size(375, 450);
            ParentPanel.TextStyle = new TextStyle()
            {
                Alignment = Alignment.TopRight,
                ForegroundColour = new Colour(255, 192, 192, 192),
                Font = ResourceManager.Fonts["Subtitle"],
                Offset = new Point(-10, 10)
            };

            tabs = new TabPanel()
            {
                TabStyle = new TabBarStyle()
                {
                    Extent = 24,
                    ForegroundColour = Colours.Black,
                    Mode = TabMode.Fill,
                    Alignment = Alignment.MiddleCenter
                },
                Skin = ResourceManager.Skins["PanelSkin"],
                Size = new Size(300, 300),
                Dock = DockStyle.Fill
            };

            residentialPanel = new GridLayoutPanel()
            {
                BorderStyle =
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                },
                HScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                VScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                Skin = ResourceManager.Skins["TabPageSkin"],
                TextStyle = new TextStyle()
                {
                    Font = ResourceManager.Fonts["Subheading"],
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Alignment = Alignment.TopLeft
                },
                Text = "Residential Buildings"
            };

            commercialPanel = new Panel()
            {
                BorderStyle =
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                },
                HScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                VScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                Skin = ResourceManager.Skins["TabPageSkin"],
                TextStyle = new TextStyle()
                {
                    Font = ResourceManager.Fonts["Subheading"],
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Alignment = Alignment.TopLeft
                },
                Text = "Commercial Buildings"
            };
            industrialPanel = new Panel()
            {
                BorderStyle =
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                },
                HScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                VScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                Skin = ResourceManager.Skins["TabPageSkin"],
                TextStyle = new TextStyle()
                {
                    Font = ResourceManager.Fonts["Subheading"],
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Alignment = Alignment.TopLeft
                },
                Text = "Industrial Buildings"
            };
            infrastructurePanel = new Panel()
            {
                BorderStyle =
                {
                    Thickness = new Thickness(1, 1, 1, 1)
                },
                HScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                VScrollBarStyle =
                {
                    Extent = 16,
                    ThumbStyle =
                    {
                        BorderStyle =
                        {
                            Thickness = new Thickness(2, 2, 2, 2)
                        }
                    }
                },
                Skin = ResourceManager.Skins["TabPageSkin"],
                TextStyle = new TextStyle()
                {
                    Font = ResourceManager.Fonts["Subheading"],
                    ForegroundColour = Colours.Black,
                    Multiline = false,
                    Alignment = Alignment.TopLeft
                },
                Text = "Infrastructure Buildings"
            };

            tabs.AddPage(residentialPanel, "Residential");
            tabs.AddPage(commercialPanel, "Commercial");
            tabs.AddPage(industrialPanel, "Industrial");
            tabs.AddPage(infrastructurePanel, "Infrastructure");

            ParentPanel.Controls.Add(tabs);


            foreach (ResidentialTypes t in Enum.GetValues(typeof(ResidentialTypes)))
            {
                //Residential b = new Residential(t);
                residentialPanel.Controls.Add(new Button()
                {
                    Text = t.ToString()
                });
            }
        }
    }
}
