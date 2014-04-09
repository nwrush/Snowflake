using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Haswell {
    /// <summary>
    /// Class Universe.
    /// </summary>
    public class Universe {

        /// <summary>
        /// Gets the time.
        /// </summary>
        /// <value>The time.</value>
        public float Time { get; private set; }
        /// <summary>
        /// Gets the timescale.
        /// </summary>
        /// <value>The timescale.</value>
        public float Timescale { get; private set; }


        /// <summary>
        /// The day length
        /// </summary>
        public const float DayLength = 2400.0f;
        /// <summary>
        /// The hour length
        /// </summary>
        public const float HourLength = 100.0f;
        /// <summary>
        /// The minute length
        /// </summary>
        public const float MinuteLength = 1.6666667f;

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <value>The current time.</value>
        public DateTime CurrentTime { get; private set; }

        /// <summary>
        /// Occurs when [hourly].
        /// </summary>
        public event EventHandler<TimeEventArgs> Hourly;
        /// <summary>
        /// Occurs when [daily].
        /// </summary>
        public event EventHandler<TimeEventArgs> Daily;
        /// <summary>
        /// Occurs when [weekly].
        /// </summary>
        public event EventHandler<TimeEventArgs> Weekly;
        /// <summary>
        /// Occurs when [monthly].
        /// </summary>
        public event EventHandler<TimeEventArgs> Monthly;
        /// <summary>
        /// Occurs when [quarterly].
        /// </summary>
        public event EventHandler<TimeEventArgs> Quarterly;
        /// <summary>
        /// Occurs when [biannually].
        /// </summary>
        public event EventHandler<TimeEventArgs> Biannually;
        /// <summary>
        /// Occurs when [yearly].
        /// </summary>
        public event EventHandler<TimeEventArgs> Yearly;

        /// <summary>
        /// Occurs when [background events].
        /// </summary>
        public event EventHandler<BackgroundEventArgs> BackgroundEvents;
        /// <summary>
        /// The background update
        /// </summary>
        private Thread BackgroundUpdate;

        /// <summary>
        /// The hourly update
        /// </summary>
        private Thread HourlyUpdate;
        /// <summary>
        /// Invokes the hourly update.
        /// </summary>
        private void InvokeHourlyUpdate() { Hourly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        /// <summary>
        /// The daily update
        /// </summary>
        private Thread DailyUpdate;
        /// <summary>
        /// Invokes the daily update.
        /// </summary>
        private void InvokeDailyUpdate() { Daily.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        /// <summary>
        /// The weekly update
        /// </summary>
        private Thread WeeklyUpdate;
        /// <summary>
        /// Invokes the weekly update.
        /// </summary>
        private void InvokeWeeklyUpdate() { Weekly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        /// <summary>
        /// The monthly update
        /// </summary>
        private Thread MonthlyUpdate;
        /// <summary>
        /// Invokes the monthly update.
        /// </summary>
        private void InvokeMonthlyUpdate() { Monthly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        /// <summary>
        /// The quarterly update
        /// </summary>
        private Thread QuarterlyUpdate;
        /// <summary>
        /// Invokes the quarterly update.
        /// </summary>
        private void InvokeQuarterlyUpdate() { Quarterly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        /// <summary>
        /// The biannually update
        /// </summary>
        private Thread BiannuallyUpdate;
        /// <summary>
        /// Invokes the binannually update.
        /// </summary>
        private void InvokeBinannuallyUpdate() { Biannually.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        /// <summary>
        /// The yearly update
        /// </summary>
        private Thread YearlyUpdate;
        /// <summary>
        /// Invokes the yearly update.
        /// </summary>
        private void InvokeYearlyUpdate() { Yearly.Invoke(this, new TimeEventArgs(this.CurrentTime)); }

        /// <summary>
        /// Initializes a new instance of the <see cref="Universe"/> class.
        /// </summary>
        public Universe() {
            Timescale = 20.0f;

        }
        /// <summary>
        /// Starts the background thread.
        /// </summary>
        private void startBackgroundThread() {
            
            BackgroundUpdate.IsBackground = false;
            BackgroundUpdate.Start();
        }

        /// <summary>
        /// Updates the specified frametime.
        /// </summary>
        /// <param name="frametime">The frametime.</param>
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
        }
        /// <summary>
        /// Checks the quarterly.
        /// </summary>
        /// <param name="_prevTime">The _prev time.</param>
        /// <returns>System.Boolean.</returns>
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

        /// <summary>
        /// 6
        /// Sets the number by which the Time gets incremented each update tick.
        /// </summary>
        /// <param name="timescale">The scale of time.</param>
        public void SetTimescale(float timescale) {
            this.Timescale = timescale;
        }
    }
}
