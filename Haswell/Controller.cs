using System;
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
        private static Universe activeEnv;
        private static BackgroundThread backgroundThread;

        public static void init(string name) {
            activeEnv = new Universe(/*later we'll pass some regional parameters in here*/);
            activeCity = new City(name);
            System.IO.File.Delete("Snowflake.log");
            backgroundThread = new BackgroundThread();
        }

        public static void Update(float frametime) {
            activeCity.Update(frametime);
            activeEnv.Update(frametime);
        }

        public delegate void UpdateDelegate();
        private static void UpdateHour() {
            activeCity.UpdateHour(activeEnv.CurrentTime);
        }
        private static void UpdateDaily() {
            activeCity.UpdateDaily(activeEnv.CurrentTime);
        }
        private static void UpdateWeekly() {
            activeCity.UpdateWeekly(activeEnv.CurrentTime);
        }
        private static void UpdateMonthly() {
            activeCity.UpdateMonthly(activeEnv.CurrentTime);
        }
        private static void UpdateBiannually() {
            activeCity.UpdateBiannually(activeEnv.CurrentTime);
        }
        private static void UpdateYearly() {
            activeCity.UpdateYearly(activeEnv.CurrentTime);
        }

        [Obsolete("Do Not Call this function")]
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
        public static Universe Environment {
            get {
                return activeEnv;
            }
        }
        public static float ChanceThatThisProgramBecomesSkynet {
            get {
                return 0.0000001f;
            }
        }
        public static BackgroundThread BackgroundThread {
            get {
                return backgroundThread;
            }
        }
    }
}
