using System;
using System.Collections.Generic;

using Bradbury.Properties;
using SharpGLass;
using OpenTK.Graphics;

namespace Bradbury
{
    class Program
    {
        static Engine Engine;

        [STAThread]
        static void Main(string[] args)
        {
            Engine = new Engine(new CityWorld(), 1024, 768, GraphicsMode.Default, "Project Sustain");
            GV.ResourceManager = Resources.ResourceManager;
            Engine.Run();
        }
    }
}
