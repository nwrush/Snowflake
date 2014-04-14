using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Haswell {
    public static partial class Controller {

        public static float Time { get; private set; }
        public static float Timescale { get; private set; }


        public const float DayLength = 2400.0f;
        public const float HourLength = 100.0f;
        public const float MinuteLength = 1.6666667f;

        public static DateTime CurrentTime { get; private set; }

        public static void SetTimescale(float timescale) {
            Timescale = timescale;
        }
    }
}
