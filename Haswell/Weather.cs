using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M = System.Math;

namespace Haswell
{
    /// <summary>
    /// Weather Manager
    /// </summary>
    public class Weather
    {

        private double x; //Time of year

        /// <summary>
        /// Gets the fog.
        /// </summary>
        /// <value>The fog.</value>
        public float Fog { get; private set; }
        /// <summary>
        /// Gets the clouds.
        /// </summary>
        /// <value>The clouds.</value>
        public float Clouds { get; private set; }
        /// <summary>
        /// Gets the temporary.
        /// </summary>
        /// <value>The temporary.</value>
        public float Temp { get; private set; }
        /// <summary>
        /// Gets the wind x.
        /// </summary>
        /// <value>The wind x.</value>
        public float WindX { get; private set; }
        /// <summary>
        /// Gets the wind y.
        /// </summary>
        /// <value>The wind y.</value>
        public float WindY { get; private set; }
        /// <summary>
        /// Gets the wind magnitude.
        /// </summary>
        /// <value>The wind magnitude.</value>
        public float WindMagnitude { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Weather"/> class.
        /// </summary>
        public Weather()
        {

        }

        /// <summary>
        /// Something Something Alex write code here
        /// </summary>
        public void Update()
        {
            x = (Controller.CurrentTime.Year + Controller.CurrentTime.DayOfYear / 365.0 + Controller.CurrentTime.Hour / (24.0 * 365.0) + Controller.CurrentTime.Minute / (60.0 * 24.0 * 365.0)) * 12.0;
        }
        /// <summary>
        /// Gets the amount of cloud coverage
        /// </summary>
        /// <returns>A float between 0 and 1 where 0 is perfectly clear and 1 is completely overcast</returns>
        public float GetCloudinessAverage()
        {
            double raw = M.Sin(x * (M.PI / 6) + 1.0) / 4 + 0.5;
            return FloatClamp(0.0f, 1.0f, raw);
        }
        public float GetCloudiness()
        {
            double raw = GetCloudinessAverage() + 0.28 * M.Sin(x * (0.6666667 * M.PI) + 0.2);
            return FloatClamp(0.0f, 1.0f, raw);
        }
        /// <summary>
        /// Gets the expected fog density for this time of year
        /// </summary>
        /// <returns>A float between 0 and 1 where 0 is no fog and 1 is pea soup</returns>
        public float GetFogginessAverage()
        {
            double raw = M.Max(M.Max(M.Sin(x * (2 * M.PI / 12.0) + (5 * M.PI / 12)), 0.0) * -M.Sin(x * (2 * M.PI / 3.0)), 0.0);
            return FloatClamp(0.0f, 1.0f, raw);
        }
        /// <summary>
        /// Gets the fog density
        /// </summary>
        /// <returns>A float between 0 and 1 where 0 is no fog and 1 is pea soup</returns>
        public float GetFogginess()
        {
            double raw = GetFogginessAverage() * M.Max(M.Sin(x * 12.78283472 + 0.1357472), 0);
            return FloatClamp(0.0f, 1.0f, raw);
        }
        /// <summary>
        /// Gets the Unit Vector representing wind direction at the present time
        /// </summary>
        /// <returns>A pointF contanining the x and y components of the unit vector of the wind.</returns>
        public System.Drawing.PointF GetWindDirection()
        {
            return new System.Drawing.PointF(0.0f, 0.0f);
        }
        /// <summary>
        /// Gets the current speed of the wind
        /// </summary>
        /// <returns>A float representing wind speed in whatever units.</returns>
        public float GetWindSpeed()
        {
            return 0.0f;
        }
        /// <summary>
        /// Gets the expected temperature for this time of year.
        /// This number is on a scale of -1 to 1. -1 is the coldest any place will ever get, and 1 is the hottest.
        /// 0 is the freezing point of water.
        /// Most places will never go below a -0.3 or above a 0.7, and a place like Washington will hover around 0.4.
        /// </summary>
        /// <returns>A float between -1 and 1 where -1 is Russian Winter and 1 is Saharah Desert</returns>
        public float GetTemperatureAverage()
        {
            double raw = 0.5 * M.Sin(x * (2 * M.PI / 12) - (4 * M.PI / 6)) + 0.2;
            return FloatClamp(0.0f, 1.0f, raw);
        }
        /// <summary>
        /// Gets the current temperature.
        /// This number is on a scale of -1 to 1. -1 is the coldest any place will ever get, and 1 is the hottest.
        /// 0 is the freezing point of water.
        /// Most places will never go below a -0.3 or above a 0.7, and a place like Washington will hover around 0.4.
        /// </summary>
        /// <returns>A float between -1 and 1 where -1 is Russian Winter and 1 is Saharah Desert</returns>
        public float GetTemperature()
        {
            double raw = GetTemperatureAverage() + 0.2325624 * M.Sin(x) * M.Sin(7.346346 * x);
            return FloatClamp(0.0f, 1.0f, raw);
        }
        /// <summary>
        /// Gets the precipitation rate.
        /// </summary>
        /// <returns>System.Single.</returns>
        public float GetPrecipitationChance()
        {
            double raw = M.Pow(M.Sin(x * (2 * M.PI / 24.0) + (5 * M.PI / 12.0)), 2.0);
            return FloatClamp(0.0f, 1.0f, raw);
        }
        public float GetPrecipitationRate()
        {
            double raw = M.Max(GetPrecipitationChance() * M.Sin(10 * x) * M.Sin(4.34645654674 * x), 0.0);
            return FloatClamp(0.0f, 1.0f, raw);
        }

        private float FloatClamp(float min, float max, double val)
        {
            return (float)M.Max(M.Min(val, max), min);
        }
    }
}
