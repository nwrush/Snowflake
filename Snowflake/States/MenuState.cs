using System;
using Miyagi;
using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.Common.Rendering;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;
using Snowflake.GuiComponents;
using Snowflake.Modules;
using Vector3 = Mogre.Vector3;

namespace Snowflake.States {
    public class MenuState : State {

        private StateManager StateMgr;
        private Mogre.SceneNode focalPoint;
        private float angle = 0.78539f;
        private float dist = -7.0f;

        private GameConsole gConsole;

        private Mogre.Light ambient;

        public MenuState() {
            StateMgr = null;
        }

        public override bool Startup(StateManager _mgr) {
            StateMgr = _mgr;
            CreateGui();
            setupCamera(_mgr.Engine);
            createLight(_mgr.Engine.SceneMgr);
            _mgr.Engine.SceneMgr.SetSkyBox(true, "Sky01", 8192, false);
            focalPoint.AttachObject(_mgr.Engine.SceneMgr.CreateEntity(Mogre.SceneManager.PrefabType.PT_CUBE));

            return true;
        }

        private void CreateGui()
        {
            int gw = StateMgr.Engine.Viewport.ActualWidth;
            int gh = StateMgr.Engine.Viewport.ActualHeight;

            GUI gui = new GUI();
            StateMgr.GuiSystem.GUIManager.GUIs.Add(gui);

            Size logosize = new Size((int)(Math.Min(1536, gw - 200)), (int)Math.Min(256, (gw - 200) / 6.0));
            Panel logo = new Panel("menu_logo")
            {
                Skin = ResourceManager.Skins["Logo"],
                Size = logosize,
                Location = new Point((int)((gw - logosize.Width) / 2), 100),
                ResizeMode = ResizeModes.None,
                Throwable = false,
                Movable = false,
                AlwaysOnTop = true
            };
            Panel logobg = new Panel("menu_logobg") {
                BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle() {
                    Thickness = new Thickness(0, 1, 0, 1)
                },
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                Size = new Size(gw, logosize.Height + 50),
                Location = new Point(0, 75),
                Throwable = false,
                Movable = false
            };
            logobg.SetBackgroundTexture(ResourceManager.Skins["BlackPanelSkin"].SubSkins["BlackPanelSkin40"]);

            Button newGameButton = new Button("menu_btnNewGame") {
                Text = "New Game",
                Size = new Size(gw, 32),
                Location = new Point(0, (int)(gh * 0.5f)),
                TextStyle = new TextStyle() {
                    ForegroundColour = Colours.White,
                    Alignment = Alignment.MiddleCenter
                },
                Skin = ResourceManager.Skins["BlackPanelSkin"],
                BorderStyle = new Miyagi.UI.Controls.Styles.BorderStyle() {
                    Thickness = new Thickness(0, 1, 0, 1)
                }
            };
            newGameButton.SetBackgroundTexture(ResourceManager.Skins["BlackPanelSkin"].SubSkins["BlackPanelSkin40"]);

            newGameButton.MouseEnter += (object sender, MouseEventArgs e) => { 
                newGameButton.SetBackgroundTexture(ResourceManager.Skins["BlackPanelSkin"].SubSkins["BlackPanelSkin60"]);
            };
            newGameButton.MouseLeave += (object sender, MouseEventArgs e) => {
                newGameButton.SetBackgroundTexture(ResourceManager.Skins["BlackPanelSkin"].SubSkins["BlackPanelSkin40"]);
            };
            newGameButton.MouseClick += (object sender, MouseButtonEventArgs e) => {
                StartNewGame();
            };

            gui.Controls.Add(logo);
            gui.Controls.Add(logobg);
            gui.Controls.Add(newGameButton);

            gConsole = new GameConsole();
            gConsole.CreateGui(gui);
        }

        private void setupCamera(OgreManager engine) {
            focalPoint = engine.SceneMgr.RootSceneNode.CreateChildSceneNode("menuFocalPoint");
            focalPoint.Position = new Vector3(0, -5, 0);

            engine.Camera.NearClipDistance = 5;
            engine.Camera.FarClipDistance = 16384;
            engine.Camera.AutoAspectRatio = true;
            engine.Camera.SetAutoTracking(true, focalPoint);
        }
        private void createLight(Mogre.SceneManager sm) {
            ambient = sm.CreateLight("menuAmbient");
            ambient.Type = Mogre.Light.LightTypes.LT_DIRECTIONAL;
            ambient.Position = new Vector3(0, 2000, 0);
            ambient.Direction = new Vector3(-0.8365f, -1, 0.5867f);
            ambient.DiffuseColour = new Mogre.ColourValue(0.99f, 0.95f, 0.9f);
            ambient.SpecularColour = Mogre.ColourValue.White;
            ambient.CastShadows = true;
            sm.RootSceneNode.AttachObject(ambient);
        }

        public override void Update(float _frameTime) {
            angle += 0.0003f;
            UpdateCameraPosition();
        }

        private void UpdateCameraPosition() {
            StateMgr.Engine.Camera.Position = new Vector3(focalPoint.Position.x + (-500) * Mogre.Math.Cos(angle), 100 * (float)System.Math.Sin(angle), focalPoint.Position.z + -(500) * Mogre.Math.Sin(angle));
            StateMgr.Engine.Camera.Position += StateMgr.Engine.Camera.Direction * (float)System.Math.Pow(dist, 3);
        }

        public void StartNewGame() {
            ambient.DiffuseColour = ambient.SpecularColour = new Mogre.ColourValue(0, 0, 0);
            ambient.Visible = false;
            ambient.CastShadows = false;
            StateMgr.Engine.SceneMgr.RootSceneNode.RemoveAndDestroyAllChildren();
            StateMgr.GuiSystem.GUIManager.DisposeAllGUIs();
            StateMgr.RequestStateChange(typeof(GameLoopState));
        }

        public override void Shutdown() {
            
        }
    }
}
