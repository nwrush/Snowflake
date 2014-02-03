using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class Universe {

        public float Time { get; private set; }
        public float Timescale { get; private set; }
        public const float DayLength = 2400.0f;
        public const float HourLength = 100.0f;
        public const float MinuteLength = 1.6666667f;

        public Universe() {
            Timescale = 1.0f;
        }

        public void Update(float frametime) {
            //Let's see...
            //So this is called once every time Haswell is updated
            //So we can already assume a somewhat fixed tickrate.
            //Frametime, then is kind of redundant?
            //So we just increment Time by Timescale.
            Time += Timescale;

            //Further on in here, there will be some variables dictating weather conditions...timers and such.
            //Cloudiness, Fogginess, and Wind Vector can be direct functions of some internal weather parameter.
            //That's Alex's job (supposedly).
        }

        /// <summary>
        /// Sets the number by which the Time gets incremented each update tick.
        /// </summary>
        /// <param name="timescale">The scale of time.</param>
        public void SetTimescale(float timescale) {
            this.Timescale = timescale;
        }

        public float GetCloudiness() {
            throw new NotImplementedException();
        }

        public float GetFogginess() {
            throw new NotImplementedException();
        }

        public Tuple<float, float> GetWindDirection() {
            throw new NotImplementedException();
        }

        public float GetWindSpeed() {
            throw new NotImplementedException();
        }
    }
}
