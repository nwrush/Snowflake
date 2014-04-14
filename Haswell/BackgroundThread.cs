using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Haswell {
    public class BackgroundThread {

        private Thread backgroundThread;
        private Queue<EventHandler<TimeEventArgs>> eventsToHandle;

        public BackgroundThread() {
            backgroundThread = new Thread(BackgroundProcess);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private void BackgroundProcess() {
            while (true) {
                EventHandler<TimeEventArgs> tmp = eventsToHandle.Dequeue();
                tmp.Invoke(this, new TimeEventArgs(Haswell.Controller.Environment.CurrentTime));
            }
        }

        public Queue<EventHandler<TimeEventArgs>> Events {
            get {
                return this.eventsToHandle;
            }
        }
    }
}
