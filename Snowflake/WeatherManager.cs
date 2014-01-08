using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

using Snowflake.States;
using Snowflake.GuiComponents;

namespace Snowflake {
    public class WeatherManager {

        public List<Weather> PastWeather;
        public Weather CurrentWeather;
        public Weather NextWeather;

        private Light sun;
        private Light sky;

        private Random randomizer;
        private float timer;
        public float Time = 0.0f;
        public float Timescale = 1.0f;
        public DateTime FormattedTime;

        private WeatherOverlay overlay;

        public WeatherManager() {
            randomizer = new Random();
            PastWeather = new List<Weather>();
            FormattedTime = DateTime.Now;
        }

        public void CreateScene(SceneManager sm) {
            sun = sm.CreateLight("Sun");
            sun.Type = Light.LightTypes.LT_POINT;
            sun.Position = new Vector3(0, 1000, 100);
            sun.Direction = new Vector3(0, -1, 0.5f);
            sun.DiffuseColour = new ColourValue(0.98f, 0.95f, 0.9f);
            sun.SpecularColour = ColourValue.White;
            //sun.AttenuationQuadratic = 0.01f;
            //sun.AttenuationLinear = 0.1f;
            sun.CastShadows = true;

            sky = sm.CreateLight("sky");
            sky.Type = Light.LightTypes.LT_DIRECTIONAL;
            sky.Position = new Vector3(0, 2000, 0);
            sky.Direction = new Vector3(0, -1, 0);
            sky.DiffuseColour = new ColourValue(0.05f, 0.075f, 0.10f);
            sky.SpecularColour = ColourValue.Black;
            sky.CastShadows = true;

            sm.RootSceneNode.AttachObject(sun);
            sm.RootSceneNode.AttachObject(sky);

            timer = randomizer.Next(1000);
        }

        public void Update() {
            
            Time += Timescale;
            
            sun.Position = new Vector3(1000 * (float)System.Math.Cos(Time / -200.0), 1000 * (float)System.Math.Sin(Time / -200.0), -300);

            if (timer <= 0) {
                SwitchWeather((Weather)Enum.GetValues(typeof(Weather)).GetValue(randomizer.Next(1, Enum.GetValues(typeof(Weather)).Length)));
            }
            else {
                timer -= Timescale;
            }
        }

        private void ResetTimer() {
            timer = randomizer.Next(800, 1600);
        }
        
        /// <summary>
        ///Switches to the specified weather state, 
        ///*not yet* performing necessary transitions
        ///and resetting the timer
        /// </summary>
        /// <param name="w">Weather to switch to</param>
        public void SwitchWeather(Weather w) {
            ForceWeather(w);
            ResetTimer();
        }

        /// <summary>
        ///Forces the specified weather state without
        ///resetting the timer.
        /// </summary>
        /// <param name="w">Weather to force</param>
        public void ForceWeather(Weather w) {
            PastWeather.Add(w);
            CurrentWeather = w;

            switch (CurrentWeather) {
                case Weather.Null:
                    CurrentWeather = Weather.Sunny;
                    break;
                case Weather.Sunny:
                    break;

            }

            overlay.SetWeatherIcon(w);
        }
        
        public void SetWeatherOverlay(WeatherOverlay wo) {
            this.overlay = wo;
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

