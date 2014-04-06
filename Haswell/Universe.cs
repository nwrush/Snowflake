﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Haswell {
    public class Universe {

        public float Time { get; private set; }
        public float Timescale { get; private set; }

        public float Fog { get; private set; }
        public float Clouds { get; private set; }
        public float Temp { get; private set; }
        public float WindX { get; private set; }
        public float WindY { get; private set; }
        public float WindMagnitude { get; private set; }

        public const float DayLength = 2400.0f;
        public const float HourLength = 100.0f;
        public const float MinuteLength = 1.6666667f;

        public DateTime CurrentTime { get; private set; }

        public event EventHandler<TimeEventArgs> Hourly;
        public event EventHandler<TimeEventArgs> Daily;
        public event EventHandler<TimeEventArgs> Weekly;
        public event EventHandler<TimeEventArgs> Monthly;
        public event EventHandler<TimeEventArgs> Quarterly;
        public event EventHandler<TimeEventArgs> Biannually;
        public event EventHandler<TimeEventArgs> Yearly;

        public event EventHandler<BackgroundEventArgs> BackgroundEvents;
        private Thread BackgroundUpdate;

        private Thread HourlyUpdate;
        private void InvokeHourlyUpdate() { Hourly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        private Thread DailyUpdate;
        private void InvokeDailyUpdate() { Daily.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        private Thread WeeklyUpdate;
        private void InvokeWeeklyUpdate() { Weekly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        private Thread MonthlyUpdate;
        private void InvokeMonthlyUpdate() { Monthly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        private Thread QuarterlyUpdate;
        private void InvokeQuarterlyUpdate() { Quarterly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        private Thread BiannuallyUpdate;
        private void InvokeBinannuallyUpdate() { Biannually.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        private Thread YearlyUpdate;
        private void InvokeYearlyUpdate() { Yearly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        public Universe() {
            Timescale = 20.0f;

        }
        private void startBackgroundThread() {
            
            BackgroundUpdate.IsBackground = false;
            BackgroundUpdate.Start();
        }

        public void Update(float frametime) {
            //Let's see...
            //So this is called once every time Haswell is updated
            //So we can already assume a somewhat fixed tickrate.
            //Frametime, then is kind of redundant?
            //So we just increment Time by Timescale.
            DateTime _prevTime = this.CurrentTime;
            Time += Timescale;
            this.CurrentTime = new DateTime(1970, 1, 1).AddMinutes(this.Time / Universe.MinuteLength);
            if (_prevTime.Hour != this.CurrentTime.Hour) {
                //Don't assume this update is being called once or more per hour. At fast timescales with low tickrates, this will not be the case!
                if (Hourly != null) { this.HourlyUpdate = new Thread(this.InvokeHourlyUpdate); this.HourlyUpdate.Start(); }
            }
            if (_prevTime.Day != this.CurrentTime.Day) {
                if (Daily != null) {
                    if (this.DailyUpdate != null) { this.DailyUpdate.Abort(); }
                    this.DailyUpdate = new Thread(this.InvokeDailyUpdate);
                    this.DailyUpdate.IsBackground = true;
                    this.DailyUpdate.Start();
                }
            }
            if (_prevTime.DayOfWeek != this.CurrentTime.DayOfWeek && this.CurrentTime.DayOfWeek == DayOfWeek.Monday) {
                if (Weekly != null) {
                    if (this.WeeklyUpdate != null) { this.WeeklyUpdate.Abort(); }
                    this.WeeklyUpdate = new Thread(this.InvokeWeeklyUpdate);
                    this.WeeklyUpdate.IsBackground = true;
                    this.WeeklyUpdate.Start();
                }
            }
            if (_prevTime.Month != this.CurrentTime.Month) {
                if (Monthly != null) {
                    if (this.MonthlyUpdate != null) { this.MonthlyUpdate.Abort(); }
                    this.MonthlyUpdate = new Thread(this.InvokeMonthlyUpdate);
                    this.MonthlyUpdate.IsBackground = true;
                    this.MonthlyUpdate.Start();
                }
            }
            if (CheckQuarterly(_prevTime)) {
                if (Quarterly != null) {
                    if (this.QuarterlyUpdate != null) { this.QuarterlyUpdate.Abort(); }
                    this.QuarterlyUpdate = new Thread(this.InvokeQuarterlyUpdate);
                    this.QuarterlyUpdate.IsBackground = true;
                    this.QuarterlyUpdate.Start();
                }
            }
            if ((this.CurrentTime.Month == 1 || this.CurrentTime.Month == 6) && this.CurrentTime.Month != _prevTime.Month) {
                if (Biannually != null) {
                    if (this.BiannuallyUpdate != null) { this.BiannuallyUpdate.Abort(); }
                    this.BiannuallyUpdate = new Thread(this.InvokeBinannuallyUpdate);
                    this.BiannuallyUpdate.IsBackground = true;
                    this.BiannuallyUpdate.Start();
                }
            }
            if (_prevTime.Year != this.CurrentTime.Year) {
                if (Yearly != null) {
                    if (this.YearlyUpdate != null) { this.YearlyUpdate.Abort(); }
                    this.YearlyUpdate = new Thread(this.InvokeYearlyUpdate);
                    this.YearlyUpdate.IsBackground = true;
                    this.YearlyUpdate.Start();
                }
            }

            //Further on in here, there will be some variables dictating weather conditions...timers and such.
            //Cloudiness, Fogginess, and Wind Vector can be direct functions of some internal weather parameter.
            //That's Alex's job (supposedly).
        }
        private bool CheckQuarterly(DateTime _prevTime) {
            if (this.CurrentTime.Month == _prevTime.Month)
                return false;

            if (this.CurrentTime.Month == 1)
                return true;
            else if (this.CurrentTime.Month == 4)
                return true;
            else if (this.CurrentTime.Month == 7)
                return true;
            else if (this.CurrentTime.Month == 10)
                return true;

            return false;
        }

        /// <summary>6
        /// Sets the number by which the Time gets incremented each update tick.
        /// </summary>
        /// <param name="timescale">The scale of time.</param>
        public void SetTimescale(float timescale) {
            this.Timescale = timescale;
        }
        /// <summary>
        /// Gets the amount of cloud coverage
        /// </summary>
        /// <returns>A float between 0 and 1 where 0 is perfectly clear and 1 is completely overcast</returns>
        public float GetCloudiness() {
            double x = (CurrentTime.Year + CurrentTime.Day / 365.0) * 12.0;
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
