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
            l.LoadTileset("Tileset");
            this.LoadLevel(l);
        }
    }
}
