using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake {
    public class WeatherManager {

        public List<Weather> PastWeather;
        public Weather CurrentWeather;
        public Weather NextWeather;

        private int timer;

        public void Update() {
            timer -= 1;
        }

        public void SwitchWeather(Weather w) {
            PastWeather.Add(w);
            CurrentWeather = w;

        }
    }

    public enum Weather {
        Null,
        Sunny,
        Cloudy,
        Rainy,
        Stormy,
        Windy,
        Foggy
    }
}
