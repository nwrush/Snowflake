﻿using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Miyagi.Common.Data;

using Snowflake.Modules;
using Snowflake.GuiComponents;
using Snowflake.States;

using Haswell;
using Haswell.Buildings;
using Haswell.Exceptions;

using Vector3 = Mogre.Vector3;
using Rectangle = Miyagi.Common.Data.Rectangle;

namespace Snowflake {
    public static class CityManager {

        private static bool initialized = false;

        private static SceneNode cityNode;
        private static Entity ground;
        private static SceneNode world;
        private static Point origin;

        private static List<Renderable> cityObjects;

        public static Point selectionStart { get; private set; }
        public static Point selectionEnd { get; private set; }

        public static bool Initialized { get { return initialized; } }

        public static Rectangle SelectionBox { 
            get { 
                return new Rectangle(selectionStart, 
                        new Size((selectionEnd.X > selectionStart.X ? selectionEnd.X - selectionStart.X : 1), 
                                 (selectionEnd.Y > selectionStart.Y ? selectionEnd.Y - selectionStart.Y : 1))); 
            } 
        }

        public static StateManager StateMgr { get { return GameMgr.StateMgr; } }
        public static SceneManager SceneMgr { get { return GameMgr.StateMgr.Engine.SceneMgr; } }
        public static OgreManager Engine { get { return GameMgr.StateMgr.Engine; } }
        public static GameLoopState GameMgr { get; private set; }

        static CityManager() {
            cityObjects = new List<Renderable>();
        }

        /// <summary>
        /// Sets up city terrain and creates road planes.
        /// </summary>
        /// <param name="sm">Scenemanager to create scene in</param>
        public static void CreateScene(SceneManager sm) {

            CreateTerrain(sm);
            CreateCity(sm);
        }

        /// <summary>
        /// Sets the current active game state.
        /// </summary>
        /// <param name="gameState">GameLoopState to reference for game functions</param>
        public static void SetGameMgr(GameLoopState gameState) {
            GameMgr = gameState;
        }

        /// <summary>
        /// Set up terrain
        /// </summary>
        private static void CreateTerrain(SceneManager sm) {
           
            Plane plane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 10000, 10000, 100, 100, true, 1, 5, 5, Vector3.UNIT_Z);

            ground = sm.CreateEntity("GroundEntity", "ground");
            ground.SetMaterialName("Grass");
            world = sm.RootSceneNode.CreateChildSceneNode();
            world.AttachObject(ground);
            //world.Translate(new Vector3(0, -1, 0));
        }

        private static void CreateCity(SceneManager sm) {
            cityNode = sm.RootSceneNode.CreateChildSceneNode("CityNode");

            //CreateRoads(sm);
            CreateObjects(sm);
        }
        
        private static void CreateRoads(SceneManager sm) {

            //Todo: initialize roads from Haswell data

            throw new NotImplementedException();
        }

        private static void CreateObjects(SceneManager sm) {

            //Todo: initialize objects from Haswell data

            foreach (Renderable r in cityObjects) {
                r.Create(sm, cityNode);
            }
        }

        /// <summary>
        /// Initializes the City, setting its origin point and creating the relevant Haswell objects.
        /// </summary>
        /// <param name="originx">X position of city origin</param>
        /// <param name="originy">Y Position of city origin</param>
        public static void Init(int originx, int originy) {
            if (!initialized) {
                GameConsole.ActiveInstance.WriteLine("Founding new City at " + originx.ToString() + ", " + originy.ToString());

                origin = new Point(originx, originy);
                Haswell.Controller.init("shrug");
                Haswell.Controller.City.BuildingCreated += CreateBuilding;
                initialized = true;
            }
            else {
                GameConsole.ActiveInstance.WriteError("Attempting to found city in an already initialized area!");
            }
        }
        /// <summary>
        /// Initializes the City, setting its origin point and creating the relevant Haswell objects.
        /// </summary>
        /// <param name="p">Origin point of the city</param>
        public static void Init(Point p) { Init(p.X, p.Y); }

