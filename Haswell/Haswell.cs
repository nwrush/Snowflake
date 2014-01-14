using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Haswell {
    public class Haswell {
        private static GameState lastFrame=null;
        private static GameState currentFrame=null;

        public static GameState Update() {
            GameState returnState = lastFrame;
            try {
                returnState = currentFrame;
            } catch (Exception e) {
                LogError(e);
            }
            return returnState;
        }

        private static void LogError(Exception e) {
            String sb=e.Message+" "+e.StackTrace;
            Debug.WriteLine(e.Message);
            Process p = new Process();
            Process.Start("ErrorLog.exe", sb);
        }
    }
}
