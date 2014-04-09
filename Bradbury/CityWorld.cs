using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Drawing;

using Bradbury.Properties;
using SharpGLass;
using Haswell;

namespace Bradbury
{
    class CityWorld : World
    {
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
        }

        public override void Update(OpenTK.FrameEventArgs e)
        {
            base.Update(e);

            UpdateInput(e);
        }

        private void UpdateInput(OpenTK.FrameEventArgs e)
        {
            if (Input.GetMouseButton(OpenTK.Input.MouseButton.Middle) == Input.KeyState.Down)
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
            int minx = (int)((Camera.X - Camera.HalfWidth * Camera.Zoom) / Level.TileWidth);
            int maxx = (int)((Camera.X + Camera.HalfWidth * Camera.Zoom) / Level.TileWidth) + 1;
            int miny = (int)((Camera.Y - Camera.HalfHeight * Camera.Zoom) / Level.TileHeight);
            int maxy = (int)((Camera.Y + Camera.HalfHeight * Camera.Zoom) / Level.TileHeight) + 1;

            for (int x = minx; x < maxx; ++x)
            {
                n.DrawLine(new PointF(x * Level.TileWidth, miny * Level.TileHeight), new PointF(x * Level.TileWidth, maxy * Level.TileHeight), Color.FromArgb(64, 0, 0, 0), 2);
            }
            for (int y = miny; y < maxy; ++y)
            {
                n.DrawLine(new PointF(minx * Level.TileWidth, y * Level.TileHeight), new PointF(maxx * Level.TileWidth, y * Level.TileHeight), Color.FromArgb(64, 0, 0, 0), 2);
            }
        }
    }
}
