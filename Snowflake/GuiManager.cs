using System;
using System.Collections.Generic;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

using Snowflake.GuiComponents;
using Snowflake.GuiComponents.Windows;

namespace Snowflake {
    public class GuiManager {
        private GUI Gui;
        private GameConsole gConsole;
        private BuildingCreationWindow bcWindow;
        private ControlPanel ctrlPanel;
        private CityInfoPanel cityPanel;
        private DebugPanel debugPanel;
        private ContextMenu contextMenu;
        private TopContainer topContainer;
        private BuildingPlacementPanel bldgPlacePanel;
        private WeatherInfoWindow weatherInfo;

        #region Properties

        public DebugPanel DebugPanel {
            get { return debugPanel; }
            set { debugPanel = value; }
        }
        public GameConsole GameConsole {
            get { return gConsole; }
            set { gConsole = value; }
        }
        public ContextMenu ContextMenu {
            get { return contextMenu; }
            set { contextMenu = value; }
        }

        #endregion

        public List<IGuiComponent> GuiComponents;

        public GuiManager() {
            GuiComponents = new List<IGuiComponent>();
        }

        public void CreateDefaultGui(MiyagiSystem GuiSystem) {
            gConsole = new GameConsole();
            ctrlPanel = new ControlPanel();
            cityPanel = new CityInfoPanel();
            DebugPanel = new DebugPanel();
            ContextMenu = new ContextMenu();
            bcWindow = new BuildingCreationWindow();
            topContainer = new TopContainer();
            bldgPlacePanel = new BuildingPlacementPanel();
            weatherInfo = new WeatherInfoWindow();

            GuiComponents.AddRange(new IGuiComponent[] { gConsole, topContainer, ctrlPanel, cityPanel,  debugPanel, contextMenu, bcWindow, bldgPlacePanel, weatherInfo});

            Gui = new GUI();
            GuiSystem.GUIManager.GUIs.Add(Gui);

            this.InitGui(Gui);
        }

        public void AddInfoPopup(string text, float ttl = 2000)
        {
            InfoPopup ip = new InfoPopup(text);
            ip.CreateGui(Gui);
            ip.TimeToLive = ttl;
            GuiComponents.Add(ip);
        }

        public void InitGui(GUI gui) {
            foreach (IGuiComponent c in GuiComponents) {
                c.CreateGui(gui);
            }
            /*ContextMenu.AddButton("Zone as...", (object source, EventArgs e) => {
                Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(ContextMenu.Location);
                if (result.first) {
                    if (!CityManager.Initialized) {
                        CityManager.Init(result.second);
                        Point newPos = result.second - CityManager.GetOrigin();

                    }
                    else {
                        
                    }
                }
                ContextMenu.Visible = false;
            });*/
        }

        public void ShowWeatherPanel()
        {
            weatherInfo.Show();
        }

        public GUI GetGui()
        {
            return this.Gui;
        }

        public void SetCurrentCursorBuilding(RenderableBuilding b)
        {
            bldgPlacePanel.SetRenderBldg(b);
        }

        public void HideBuildingPlacementPanel()
        {
            bldgPlacePanel.Visible = false;
        }
        public void ShowBuildingPlacementPanel()
        {
            bldgPlacePanel.Visible = true;
        }

        public void SetDebugText(string text) {
            debugPanel.SetDebugText(text);
        }
        public void ConsoleWrite(string text) {
            gConsole.WriteLine(text);
        }

        public void Update(float frametime) {
            foreach (IGuiComponent c in GuiComponents) {
                c.Update(frametime);
            }
            int i = 1;
            int gh = Gui.MiyagiSystem.RenderManager.MainViewport.Size.Height;
            foreach (InfoPopup ip in GuiComponents.FindAll(item => item is InfoPopup))
            {
                ip.Y = gh - i * ip.ParentPanel.Height;
                if (ip.TimeToLive <= 0)
                {
                    ip.Dispose();
                    GuiComponents.Remove(ip);
                }
                ++i;
            }
        }
    }
}
