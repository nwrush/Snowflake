using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing;

using Bradbury.Properties;
using SharpGLass;
using SharpGLass.GUI;
using Haswell;

using OpenTK;
using OpenTK.Input;

namespace Bradbury
{
    class CityWorld : World
    {
        //Initialization
        private string[] Months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public override void Load()
        {
            base.Load();
            Level l = new Level(1024, 1024);
            l.LoadTileset("Tiles");
            l.Tilesets[0].Texture = new Texture(Resources.Tiles_gfx);
            this.LoadLevel(l);

            this.BackgroundColor = System.Drawing.Color.DarkGreen;

            Camera.X = 512 * this.Level.TileWidth;
            Camera.Y = 512 * this.Level.TileHeight;

            InitHaswell();
            CreateGUI();
        }

        private void InitHaswell()
        {
            Haswell.Controller.init("New City");
        }

        private void CreateGUI()
        {
            Label timeLabel = new Label("January 1, 1970 - 0:00")
            {
                TextColor=Color.White,
                Font=new Font(FontFamily.GenericSansSerif, 20),
                Layer=90,
                Location = new Point(250, 320)
            };
            timeLabel.OnUpdate += (object sender, FrameEventArgs e) => {
                DateTime newTime = Haswell.Controller.Environment.CurrentTime;
                timeLabel.Text = newTime.DayOfWeek + ", " + newTime.Day + " " + Months[newTime.Month - 1] + ", " + newTime.Year + " - " + newTime.Hour + ":" + (newTime.Minute < 10 ? ("0" + newTime.Minute.ToString()) : newTime.Minute.ToString());
            };
            this.Add(timeLabel);
        }

        //Handy properties

        private Point HoveredTile
        {
            get { return new Point((int)(PointToWorldRotated(Input.MousePosition).X) / Level.TileWidth, ((int)PointToWorldRotated(Input.MousePosition).Y) / Level.TileHeight); }
        }

        //Update Loops

        public override void Update(FrameEventArgs e)
        {
            base.Update(e);

            UpdateInput(e);
            UpdateHaswell(e);
        }

        private void UpdateHaswell(FrameEventArgs e)
        {
            Haswell.Controller.Update((float)e.Time);
        }

        private void UpdateInput(FrameEventArgs e)
        {
            if (Input.GetMouseButton(MouseButton.Middle) == Input.KeyState.Down)
            {
                this.Camera.X -= Input.MouseDeltaPosition.X * this.Camera.Zoom;
                this.Camera.Y -= Input.MouseDeltaPosition.Y * this.Camera.Zoom;
            }

            this.Camera.Zoom *= (float)Math.Pow(1.0717734625362931642130063250233 /* 10th root of 2 */, -Input.MouseDeltaScroll); 
        }

        public override void Render(OpenTK.FrameEventArgs e, Engine n)
        {
 	         base.Render(e, n);

             DrawGrid(e, n);
        }

        private void DrawGrid(OpenTK.FrameEventArgs e, Engine n)
        {
            int minx = (int)((Camera.X - Camera.HalfWidth * Camera.Zoom) / Level.TileWidth) - 1;
            int maxx = (int)((Camera.X + Camera.HalfWidth * Camera.Zoom) / Level.TileWidth) + 1;
            int miny = (int)((Camera.Y - Camera.HalfHeight * Camera.Zoom) / Level.TileHeight) - 1;
            int maxy = (int)((Camera.Y + Camera.HalfHeight * Camera.Zoom) / Level.TileHeight) + 1;

            float o = Math.Min(64 / Camera.Zoom, 64);

            if ((int)o > 5) { 
                for (int x = minx; x < maxx; ++x)
                {
                    n.DrawLine(new PointF(x * Level.TileWidth, miny * Level.TileHeight), new PointF(x * Level.TileWidth, maxy * Level.TileHeight), Color.FromArgb((int)o, 0, 0, 0), 2);
                }
                for (int y = miny; y < maxy; ++y)
                {
                    n.DrawLine(new PointF(minx * Level.TileWidth, y * Level.TileHeight), new PointF(maxx * Level.TileWidth, y * Level.TileHeight), Color.FromArgb((int)o, 0, 0, 0), 2);
                }
            }
            //Draw rect around current hovered tile
            n.DrawRectangle(new PointF(HoveredTile.X * Level.TileWidth, HoveredTile.Y * Level.TileHeight),
                            new PointF((HoveredTile.X + 1) * Level.TileWidth, (HoveredTile.Y + 1) * Level.TileHeight),
                            Color.FromArgb(128, 255, 255, 192));
        }
    }
}
