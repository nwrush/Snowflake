using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Haswell.Buildings
{
    [Serializable]
    public class Industrial : Building
    {
        private string CONFIGURATIONFILE = "Building Configuration/Industrial_";

        private double _income;
        private int _employees;
        private int _pollution;

        public Industrial(BuildingConfiguration _bc)
            : base(Zones.Industrial)
        {
            this._buildingConfig = _bc;

        }
        private void LoadConfiguration(BuildingConfiguration _bc)
        {
            if (0 < _bc.Version && _bc.Version <= 3)
            {
                switch (_bc.Version)
                {
                    case 1:
                        this.Income = 1000000;
                        this.Employees = 250;
                        this.PollutionAmount = 100;
                        break;
                    case 2:
                        this.Income = 5000000;
                        this.Employees = 750;
                        this.PollutionAmount = 500;
                        break;
                    case 3:
                        this.Income = 75000000;
                        this.Employees = 1000;
                        this.PollutionAmount = 750;
                        break;
                    default:
                        goto case 1;
                }
            }
        }

        public override void Update(ResourceDict plotResources)
        {
            base.Update(plotResources);
        }
        public override void UpdateDaily(ResourceDict plotResources)
        {
            base.UpdateDaily(plotResources);
        }
        public override void UpdateWeekly(ResourceDict plotResources)
        {
            base.UpdateWeekly(plotResources);
        }
        public override void UpdateMonthly(ResourceDict plotResources)
        {
            base.UpdateMonthly(plotResources);
        }
        public override void UpdateQuarterly(ResourceDict plotResources)
        {
            base.UpdateQuarterly(plotResources);
        }
        public override void UpdateBiannually(ResourceDict plotResources)
        {
            base.UpdateBiannually(plotResources);
        }
        public override void UpdateYearly(ResourceDict plotResources)
        {
            base.UpdateYearly(plotResources);
        }

        public double Income
        {
            get
            {
                return this._income;
            }
            private set
            {
                this._income = value;
            }
        }
        public int Employees
        {
            get
            {
                return this._employees;
            }
            private set
            {
                this._employees = value;
            }
        }
        public int PollutionAmount
        {
            get
            {
                return this._pollution;
            }
            private set
            {
                this._pollution = value;
            }
        }
    }
}
