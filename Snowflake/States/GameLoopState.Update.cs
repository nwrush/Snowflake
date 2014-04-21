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

namespace Snowflake.States {
    public partial class GameLoopState {

        /// <summary>
        /// Update the game
        /// </summary>
        /// <param name="_frameTime"></param>
        public override void Update(float _frameTime) {
            // check if the state was initialized before
            if (StateMgr == null)
                return;

            UpdateCameraPosition();
            HandleInput(StateMgr);

            if (CityManager.Initialized) {
                CityManager.Update(_frameTime);
                UpdateGUI(_frameTime);
            }

            WeatherMgr.Update(StateMgr.Engine.SceneMgr);
            DebugPanel.UpdateFPS(_frameTime);
        }

        private void UpdateCameraPosition() {
            StateMgr.Engine.Camera.Position = new Vector3(focalPoint.Position.x + (-500) * Mogre.Math.Cos(angle), 500, focalPoint.Position.z + -(500) * Mogre.Math.Sin(angle));
            StateMgr.Engine.Camera.Position += StateMgr.Engine.Camera.Direction * (float)(System.Math.Pow(dist, 3) + 100.0f);
        }

        /// <summary>
        /// Provide input handling during the game.
        /// </summary>
        /// <param name="mStateMgr"></param>
        private void HandleInput(StateManager mStateMgr) {
            // get reference to the ogre manager
            OgreManager engine = mStateMgr.Engine;
            MoisManager input = mStateMgr.Input;

            HandleMouseMove(input);
            if (mStateMgr.Input.WasMouseButtonPressed(MouseButtonID.MB_Left)) { HandleLeftMousePressed(input); }
            if (mStateMgr.Input.WasMouseButtonPressed(MouseButtonID.MB_Middle)) { HandleMiddleMousePressed(input); }
            if (mStateMgr.Input.WasMouseButtonPressed(MouseButtonID.MB_Right)) { HandleRightMousePressed(input); }
            if (mStateMgr.Input.IsMouseButtonDown(MouseButtonID.MB_Left)) { HandleLeftMouseHeld(input); }
            if (mStateMgr.Input.IsMouseButtonDown(MouseButtonID.MB_Middle)) { HandleMiddleMouseHeld(input); }
            if (mStateMgr.Input.IsMouseButtonDown(MouseButtonID.MB_Right)) { HandleRightMouseHeld(input); }
            if (mStateMgr.Input.WasMouseButtonReleased(MouseButtonID.MB_Left)) { HandleLeftMouseReleased(input); }
            if (mStateMgr.Input.WasMouseButtonReleased(MouseButtonID.MB_Middle)) { HandleMiddleMouseReleased(input); }
            if (mStateMgr.Input.WasMouseButtonReleased(MouseButtonID.MB_Right)) { HandleRightMouseReleased(input); }
            HandleKeyboard(input);
        }

        private void HandleMouseMove(MoisManager input) {
            Ray mouseRay = GetSelectionRay(input.MousePosX, input.MousePosY);

            Mogre.Pair<bool, float> intersection = mouseRay.Intersects(new Plane(Vector3.UNIT_Y, Vector3.ZERO));
            if (intersection.first && selboxShouldUpate()) {
                Vector3 intersectionPt = mouseRay.Origin + mouseRay.Direction * intersection.second;
                Point plotCoord = CityManager.GetPlotCoords(intersectionPt);
                Vector3 plotCenter = CityManager.GetPlotCenter(plotCoord);
                cursorPlane.SetPosition(plotCenter.x, plotCenter.y + 1f, plotCenter.z);

                if (mouseMode == MouseMode.PlacingBuilding && tempBuilding != null) {
                    tempBuilding.SetPosition(plotCoord.X, plotCoord.Y);
                }

                DebugPanel.SetDebugText(CityManager.GetPlotCoords(intersectionPt).ToString());
            }

            if (input.MouseMoveZ != 0.0f) {
                dist += input.MouseMoveZ * 0.002f;
                if (dist < -14.0f) { dist = -14.0f; }
                if (dist > 0.0f) { dist = 0.0f; }
            }
        }

