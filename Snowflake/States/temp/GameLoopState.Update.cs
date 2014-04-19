using System;
using System.Reflection;
using System.Linq;

using Mogre;

using Snowflake.Modules;
using Snowflake.GuiComponents;
using Snowflake.GuiComponents.Windows;

using Haswell;

using Miyagi.Common;
using Miyagi.Common.Data;
using Miyagi.UI;
using Miyagi.UI.Controls;

using MOIS;

using Vector3 = Mogre.Vector3;
using Quaternion = Mogre.Quaternion;

namespace Snowflake.States
{
    public partial class GameLoopState
    {

        /// <summary>
        /// Update the game
        /// </summary>
        /// <param name="_frameTime"></param>
        public override void Update(float _frameTime)
        {
            // check if the state was initialized before
            if (StateMgr == null)
                return;

            UpdateCameraPosition();
            HandleInput(StateMgr);

            if (CityManager.Initialized)
            {
                CityManager.Update(_frameTime);
                UpdateGUI(_frameTime);
            }

            WeatherMgr.Update(StateMgr.Engine.SceneMgr);
            DebugPanel.UpdateFPS(_frameTime);
        }

        private void UpdateCameraPosition()
        {
            StateMgr.Engine.Camera.Position = new Vector3(focalPoint.Position.x + (-500) * Mogre.Math.Cos(angle), 500, focalPoint.Position.z + -(500) * Mogre.Math.Sin(angle));
            StateMgr.Engine.Camera.Position += StateMgr.Engine.Camera.Direction * (float)(System.Math.Pow(dist, 3) + 100.0f);
        }

        /// <summary>
        /// Provide input handling during the game.
        /// </summary>
        /// <param name="mStateMgr"></param>
        private void HandleInput(StateManager mStateMgr)
        {
            // get reference to the ogre manager
            OgreManager engine = mStateMgr.Engine;

            //If we're not typing into a form or something...
            if (!StateManager.SupressGameControl)
            {

                if (viewShouldUpdate())
                {
                    //WASD Control
                    int speed = 10;
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_A))
                    {
                        focalPoint.Translate(new Vector3((-dist + speed) * Mogre.Math.Sin(angle), 0, -(-dist + speed) * Mogre.Math.Cos(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_W))
                    {
                        focalPoint.Translate(new Vector3((-dist + speed) * Mogre.Math.Cos(angle), 0, (-dist + speed) * Mogre.Math.Sin(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_D))
                    {
                        focalPoint.Translate(new Vector3(-(-dist + speed) * Mogre.Math.Sin(angle), 0, (-dist + speed) * Mogre.Math.Cos(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_S))
                    {
                        focalPoint.Translate(new Vector3(-(-dist + speed) * Mogre.Math.Cos(angle), 0, -(-dist + speed) * Mogre.Math.Sin(angle)));
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_Q))
                    {
                        angle += 0.01f;
                    }
                    if (mStateMgr.Input.IsKeyDown(KeyCode.KC_E))
                    {
                        angle -= 0.01f;
                    }
                }
                if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_TAB))
                {
                    if (mouseMode == MouseMode.DrawingZone)
                    {
                        CycleDrawnZone();
                    }
                }

                //Toggle the console with `
                if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_GRAVE))
                {
                    gConsole.Visible = !gConsole.Visible;
                }

                if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_DELETE))
                {
                    CityManager.DeleteSelectedBuildings();
                }

                if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_ESCAPE))
                {
                    if (mouseMode == MouseMode.PlacingBuilding) { CancelBuildingPlacement(); }
                    else if (mouseMode == MouseMode.DrawingZone) { 
                        CityManager.ClearScratchZone();
                        scratchZone.SetVisible(false);
                    }
                    else if (mouseMode == MouseMode.Selection)
                    {
                        CityManager.DeselectBuildings();
                    }
                    mouseMode = MouseMode.Selection;
                }
            }

            // check if the escape key was pressed
            if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_W) && mStateMgr.Input.IsKeyDown(KeyCode.KC_LCONTROL))
            {
                // quit the application
                StartShutdown();
            }
        }

        private void UpdateSelectionBox()
        {
            Vector3 center = (CityManager.GetPlotCenter(CityManager.SelectionBox.Left, CityManager.SelectionBox.Top)
                + CityManager.GetPlotCenter(CityManager.SelectionBox.Right, CityManager.SelectionBox.Bottom))
                 * 0.5f;
            selectionBox.SetPosition(center.x, center.y, center.z);
            selectionBox.SetScale(CityManager.SelectionBox.Width * SCALEFACTOR + SCALEFACTOR, SCALEFACTOR / 2.0f, CityManager.SelectionBox.Height * SCALEFACTOR + SCALEFACTOR);
            selectionBox.SetVisible(true);
        }
        private void UpdateScratchZoneBox()
        {
            Vector3 center = (CityManager.GetPlotCenter(CityManager.scratchZoneBox.Left, CityManager.scratchZoneBox.Top)
                + CityManager.GetPlotCenter(CityManager.scratchZoneBox.Right, CityManager.scratchZoneBox.Bottom))
                 * 0.5f;
            scratchZone.SetPosition(center.x, center.y + 1.0f, center.z);
            scratchZone.SetScale(CityManager.scratchZoneBox.Width + 1, 1.0f, CityManager.scratchZoneBox.Height + 1);
            scratchZone.SetVisible(true);
        }
        public void UpdateScratchZoneBoxZone(Zones z)
        {
            scratchZoneEnt.GetSubEntity(0).SetMaterial(
                RenderablePlot.GetZoneColoredMaterial(
                scratchZoneEnt
                .GetSubEntity(0)
                .GetMaterial(),
                z));
        }
        public void UpdateGUI(float frametime)
        {
            if (CityManager.Initialized)
            {
                GuiMgr.SetCurrentCursorBuilding(this.tempBuilding);
                GuiMgr.Update(frametime);
            }
            GuiMgr.DebugPanel[4] = mouseMode.ToString();
        }

        private bool selboxShouldUpate()
        {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null;
        }

        private bool viewShouldUpdate()
        {
            return ContextMenu.Visible == false;
        }

        private bool canZone()
        {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null && mouseMode == MouseMode.DrawingZone;
        }

        private bool canSelect()
        {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null && mouseMode == MouseMode.Selection;
        }

        private bool canPlaceBuilding() 
        
        {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null && mouseMode == MouseMode.PlacingBuilding && tempBuilding != null;
        }
    }
}
