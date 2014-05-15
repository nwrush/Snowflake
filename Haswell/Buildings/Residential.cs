using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Haswell.Buildings
{
    [Serializable]
    public class Residential : Building, IBuilding
    {
        private const string CONFIGURATIONFILE = "Building Configuration/Residential_";

        private int _residents;
        private float _income;

        private Residential() : base(Zones.Residential) { }
        public Residential(BuildingConfiguration _bc)
            : base(Zones.Residential)
        {
            this._buildingConfig = _bc;
            //LoadConfiguration(_bc);
        }
        private Residential(int residents, int income):base (Zones.Residential)
        {
            this._residents = residents;
            this._income = income;
        }
        private void LoadConfiguration(BuildingConfiguration _bc)
        {
            if (0 < _bc.Version && _bc.Version <= 3)
            {
                #region TODO
                //string configurationText = File.OpenText(CONFIGURATIONFILE + _bc.Version + ".json").ReadToEnd();

                //JsonTextReader reader = new JsonTextReader(new StringReader(configurationText));

                //string s;

                //while ((s = reader.ReadAsString()) == null)
                //{
                //    if (s == null)
                //        break;

                //    this.configuration[s] = reader.Value;
                //}
                #endregion
                switch (_bc.Version)
                {
                    case 1:
                        this._residents = 4;
                        this._income = 10000;
                        break;
                    case 2:
                        this._residents = 500;
                        this._income = 20000;
                        break;
                    case 3:
                        this._residents = 16;
                        this._income = 100000;
                        break;
                    default:
                        goto case 1;
                }
            }
        }

        public override void Update(ResourceDict pltRes)
        {
            base.Update(pltRes);
        }
        public override void UpdateDaily(ResourceDict pltRes)
        {
            base.UpdateDaily(pltRes);
        }
        public override void UpdateWeekly(ResourceDict pltRes)
        {
            base.UpdateWeekly(pltRes);
        }
        public override void UpdateMonthly(ResourceDict pltRes)
        {
            base.UpdateMonthly(pltRes);
        }
        public override void UpdateQuarterly(ResourceDict pltRes)
        {
            base.UpdateQuarterly(pltRes);
        }
        public override void UpdateBiannually(ResourceDict pltRes)
        {
            base.UpdateBiannually(pltRes);
        }
        public override void UpdateYearly(ResourceDict pltRes)
        {
            PayTaxes(pltRes);
            base.UpdateYearly(pltRes);
        }
        //Todo: Remember to reset this
        protected void PayTaxes(ResourceDict plot)
        {
            //plot[ResourceType.Money] += this._income * 0.15f;
            plot[ResourceType.Money] += 100;
        }

        public int Residents
        {
            get
            {
                return this._residents;
            }
            set
            {
                this._residents = value;
            }
        }
        public float Income
        {
            get
            {
                return this._income;
            }
        }

    }
}
