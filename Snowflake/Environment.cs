using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

using Snowflake.States;
using Snowflake.GuiComponents;

namespace Snowflake {
    public class Environment {

        public List<Weather> PastWeather;
        public Weather CurrentWeather;
        public Weather NextWeather;

        private ParticleSystem rainSystem;
        private ParticleSystem fogSystem;
        private ParticleSystem snowSystem;
        private SceneNode particleNode;

        private Light sun;
        private Light ambient;

        private Random randomizer;
        private float timer;
        public float Time = 0.0f;
        public float Timescale = 1.0f;
        public DateTime FormattedTime;

        //Relative length of days, hours, and minutes, according to a tick of 1.0.
        private const float DayLength = 2400.0f;
        private const float HourLength = 100.0f;
        private const float MinuteLength = 1.6666667f;

        private WeatherOverlay overlay;

        public Environment() {
            randomizer = new Random();
            PastWeather = new List<Weather>();
            FormattedTime = new DateTime(2014, 01, 01, 6, 0, 0);
        }

        public void CreateScene(SceneManager sm) {
            sun = sm.CreateLight("Sun");
            sun.Type = Light.LightTypes.LT_POINT;
            sun.Position = new Vector3(0, 1000, 100);
            sun.Direction = new Vector3(0, -1, 0.5f);
            sun.DiffuseColour = new ColourValue(0.98f, 0.95f, 0.9f);
            sun.SpecularColour = ColourValue.White;
            sun.CastShadows = true;

            ambient = sm.CreateLight("ambient");
            ambient.Type = Light.LightTypes.LT_DIRECTIONAL;
            ambient.Position = new Vector3(0, 2000, 0);
            ambient.Direction = new Vector3(0, -1, 0);
            ambient.DiffuseColour = new ColourValue(0.005f, 0.0075f, 0.010f);
            ambient.SpecularColour = ColourValue.Black;
            ambient.CastShadows = true;

            sm.RootSceneNode.AttachObject(sun);
            sm.RootSceneNode.AttachObject(ambient);

            rainSystem = sm.CreateParticleSystem("Rain", "Weather/Rain");
            particleNode = sm.GetSceneNode("focalPoint").CreateChildSceneNode("Weather");
            particleNode.AttachObject(rainSystem);
            particleNode.Translate(new Vector3(0, 600, 0));

            timer = randomizer.Next(1000, 4800);
        }

        public void Update() {
            
            Time += Timescale;
            UpdateFormatTime(0, 0, Timescale / MinuteLength);

            sun.Position = new Vector3(1000 * (float)System.Math.Cos(Time * (2 * System.Math.PI / DayLength)), 1000 * (float)System.Math.Sin(Time * (2 * System.Math.PI / DayLength)), -300);

            //brightness
            float multiplier = (float)System.Math.Max(0.0, System.Math.Pow(sun.Position.y / 1000, 3));
            sun.DiffuseColour = new ColourValue(0.98f * multiplier, 0.95f * multiplier, 0.9f * multiplier);

            rainSystem.GetEmitter(0).Colour = new ColourValue(sun.DiffuseColour.b * 0.7f + 0.2f, sun.DiffuseColour.b * 0.7f + 0.2f, sun.DiffuseColour.b * 0.7f + 0.2f, 0.6f);

            if (!(this.CurrentWeather == Weather.Rainy || this.CurrentWeather == Weather.Stormy) && rainSystem.GetEmitter(0).EmissionRate > 0) { rainSystem.GetEmitter(0).EmissionRate -= Timescale; }
            if (this.CurrentWeather == Weather.Rainy && rainSystem.GetEmitter(0).EmissionRate < 800) { rainSystem.GetEmitter(0).EmissionRate += Timescale; }
            if (this.CurrentWeather == Weather.Stormy && rainSystem.GetEmitter(0).EmissionRate < 1600) { rainSystem.GetEmitter(0).EmissionRate += Timescale; }

            //Temp weather change code (ultimately will be handled in Nikko's code)
            if (timer <= 0) {
                SwitchWeather((Weather)Enum.GetValues(typeof(Weather)).GetValue(randomizer.Next(1, Enum.GetValues(typeof(Weather)).Length)));
            }
            else {
                timer -= Timescale;
            }
        }

        private void UpdateFormatTime(double days = 0, double hours = 0, double minutes = 0) {
            FormattedTime = FormattedTime.AddDays(days);
            FormattedTime = FormattedTime.AddHours(hours);
            FormattedTime = FormattedTime.AddMinutes(minutes);
            this.overlay.UpdateTimeLabel(FormattedTime);
        }

        private void ResetTimer() {
            timer = randomizer.Next(1000, 4800);
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

