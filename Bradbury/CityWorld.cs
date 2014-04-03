using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpGLass;
using Haswell;

namespace Bradbury
{
    class CityWorld : World
    {
        public override void Load()
        {
            base.Load();
            Level l = new Level(UInt32.MaxValue, UInt32.MinValue);
            this.LoadLevel(l);
        }
    }
}
