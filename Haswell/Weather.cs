using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Weather Manager
    /// </summary>
    public class Weather {

        public float Fog { get; private set; }
        public float Clouds { get; private set; }
        public float Temp { get; private set; }
        public float WindX { get; private set; }
        public float WindY { get; private set; }
        public float WindMagnitude { get; private set; }

        public Weather() {

        }

        /// <summary>
        /// Something Something Alex write code here
        /// </summary>
        public void Update() {

        }
        /// <summary>
        /// Gets the amount of cloud coverage
        /// </summary>
        /// <returns>A float between 0 and 1 where 0 is perfectly clear and 1 is completely overcast</returns>
        public float GetCloudiness() {
            double x = (Controller.Environment.CurrentTime.Year + Controller.Environment.CurrentTime.Day / 365.0) * 12.0;
            double raw = (((Math.Sin(x * (Math.PI / 6) + 1.0) + 0.3 * Math.Sin(x * (0.6666667 * Math.PI) + 0.2)) / (1.3 * 4)) + ((0.5 * Math.Sin(x * 23.4142135 + 2.2324) + 0.5 * Math.Sin(37.7889 + 6.42323)) * 0.5) + 0.5);
            return (float)Math.Max(Math.Min(raw, 1.0), 0.0);
        }
        /// <summary>
        /// Gets the fog density
        /// </summary>
        /// <returns>A float between 0 and 1 where 0 is no fog and 1 is pea soup</returns>
        public float GetFogginess() {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the Unit Vector representing wind direction at the present time
        /// </summary>
        /// <returns>A pointF contanining the x and y components of the unit vector of the wind.</returns>
        public System.Drawing.PointF GetWindDirection() {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the current speed of the wind
        /// </summary>
        /// <returns>A float representing wind speed in whatever units.</returns>
        public float GetWindSpeed() {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the current temperature.
        /// This number is on a scale of -1 to 1. -1 is the coldest any place will ever get, and 1 is the hottest.
        /// 0 is the freezing point of water.
        /// Most places will never go below a -0.3 or above a 0.7, and a place like Washington will hover around 0.4.
        /// </summary>
        /// <returns>A float between -1 and 1 where -1 is Russian Winter and 1 is Saharah Desert</returns>
        public float GetTemperature() {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets the current level of precipitation (usually rain unless temp < 0)
        /// </summary>
        /// <returns></returns>
        public float GetPrecipitationRate() {
            throw new NotImplementedException();
        }
    }
}
