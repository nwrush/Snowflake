using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Haswell {
    private class BackgroundThread {

        private Thread backgroundThread;
        private Queue<EventHandler<TimeEventArgs>> eventsToHandle;

        public BackgroundThread() {
            backgroundThread = new Thread(BackgroundProcess);
            backgroundThread.IsBackground = true;
        }

        private void BackgroundProcess() {
            EventHandler<TimeEventArgs> tmp = eventsToHandle.Dequeue();
            tmp.Invoke(this, new TimeEventArgs(Haswell.Controller.Environment.CurrentTime));
        }
    }
}
