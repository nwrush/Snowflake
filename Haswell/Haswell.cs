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
    public class Haswell {
        private static GameState lastFrame;
        private static GameState currentFrame;
        private static City activeCity;

        public static void init(string name) {
            lastFrame = null;
            currentFrame=null;
            activeCity = new City(name);
        }

        public static GameState Update() {
            GameState returnState = lastFrame;
            try {
                returnState = currentFrame;
            } catch (Exception e) {
                LogError(e);
            }
            return new GameState(new long());
        }

        private static void LogError(Exception e) {
            String sb=e.Message+" "+e.StackTrace;
            Debug.WriteLine(e.Message);
            Process p = new Process();
            Process.Start("ErrorLog.exe", sb);
        }
    }
}