        private void HandleLeftMousePressed(MoisManager input) {
            if (!StateManager.SupressGameControl) {
                if (!ContextMenu.HitTest(MousePosition(input))) {
                    ContextMenu.Visible = false;

                    if (canPlaceBuilding()) {
                        //Do a little reflection to be able to pass the type of the cursor building into the generic method
                        MethodInfo method = typeof(CityManager).GetMethod("NewBuilding");
                        MethodInfo newBuilding = method.MakeGenericMethod(this.tempBuilding.Data.GetType());
                        newBuilding.Invoke(null, new object[] { this.tempBuilding.PlotX, this.tempBuilding.PlotY });
                        gConsole.WriteLine("Placing building...");

                        if (!input.IsKeyDown(KeyCode.KC_LSHIFT)) {
                            //Then dispose the cursor building as the game will be making a new one very shortly
                            CancelBuildingPlacement();
                            mouseMode = MouseMode.Selection;
                        }
                    }

                    if (canSelect()) {
                        Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                        if (result.first) {
                            CityManager.SetSelectionOrigin(result.second);
                        }
                    }

                    if (canZone() || canUnzone()) {
                        Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                        if (result.first) {
                            CityManager.SetScratchZoneOrigin(result.second);
                        }
                    }
                }
            }
        }

        private void HandleLeftMouseHeld(MoisManager input) {
            if (!StateManager.SupressGameControl) {
                if (canSelect()) {
                    Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                    if (result.first) {
                        CityManager.UpdateSelectionBox(result.second);
                        if (CityManager.SelectionIsValid()) {
                            UpdateSelectionBox();
                        }
                    }
                }
                if (canZone() || canUnzone()) {
                    Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                    if (result.first) {
                        CityManager.UpdateScratchZoneBox(result.second);
                        if (CityManager.ScratchZoneIsValid()) {
                            UpdateScratchZoneBox();
                        }
                    }
                }
                if (canRoad()) {
                    Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                    if (result.first) {
                        Haswell.Controller.City.CreateRoad(result.second.X, result.second.Y);
                    }

                }
            }
        }

        private void HandleLeftMouseReleased(MoisManager input) {
            if (mouseMode == MouseMode.Selection && CityManager.SelectionIsValid()) {
                Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                if (result.first) {
                    CityManager.UpdateSelectionBox(result.second);
                }
                UpdateSelectionBox();
                CityManager.MakeSelection();
                CityManager.ClearSelection();
                selectionBox.SetVisible(false);
            }
            if ((canZone() || canUnzone()) && CityManager.ScratchZoneIsValid()) {
                Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                if (result.first) {
                    CityManager.UpdateScratchZoneBox(result.second);
                }
                UpdateScratchZoneBox();
                CityManager.MakeZone();
                CityManager.ClearScratchZone();
                scratchZone.SetVisible(false);

            }
        }

        private void HandleMiddleMousePressed(MoisManager input) {

        }

        private void HandleMiddleMouseHeld(MoisManager input) {
            if (!StateManager.SupressGameControl) {
                if (viewShouldUpdate()) {
                    //Mouse rotate control
                    angle += input.MouseMoveX * 0.01f;
                    //mStateMgr.Input += mStateMgr.Input.MouseMoveX; //Enable this if you ever figure out how to prevent the mouse from moving

                    //Mouse drag control //Re-enable if control scheme changes to allow for mouse dragging
                    /*Vector2 mouseMoveRotated = Utils3D.RotateVector2(new Vector2(mStateMgr.Input.MouseMoveX, mStateMgr.Input.MouseMoveY), angle);
                    focalPoint.Translate(new Vector3(mouseMoveRotated.y, 0, mouseMoveRotated.x));
                    mStateMgr.GuiSystem.GUIManager.Cursor.SetActiveMode(CursorMode.ResizeTop);*/

                    if (!ContextMenu.HitTest(MousePosition(input))) {
                        ContextMenu.Visible = false;
                    }
                }
            }
        }

        private void HandleMiddleMouseReleased(MoisManager input) {

        }

