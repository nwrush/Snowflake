using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

            this.Add(new Entity(new System.Drawing.PointF(-200, -200), new System.Drawing.Size(128, 128), new Texture(Resources.Tiles_gfx)));

            this.BackgroundColor = System.Drawing.Color.LawnGreen;
        }

        public override void Update(OpenTK.FrameEventArgs e)
        {
            base.Update(e);
            if (Input.GetMouseButton(OpenTK.Input.MouseButton.Middle) == Input.KeyState.Down)
            {
                this.Camera.X -= Input.MouseDeltaPosition.X *this.Camera.Zoom;
                this.Camera.Y -= Input.MouseDeltaPosition.Y *this.Camera.Zoom;
            }
            this.Camera.Zoom *= (float)Math.Pow(1.1, -Input.MouseDeltaScroll);
        }
    }
}
