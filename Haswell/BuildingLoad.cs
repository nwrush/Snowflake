using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace Haswell {
    public static class BuildingLoad {
        private const string RESIDENTIAL_FILE = "Residential.json";
        private const string COMMERCIAL_FILE = "Commercial.json";
        private const string INDUSTRIAL_FILE = "Industrial.json";

        public static void saveResidential(Buildings.Residential r) {
            StreamWriter sw = File.CreateText(RESIDENTIAL_FILE);
            sw.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));
            sw.Close();
        }
        public static Buildings.Residential loadResidential() {
            try {
                StreamReader sr = File.OpenText(RESIDENTIAL_FILE);
                return JsonConvert.DeserializeObject<Buildings.Residential>(sr.ReadToEnd());
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }
    }
}
