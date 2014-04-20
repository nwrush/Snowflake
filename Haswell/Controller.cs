using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Haswell {
    public delegate void Serialized();
    public static partial class Controller {

        private static City activeCity;
        private static BackgroundThread backgroundThread;

        public static void init(string name) {
            activeCity = new City(name);
            System.IO.File.Delete("Snowflake.log");
            backgroundThread = new BackgroundThread();

            //Time Values
            Timescale = 100.0f;
        }

        public static void Update(float frametime) {
            activeCity.Update(frametime);

            //Time
            DateTime _prevTime = CurrentTime;
            Time += Timescale;
            CurrentTime = new DateTime(1970, 1, 1).AddMinutes(Time / MinuteLength);

            CheckTimedUpdates(_prevTime, CurrentTime);
        }
        private static void CheckTimedUpdates(DateTime _prevTime, DateTime time) {
            if (_prevTime.Hour != time.Hour) {
                backgroundThread.Events.Enqueue(UpdateHour);
            }
            if (_prevTime.Day != time.Day) {
                backgroundThread.Events.Enqueue(UpdateDaily);
            }
            if (_prevTime.DayOfWeek != time.DayOfWeek && time.DayOfWeek == DayOfWeek.Monday) {
                backgroundThread.Events.Enqueue(UpdateWeekly);
            }
            if (_prevTime.Month != time.Month) {
                backgroundThread.Events.Enqueue(UpdateMonthly);
            }
            if (CheckQuarterly(_prevTime, time)) {
                backgroundThread.Events.Enqueue(UpdateQuarterly);
            }
            if ((time.Month == 1 || time.Month == 6) && time.Month != _prevTime.Month) {
                backgroundThread.Events.Enqueue(UpdateBiannually);
            }
            if (_prevTime.Year != time.Year) {
                backgroundThread.Events.Enqueue(UpdateYearly);
            }
        }
        private static bool CheckQuarterly(DateTime _prevTime, DateTime _time) {
            if (_time.Month == _prevTime.Month)
                return false;

            if (_time.Month == 1)
                return true;
            else if (_time.Month == 4)
                return true;
            else if (_time.Month == 7)
                return true;
            else if (_time.Month == 10)
                return true;

            return false;
        }

        public delegate void UpdateDelegate(DateTime time);
        private static void UpdateHour(DateTime time) {
            activeCity.UpdateHour(time);
        }
        private static void UpdateDaily(DateTime time) {
            activeCity.UpdateDaily(time);
        }
        private static void UpdateWeekly(DateTime time) {
            activeCity.UpdateWeekly(time);
        }
        private static void UpdateMonthly(DateTime time) {
            activeCity.UpdateMonthly(time);
        }
        private static void UpdateQuarterly(DateTime time) {
            activeCity.UpdateQuarterly(time);
        }
        private static void UpdateBiannually(DateTime time) {
            activeCity.UpdateBiannually(time);
        }
        private static void UpdateYearly(DateTime time) {
            activeCity.UpdateYearly(time);
        }

        [Obsolete("Do Not Call this function")]
        public static void LogError(Exception e) {
            String msg = "\"" + e.Message + "\"";
            msg += " " + "\"" + e.StackTrace + "\"";
            Process p = new Process();
            Process.Start("Log.exe", msg);
        }


        public static event Serialized OnSerialized;
        /// <summary>
        /// Saves a copy of the current city to City.bin
        /// </summary>
        public static void Save() {
            BinaryFormatter serializer = new BinaryFormatter();
            Stream stream = new FileStream("TSPI.city", FileMode.Create, FileAccess.Write, FileShare.None);
            serializer.Serialize(stream, activeCity);
            stream.Close();
            OnSerialized.Invoke();
        }

        public static event Serialized OnDeserialized;
        /// <summary>
        /// Load a copy of the city from a binary file
        /// </summary>
        public static void Load() {
            return;
            BinaryFormatter deserializer = new BinaryFormatter();
            //Todo: Better way to get the name of the city
            FileStream stream = new FileStream("TSPI.city", FileMode.Open, FileAccess.Read, FileShare.None);
            City c = (City)deserializer.Deserialize(stream);
            activeCity = c;
            OnDeserialized.Invoke();
        }

        public static City City {
            get {
                return activeCity;
            }
        }
        public static float ChanceThatThisProgramBecomesSkynet {
            get {
                return 0.0000001f;
            }
        }
        public static BackgroundThread BackgroundThread {
            get {
                return backgroundThread;
            }
        }
    }
}
