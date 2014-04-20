using System;
using System.Collections.Generic;
using System.Text;

using Mogre;

using Snowflake.States;
using Snowflake.GuiComponents;

using Haswell;

namespace Snowflake {
    public class WeatherManager {

        public List<Weather> PastWeather;

        private ParticleSystem rainSystem;
        //private ParticleSystem fogSystem;
        //private ParticleSystem snowSystem;
        private SceneNode particleNode;

        private Light sun;
        private Light ambient;

        private Random randomizer;
        private float timer;

        public WeatherManager() {
            randomizer = new Random();
            PastWeather = new List<Weather>();
        }

        public void CreateScene(SceneManager sm) {
            sun = sm.CreateLight("Sun");
            sun.Type = Light.LightTypes.LT_POINT;
            sun.Position = new Vector3(0, 1000, 100);
            sun.Direction = new Vector3(0, -1, 0.5f);
            sun.DiffuseColour = new ColourValue(0.98f, 0.95f, 0.9f);
            sun.SpecularColour = ColourValue.White;
            sun.CastShadows = true;
            sun.PowerScale = 5.0f;

            ambient = sm.CreateLight("ambient");
            ambient.Type = Light.LightTypes.LT_DIRECTIONAL;
            ambient.Position = new Vector3(0, 2000, 0);
            ambient.Direction = new Vector3(0, -1, 0);
            ambient.DiffuseColour = new ColourValue(0.01f, 0.05f, 0.1f);
            ambient.SpecularColour = ColourValue.Black;
            ambient.CastShadows = false;

            sm.RootSceneNode.AttachObject(sun);
            sm.RootSceneNode.AttachObject(ambient);

            rainSystem = sm.CreateParticleSystem("Rain", "Weather/Rain");
            particleNode = sm.GetSceneNode("focalPoint").CreateChildSceneNode("Weather");
            particleNode.AttachObject(rainSystem);
            particleNode.Translate(new Vector3(0, 600, 0));
        }

        public void Update(SceneManager sm) {

            float t = CityManager.Time;
            float pi = (float)System.Math.PI;
            float ts = CityManager.Timescale;
            if (t == 0.0) { return; }

            float daynight = -(float)System.Math.Cos(t * (2 * pi / CityManager.DayLength));
            float daynightx = (float)System.Math.Sin(t * (2 * pi / CityManager.DayLength));
            float season = -(float)System.Math.Cos((t / 31536000.0) * (2 * pi));
            sun.Position = new Vector3(0 /*10000 * daynightx*/, 10000 /* * daynight*/, (int)(-10000.0f * season));
            //CityManager.GuiMgr.SetDebugText(daynight.ToString());

            //brightness
            // ----> float multiplier = (float)System.Math.Max(0.0, System.Math.Sign(daynight) * System.Math.Pow(daynight, (1.0 / 3.0)));
            //float shadowCoef = System.Math.Max(1.0f - multiplier, GetCloudiness() * 0.5f + 0.5f);
            //sm.ShadowColour = new ColourValue(shadowCoef, shadowCoef, shadowCoef);
            // ----> sun.DiffuseColour = new ColourValue(0.98f * multiplier, 0.95f * multiplier, 0.9f * multiplier);
            //sm.AmbientLight = new ColourValue(sun.DiffuseColour.r * GetCloudiness() + 0.1f, sun.DiffuseColour.g * GetCloudiness() + 0.1f, sun.DiffuseColour.b * GetCloudiness() + 0.1f);

            float col = (float)System.Math.Max(daynight, 0.0) * 0.7f + 0.2f;
            rainSystem.GetEmitter(0).Colour = new ColourValue(col, col, col, 0.6f);
            
            //Fade in/out particle system emission rates
            if (!(this.CurrentWeather == Weather.Rainy 
                || this.CurrentWeather == Weather.Stormy)
                && rainSystem.GetEmitter(0).EmissionRate > 0) { 
                rainSystem.GetEmitter(0).EmissionRate -= ts; 
            }
            if (this.CurrentWeather == Weather.Rainy 
                && rainSystem.GetEmitter(0).EmissionRate < 800) { 
                rainSystem.GetEmitter(0).EmissionRate += ts;
            }
            if (this.CurrentWeather == Weather.Stormy 
                && rainSystem.GetEmitter(0).EmissionRate < 1600) { 
                rainSystem.GetEmitter(0).EmissionRate += ts; 
            }
        }

        private void ResetTimer() {
            timer = randomizer.Next(1000, 4800);
        }

        public Weather CurrentWeather {
            get {
                return Weather.Sunny;
            }
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
            //CurrentWeather = w;

            switch (CurrentWeather) {
                case Weather.Null:
                    //CurrentWeather = Weather.Sunny;
                    break;
                case Weather.Sunny:
                    break;

            }
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

