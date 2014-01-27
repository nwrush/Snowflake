﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Haswell {
    /// <summary>
    /// Controls the Game logic and holds "global" variables and functions
    /// Game shouold only make calls to this class
    /// </summary>
    public class Controller {
        private static City activeCity;
         
        public static void init(string name) {
            activeCity = new City(name);
            System.IO.File.Delete("Snowflake.log");
        }

        public static void Update(float frametime) {
            activeCity.Update(frametime);
        }

        public static void LogError(Exception e) {
            String msg = "\"" + e.Message + "\"";
            msg += " " + "\"" + e.StackTrace + "\"";
            Process p = new Process();
            Process.Start("Log.exe", msg);
        }

        public static City City {
            get {
                return activeCity;
            }
        }
        public static float ChanceThatThisProgramBecomesSkynet {
            get {
                return 0.0000001f;
            }
        }
    }
}
