using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Haswell {
    public class BackgroundThread {

        private Thread backgroundThread;
        private Queue<Controller.UpdateDelegate> eventsToHandle;

        public BackgroundThread() {
            eventsToHandle = new Queue<Controller.UpdateDelegate>();
            backgroundThread = new Thread(BackgroundProcess);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private void BackgroundProcess() {
            while (true) {
                if (this.eventsToHandle.Count == 0)
                    continue;

                Controller.UpdateDelegate tmp = eventsToHandle.Dequeue();
                tmp(Controller.CurrentTime);
            }
        }

        public Queue<Controller.UpdateDelegate> Events {
            get {
                return this.eventsToHandle;
            }
        }
    }
}
