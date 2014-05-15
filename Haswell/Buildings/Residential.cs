﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Newtonsoft.Json;

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
            LoadConfiguration(_bc);
        }
        private void LoadConfiguration(BuildingConfiguration _bc)
        {
            if (0 < _bc.Version && _bc.Version <= 3)
            {
                string configurationText = File.OpenText(CONFIGURATIONFILE + _bc.Version + ".json").ReadToEnd();

                Residential tmp = JsonConvert.DeserializeObject<Residential>(configurationText);

                this._income = tmp.Income;
                this.Residents = tmp.Residents;
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
