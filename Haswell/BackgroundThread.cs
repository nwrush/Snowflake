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
            eventsToHandle = new Queue<EventHandler<TimeEventArgs>>();
            backgroundThread = new Thread(BackgroundProcess);
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }

        private void BackgroundProcess() {
            while (true) {
                if (this.eventsToHandle.Count == 0)
                    continue;

                EventHandler<TimeEventArgs> tmp = eventsToHandle.Dequeue();
                tmp.Invoke(this, new TimeEventArgs(Haswell.Controller.CurrentTime));
            }
        }

        public Queue<EventHandler<TimeEventArgs>> Events {
            get {
                return this.eventsToHandle;
            }
        }
    }
}
