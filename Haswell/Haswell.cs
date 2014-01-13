using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Haswell {
    public class Haswell {
        private GameState lastFrame;
        private GameState currentFrame;

        public static GameState Update() {
            try {
                throw new ExecutionEngineException("This");
            } catch (Exception e) {
                LogError(e);
            }
            return new GameState();
        }

        private static void LogError(Exception e) {
            String sb=e.Message+" "+e.StackTrace;
            Debug.WriteLine(e.Message);
            Process p = new Process();
            Process.Start("ErrorLog.exe", sb);
        }
    }
}
