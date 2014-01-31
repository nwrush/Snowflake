using System;
using System.Collections.Generic;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.Common.Events;
using Miyagi.UI;
using Miyagi.UI.Controls;
using Miyagi.UI.Controls.Styles;

using Snowflake.GuiComponents;

namespace Snowflake {
    public class GuiManager {
        private GUI Gui;
        private GameConsole gConsole;
        private BuildingCreationWindow bcWindow;
        private StatsPanel statsPanel;
        private ToolsPanel toolsPanel;
        private WeatherOverlay weatherOverlay;
        private DebugPanel debugPanel;
        private ContextMenu contextMenu;

        #region Properties

        public DebugPanel DebugPanel {
            get { return debugPanel; }
            set { debugPanel = value; }
        }
        public GameConsole GameConsole {
            get { return gConsole; }
            set { gConsole = value; }
        }
        public WeatherOverlay WeatherOverlay {
            get { return weatherOverlay; }
            set { weatherOverlay = value; }
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
            statsPanel = new StatsPanel();
            toolsPanel = new ToolsPanel();
            weatherOverlay = new WeatherOverlay();
            DebugPanel = new DebugPanel();
            ContextMenu = new ContextMenu();
            bcWindow = new BuildingCreationWindow();

            GuiComponents.AddRange(new IGuiComponent[] { gConsole, statsPanel, toolsPanel, weatherOverlay, debugPanel, contextMenu, bcWindow });

            Gui = new GUI();
            GuiSystem.GUIManager.GUIs.Add(Gui);
            this.InitGui(Gui);
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

        public void Update(float frametime) {
            foreach (IGuiComponent c in GuiComponents) {
                c.Update(frametime);
            }
        }
    }
}
