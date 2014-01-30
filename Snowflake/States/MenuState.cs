using System;

using Miyagi;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

using Snowflake.GuiComponents;
using Snowflake.Modules;

namespace Snowflake.States {
    public class MenuState : State {

        private StateManager StateMgr;

        public MenuState() {
            StateMgr = null;
        }

        public override bool Startup(StateManager _mgr) {
            StateMgr = _mgr;

            CreateGui();
            return true;
        }

        private void CreateGui()
        {
            int gw = StateMgr.Engine.Viewport.ActualWidth;
            int gh = StateMgr.Engine.Viewport.ActualHeight;

            GUI gui = new GUI();
            StateMgr.GuiSystem.GUIManager.GUIs.Add(gui);

            Size logosize = new Size((int)(Math.Max(512, gw - 400)), (int)Math.Max(128, (gw - 400) / 4.0));
            Panel logo = new Panel("menu_logo")
            {
                Skin = ResourceManager.Skins["Logo"],
                Size = logosize,
                Location = new Point((int)((gw - logosize.Width) / 2), 100),
                ResizeMode = ResizeModes.None,
                Throwable = false,
                Movable = false,
            };
            Panel logobg = new Panel("menu_logobg") {
                BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle() {
                    Thickness = new Thickness(0, 1, 0, 1)
                },
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                Size = new Size(gw, logosize.Height + 50),
                Location = new Point(0, 75)
            };
            logobg.SetBackgroundTexture(ResourceManager.Skins["BlackPanelSkin"].SubSkins["BlackPanelSkin40"]);

            gui.Controls.Add(logo);
        }

        public override void Update(float _frameTime) {

        }

        public override void Shutdown() {
            
        }
    }
}
