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

        public static void saveCommercial(Buildings.Commercial c) {
            StreamWriter sw = File.CreateText(COMMERCIAL_FILE);
            sw.WriteLine(JsonConvert.SerializeObject(c, Formatting.Indented));
            sw.Close();
        }
        public static Buildings.Commercial loadCommercial() {
            try {
                StreamReader sr = File.OpenText(COMMERCIAL_FILE);
                return JsonConvert.DeserializeObject<Buildings.Commercial>(sr.ReadToEnd());
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }

        public static void saveIndustrial(Buildings.Industrial i) {
            StreamWriter sw = File.CreateText(INDUSTRIAL_FILE);
            sw.WriteLine(JsonConvert.SerializeObject(i, Formatting.Indented));
            sw.Close();
        }
        public static Buildings.Industrial loadIndustrial() {
            try {
                StreamReader sr = File.OpenText(COMMERCIAL_FILE);
                return JsonConvert.DeserializeObject<Buildings.Industrial>(sr.ReadToEnd());
            } catch (Exception e) {
                System.Diagnostics.Debug.WriteLine(e);
                return null;
            }
        }
    }
}