        /// <summary>
        /// Add a new building to the city at the specified coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void NewBuilding(int x, int y) {
            if (initialized) {
                try { Haswell.Controller.City.CreateBuilding<Commercial>(x, y); }
                catch (BuildingCreationFailedException e) {
                    GameConsole.ActiveInstance.WriteLine(e.Message);
                }
            }
            else {
                GameConsole.ActiveInstance.WriteError("Unable to create building, no city initialized!");
            }
        }
        /// <summary>
        /// Add a new building to the city at the specified coordinates
        /// </summary>
        /// <param name="p">2d location to place the building at.</param>
        public static void NewBuilding(Point p) { NewBuilding(p.X, p.Y); }

        private static void CreateBuilding(object sender, BuildingEventArgs e) {
            GameConsole.ActiveInstance.WriteLine("Added a building at " + e.Building.Parent.X + ", " + e.Building.Parent.Y);
            RenderableBuilding rb = new RenderableBuilding(e.Building);
            rb.Create( GameMgr.StateMgr.Engine.SceneMgr, cityNode);
            cityObjects.Add(rb);
        }
            
        /// <summary>
        /// Update the city
        /// </summary>
        /// <param name="frametime">Milliseconds since last frame</param>
        public static void Update(float frametime) {

            if (initialized && true) {
                try {
                    Haswell.Controller.Update(frametime);
                }
                catch (NotImplementedException e) {
                    DebugPanel.ActiveInstance.SetDebugText(e.Message);
                }

                foreach (Renderable r in cityObjects) {
                    //Check if r needs updating, and if so:
                    try {
                        r.Update();
                    } catch (NotImplementedException e) {
                        DebugPanel.ActiveInstance.SetDebugText(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the point where the selection rectangle begins
        /// </summary>
        /// <param name="p">Selection start point</param>
        public static void SetSelectionStart(Point p) {
            selectionStart = p;
        }
        /// <summary>
        /// Sets the point where the selection rectangle ends
        /// </summary>
        /// <param name="p">Selection end point</param>
        public static void SetSelectionEnd(Point p) {
            if (p.X < selectionStart.X) {
                if (p.Y < selectionStart.Y) {
                    selectionEnd = selectionStart;
                    selectionStart = p;
                }
                else {
                    selectionEnd = new Point(selectionStart.X, p.Y);
                    selectionStart = new Point(p.X, selectionStart.Y);
                }
            }
            else if (p.Y < selectionStart.Y) {
                if (p.X < selectionStart.X) {
                    selectionEnd = selectionStart;
                    selectionStart = p;
                }
                else {
                    selectionEnd = new Point(p.X, selectionStart.Y);
                    selectionStart = new Point(selectionStart.X, p.Y);
                }
            }
            else {
                selectionEnd = p;
            }
        }
        /// <summary>
        /// Takes the active selection and performs the selection action on all the things contained within.
        /// </summary>
        public static bool MakeSelection() {
            //Todo: make selection
            if (SelectionBox.Left != Int32.MaxValue - 1 && SelectionBox.Top != Int32.MaxValue - 1) {
                GameConsole.ActiveInstance.WriteLine("Selecting region " + SelectionBox.ToString());
                return true;
                //ClearSelection();
            }
            return false;
        }

        /// <summary>
        /// Clears the selection
        /// </summary>
        public static void ClearSelection() {
            selectionStart = new Point(Int32.MaxValue - 1, Int32.MaxValue - 1);
            selectionEnd = new Point(Int32.MaxValue, Int32.MaxValue);
        }

        /// <summary>
        /// Gives the 2D plot coordinates of a given 3D point
        /// </summary>
        /// <param name="src">Source point</param>
        /// <returns>Destination point</returns>
        public static Point GetPlotCoords(Vector3 src) {
            if (!(origin.X == 0 && origin.Y == 0)) {
                return new Point((int)System.Math.Floor(src.x / Renderable.PlotWidth - origin.X), (int)System.Math.Floor(src.z / Renderable.PlotHeight - origin.Y));
            }
            return new Point((int)System.Math.Floor(src.x / Renderable.PlotWidth), (int)System.Math.Floor(src.z / Renderable.PlotHeight));
        }

        /// <summary>
        /// Gets the 3D point representing the center of the given 2D plot.
        /// </summary>
        /// <param name="x">x position of the plot</param>
        /// <param name="y">y position of the plot</param>
        /// <returns>3D point in the center of the plot</returns>
        public static Vector3 GetPlotCenter(int x, int y) { return GetPlotCenter(new Point(x, y)); }

        /// <summary>
        /// Gets the 3D point representing the center of the given 2D plot.
        /// </summary>
        /// <param name="plotCoord">2D Point representing plot coordinate</param>
        /// <returns>3D point in the center of the plot</returns>
        public static Vector3 GetPlotCenter(Point plotCoord) {
            if (!(origin.X == 0 && origin.Y == 0)) {
                //Todo: account for terrain height
                return new Vector3((plotCoord.X + origin.X) * Renderable.PlotWidth + (Renderable.PlotWidth * 0.5f), 
                                    0.0f,
                                    (plotCoord.Y + origin.Y) * Renderable.PlotHeight + (Renderable.PlotHeight * 0.5f));
            }
            else {
                return new Vector3(plotCoord.X * Renderable.PlotWidth + (Renderable.PlotWidth * 0.5f),
                    0.0f,
                    plotCoord.Y * Renderable.PlotHeight + (Renderable.PlotHeight * 0.5f));
            }
        }

        /// <summary>
        /// Gets the origin point of the current city.
        /// </summary>
        /// <returns>2D origin point</returns>
        public static Point GetOrigin() {
            return origin;
        }
    }
}
