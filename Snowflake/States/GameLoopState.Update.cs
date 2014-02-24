using System;
using System.Reflection;

using Mogre;

using Snowflake.Modules;
using Snowflake.GuiComponents;

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

                    if (mouseMode == MouseMode.PlacingBuilding && cursorBuilding != null) {
                        cursorBuilding.SetPosition(plotCoord.X, plotCoord.Y);
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
                            MethodInfo newBuilding = method.MakeGenericMethod(this.cursorBuilding.GetData().GetType());
                            newBuilding.Invoke(null, new object[] { this.cursorBuilding.PlotX, this.cursorBuilding.PlotY });
                            gConsole.WriteLine("Placing building...");

                            if (!mStateMgr.Input.IsKeyDown(KeyCode.KC_LSHIFT))
                            {
                                //Then dispose the cursor building as the game will be making a new one very shortly
                                this.cursorBuilding.Dispose();
                                this.cursorBuilding = null;
                                this.mouseMode = MouseMode.Selection;
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
                }

                if (mStateMgr.Input.WasMouseButtonReleased(MouseButtonID.MB_Left))
                {
                    if (canSelect())
                    {
                        Mogre.Pair<bool, Point> result = getPlotCoordsFromScreenPoint(MousePosition(mStateMgr.Input));
                        if (result.first)
                        {
                            CityManager.UpdateSelectionBox(result.second);
                        }
                        UpdateSelectionBox();
                        CityManager.MakeSelection();
                        selectionBox.SetVisible(false);
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

                //Toggle the console with `
                if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_GRAVE))
                {
                    gConsole.Visible = !gConsole.Visible;
                }

                if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_DELETE))
                {
                    CityManager.DeleteSelectedBuildings();
                }
            }

            // check if the escape key was pressed
            if (mStateMgr.Input.WasKeyPressed(KeyCode.KC_EQUALS))
            {
                // quit the application
                mStateMgr.RequestShutdown();
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
        public void UpdateGUI(float frametime)
        {
            if (CityManager.Initialized)
            {
                GuiMgr.SetCurrentCursorBuilding(this.cursorBuilding);
                GuiMgr.Update(frametime);
            }
        }

        private bool selboxShouldUpate()
        {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null;
        }

        private bool viewShouldUpdate()
        {
            return ContextMenu.Visible == false;
        }

        private bool canSelect()
        {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null && mouseMode == MouseMode.Selection;
        }

        private bool canPlaceBuilding() {
            return ContextMenu.Visible == false && StateMgr.GuiSystem.GUIManager.GetTopControlAt(MousePosition(StateMgr.Input)) == null && mouseMode == MouseMode.PlacingBuilding && cursorBuilding != null;
        }
    }
}
