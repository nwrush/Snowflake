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
                Ray mouseRay = GetSelectionRay(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY);

                Mogre.Pair<bool, float> intersection = mouseRay.Intersects(new Plane(Vector3.UNIT_Y, Vector3.ZERO));
                if (intersection.first && selboxShouldUpate())
                {
                    Vector3 intersectionPt = mouseRay.Origin + mouseRay.Direction * intersection.second;
                    Point plotCoord = CityManager.GetPlotCoords(intersectionPt);
                    Vector3 plotCenter = CityManager.GetPlotCenter(plotCoord);
                    cursorPlane.SetPosition(plotCenter.x, plotCenter.y + 1f, plotCenter.z);

                    if (mouseMode == MouseMode.PlacingBuilding && tempBuilding != null) {
                        tempBuilding.SetPosition(plotCoord.X, plotCoord.Y);
                    }

                    DebugPanel.SetDebugText(CityManager.GetPlotCoords(intersectionPt).ToString());
                }

                //Middle click - rotate the view
                if (mStateMgr.Input.IsMouseButtonDown(MouseButtonID.MB_Middle) && viewShouldUpdate())
                {

                    //Mouse rotate control
                    angle += mStateMgr.Input.MouseMoveX * 0.01f;
                    //mStateMgr.Input += mStateMgr.Input.MouseMoveX; //Enable this if you ever figure out how to prevent the mouse from moving

                    //Mouse drag control //Re-enable if control scheme changes to allow for mouse dragging
                    /*Vector2 mouseMoveRotated = Utils3D.RotateVector2(new Vector2(mStateMgr.Input.MouseMoveX, mStateMgr.Input.MouseMoveY), angle);
                    focalPoint.Translate(new Vector3(mouseMoveRotated.y, 0, mouseMoveRotated.x));
                    mStateMgr.GuiSystem.GUIManager.Cursor.SetActiveMode(CursorMode.ResizeTop);*/

                    if (!ContextMenu.HitTest(MousePosition(mStateMgr.Input)))
                    {
                        ContextMenu.Visible = false;
                    }
                }
                //Right click - context menus
                if (mStateMgr.Input.WasMouseButtonPressed(MouseButtonID.MB_Right))
                {
                    if (ContextMenu.Visible == true && !ContextMenu.HitTest(MousePosition(mStateMgr.Input)))
                    {
                        ContextMenu.Visible = false;
                    }
                    else
                    {
                        ContextMenu.Location = new Point(mStateMgr.Input.MousePosX, mStateMgr.Input.MousePosY);

                        Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                        if (result.first)
                        {
                            CityManager.UpdateSelectionBox(result.second);
                        }
                        if (canSelect())
                        {
                            UpdateSelectionBox();
                            CityManager.MakeSelection();

                            if (CityManager.GetSelectedBuildings().Count > 0)
                            {
                                ContextMenu.AddButton("Properties...", (object sender, EventArgs e) =>
                                {
                                    int i = 0;
                                    foreach (Building b in CityManager.GetSelectedBuildings())
                                    {
                                        BuildingPropertiesWindow bpw = new BuildingPropertiesWindow(b);
                                        bpw.CreateGui(this.GuiMgr.GetGui());
                                        bpw.Location += new Point(i * 24, i * 24);
                                        ++i;
                                    }
                                    ContextMenu.RemoveButton("Properties...");
                                });
                            }
                        }
                        CityManager.ClearSelection();
                        ContextMenu.Visible = true;
                    }
                }

                //Mouse click - 3D selection
                if (mStateMgr.Input.WasMouseButtonPressed(MouseButtonID.MB_Left))
                {
                    if (!ContextMenu.HitTest(MousePosition(mStateMgr.Input)))
                    {
                        ContextMenu.Visible = false;
                        if (canPlaceBuilding()) {
                            //Do a little reflection to be able to pass the type of the cursor building into the generic method
                            MethodInfo method = typeof(CityManager).GetMethod("NewBuilding");
                            MethodInfo newBuilding = method.MakeGenericMethod(this.tempBuilding.Data.GetType());
                            newBuilding.Invoke(null, new object[] { this.tempBuilding.PlotX, this.tempBuilding.PlotY });
                            gConsole.WriteLine("Placing building...");

                            if (!mStateMgr.Input.IsKeyDown(KeyCode.KC_LSHIFT))
                            {
                                //Then dispose the cursor building as the game will be making a new one very shortly
                                CancelBuildingPlacement();
                                mouseMode = MouseMode.Selection;
                            }
                        }

                        if (canSelect())
                        {
                            Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                            if (result.first)
                            {
                                CityManager.SetSelectionOrigin(result.second);
                            }
                        }

                        if (canZone())
                        {
                            Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                            if (result.first)
                            {
                                CityManager.SetScratchZoneOrigin(result.second);
                            }
                        }
                    }
                }

                if (mStateMgr.Input.IsMouseButtonDown(MouseButtonID.MB_Left))
                {
                    if (canSelect())
                    {
                        Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                        if (result.first)
                        {
                            CityManager.UpdateSelectionBox(result.second);
                            UpdateSelectionBox();
                        }
                    }
                    if (canZone())
                    {
                        Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                        if (result.first)
                        {
                            CityManager.UpdateScratchZoneBox(result.second);
                            UpdateScratchZoneBox();
                        }
                    }
                }

                if (mStateMgr.Input.MouseMoveZ != 0.0f)
                {
                    dist += mStateMgr.Input.MouseMoveZ * 0.002f;
                    if (dist < -14.0f) { dist = -14.0f; }
                    if (dist > 0.0f) { dist = 0.0f; }
                }

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
                        int[] mouseModeVals = (int[])(Enum.GetValues(typeof(Haswell.Zones)));
                        CityManager.scratchZoneType = (Haswell.Zones)System.Math.Max(((int)CityManager.scratchZoneType + 1) % mouseModeVals.Length, 1);
                        UpdateScratchZoneBoxZone(CityManager.scratchZoneType);
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
            //Handle Mouse release (this should happen even if you go into a UI element)
            if (mStateMgr.Input.WasMouseButtonReleased(MouseButtonID.MB_Left))
            {
                if (mouseMode == MouseMode.Selection && CityManager.SelectionOriginIsValid())
                {
                    Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                    if (result.first)
                    {
                        CityManager.UpdateSelectionBox(result.second);
                    }
                    UpdateSelectionBox();
                    CityManager.MakeSelection();
                    CityManager.ClearSelection();
                    selectionBox.SetVisible(false);
                }
                if (mouseMode == MouseMode.DrawingZone && CityManager.ScratchZoneOriginIsValid())
                {
                    Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                    if (result.first)
                    {
                        CityManager.UpdateScratchZoneBox(result.second);
                    }
                    UpdateScratchZoneBox();
                    CityManager.MakeZone();
                    CityManager.ClearScratchZone();
                    scratchZone.SetVisible(false);

                }
            }

            // check if the escape key was pressed
            if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_W) && mStateMgr.Input.IsKeyDown(KeyCode.KC_LCONTROL))
            {
                // quit the application
                mStateMgr.RequestShutdown();
            }
        }

        private void CancelBuildingPlacement()
        {
            this.tempBuilding.Dispose();
            this.tempBuilding = null;
            this.GuiMgr.HideBuildingPlacementPanel();
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
