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
            float gw = StateMgr.Engine.Camera.Viewport.Width;
            float gh = StateMgr.Engine.Camera.Viewport.Height;

            GUI gui = new GUI();
            StateMgr.GuiSystem.GUIManager.GUIs.Add(gui);

            Size logosize = new Size((int)(Math.Min(512, gw - 200)), (int)Math.Min(256, (gw - 200) / 2.0));
            Panel logo = new Panel("menu_logo")
            {
                Skin = ResourceManager.Skins["Logo"],
                Size = logosize,
                Location = new Point((int)((gw - logosize.Width) / 2), 100),
                ResizeMode = ResizeModes.None,
                Throwable = false,
                Movable = false,
                BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle()
                {
                    Thickness = new Thickness(0, 1, 0, 1)
                }
            };
        }

        public override void Update(float _frameTime) {

        }

        public override void Shutdown() {
            
        }
    }
}
