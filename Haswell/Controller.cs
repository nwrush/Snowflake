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
        /// <summary>
        /// The active city
        /// </summary>
        private static City activeCity;
        /// <summary>
        /// The active env
        /// </summary>
        private static Universe activeEnv;

        /// <summary>
        /// Initializes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public static void init(string name) {
            activeEnv = new Universe(/*later we'll pass some regional parameters in here*/);
            activeCity = new City(name);
            System.IO.File.Delete("Snowflake.log");
        }

        /// <summary>
        /// Updates the specified frametime.
        /// </summary>
        /// <param name="frametime">The frametime.</param>
        public static void Update(float frametime) {
            activeCity.Update(frametime);
            activeEnv.Update(frametime);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="e">The e.</param>
        public static void LogError(Exception e) {
            String msg = "\"" + e.Message + "\"";
            msg += " " + "\"" + e.StackTrace + "\"";
            Process p = new Process();
            Process.Start("Log.exe", msg);
        }

        /// <summary>
        /// Gets the city.
        /// </summary>
        /// <value>The city.</value>
        public static City City {
            get {
                return activeCity;
            }
        }
        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public static Universe Environment {
            get {
                return activeEnv;
            }
        }
        /// <summary>
        /// Gets the chance that this program becomes skynet.
        /// </summary>
        /// <value>The chance that this program becomes skynet.</value>
        public static float ChanceThatThisProgramBecomesSkynet {
            get {
                return 0.0000001f;
            }
        }
    }
}