        private void HandleRightMousePressed(MoisManager input) {
            if (!StateManager.SupressGameControl) {
                if (ContextMenu.Visible == true && !ContextMenu.HitTest(MousePosition(input))) {
                    ContextMenu.Visible = false;
                } else {
                    ContextMenu.Location = new Point(input.MousePosX, input.MousePosY);

                    Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(input));
                    if (result.first) {
                        CityManager.UpdateSelectionBox(result.second);
                    }
                    if (canSelect()) {
                        UpdateSelectionBox();
                        CityManager.MakeSelection();

                        if (CityManager.GetSelectedBuildings().Count > 0) {
                            ContextMenu.AddButton("Properties...", (object sender, EventArgs e) => {
                                int i = 0;
                                foreach (Building b in CityManager.GetSelectedBuildings()) {
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
        }

        private void HandleRightMouseHeld(MoisManager input) {

        }

        private void HandleRightMouseReleased(MoisManager input) {

        }

        private void HandleKeyboard(MoisManager input) {

            if (!StateManager.SupressGameControl) {
                if (viewShouldUpdate()) {
                    //WASD Control
                    int speed = 10;
                    if (input.IsKeyDown(KeyCode.KC_A)) { CameraLeft(speed); }
                    if (input.IsKeyDown(KeyCode.KC_W)) { CameraForward(speed); }
                    if (input.IsKeyDown(KeyCode.KC_D)) { CameraRight(speed); }
                    if (input.IsKeyDown(KeyCode.KC_S)) { CameraBackward(speed); }

                    //Q and E to rotate
                    if (input.IsKeyDown(KeyCode.KC_Q)) { angle += 0.01f; }
                    if (input.IsKeyDown(KeyCode.KC_E)) { angle -= 0.01f; }
                }

                //Tab to cycle zones
                if (input.WasKeyPressed(KeyCode.KC_TAB)) {
                    if (mouseMode == MouseMode.DrawingZone) {
                        CycleDrawnZone();
                    }
                }

                //Toggle the console with `
                if (input.WasKeyPressed(KeyCode.KC_GRAVE)) { ToggleConsole(); }

                //Delete buildings with delete key
                if (input.WasKeyPressed(KeyCode.KC_DELETE)) { CityManager.DeleteSelectedBuildings(); }

                //Escape to cancel current action
                if (input.WasKeyPressed(KeyCode.KC_ESCAPE)) { CancelCurrentAction(); }

            }

            //Ctrl + W to quit the application
            if (input.WasKeyPressed(KeyCode.KC_W) && input.IsKeyDown(KeyCode.KC_LCONTROL)) { StartShutdown(); }

        }

        private void OnMouseModeChanged(MouseMode oldMouseMode, MouseMode newMouseMode)
        {
            if (oldMouseMode == MouseMode.PlacingBuilding)
            {
                CancelBuildingPlacement();
            }
        }

        private void UpdateSelectionBox() {
            Vector3 center = (CityManager.GetPlotCenter(CityManager.SelectionBox.Left, CityManager.SelectionBox.Top)
                + CityManager.GetPlotCenter(CityManager.SelectionBox.Right, CityManager.SelectionBox.Bottom))
                 * 0.5f;
            selectionBox.SetPosition(center.x, center.y, center.z);
            selectionBox.SetScale(CityManager.SelectionBox.Width * SCALEFACTOR + SCALEFACTOR, SCALEFACTOR / 2.0f, CityManager.SelectionBox.Height * SCALEFACTOR + SCALEFACTOR);
            selectionBox.SetVisible(true);
        }
        private void UpdateScratchZoneBox() {
            Vector3 center = (CityManager.GetPlotCenter(CityManager.scratchZoneBox.Left, CityManager.scratchZoneBox.Top)
                + CityManager.GetPlotCenter(CityManager.scratchZoneBox.Right, CityManager.scratchZoneBox.Bottom))
                 * 0.5f;
            scratchZone.SetPosition(center.x, center.y + 1.0f, center.z);
            scratchZone.SetScale(CityManager.scratchZoneBox.Width + 1, 1.0f, CityManager.scratchZoneBox.Height + 1);
            scratchZone.SetVisible(true);
        }
        public void UpdateScratchZoneBoxZone(Zones z) {
            scratchZoneEnt.GetSubEntity(0).SetMaterial(
                RenderablePlot.GetZoneColoredMaterial(
                scratchZoneEnt
                .GetSubEntity(0)
                .GetMaterial(),
                z));
        }
        public void UpdateGUI(float frametime) {
            if (CityManager.Initialized) {
                GuiMgr.SetCurrentCursorBuilding(this.tempBuilding);
                GuiMgr.Update(frametime);
            }
            GuiMgr.DebugPanel[4] = mouseMode.ToString();
        }

        private bool selboxShouldUpate() {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null;
        }

        private bool viewShouldUpdate() {
            return ContextMenu.Visible == false;
        }

        private bool notInteractingWithGUI() {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null;
        }

        private bool canRoad() {
            return notInteractingWithGUI() && mouseMode == MouseMode.DrawingRoad;
        }

        private bool canZone() {
            return notInteractingWithGUI() && mouseMode == MouseMode.DrawingZone;
        }

        private bool canUnzone()
        {
            return notInteractingWithGUI() && mouseMode == MouseMode.DeletingZone;
        }

        private bool canSelect() {
            return notInteractingWithGUI() && mouseMode == MouseMode.Selection;
        }

        private bool canPlaceBuilding() {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null && mouseMode == MouseMode.PlacingBuilding && tempBuilding != null;
        }
    }
}
