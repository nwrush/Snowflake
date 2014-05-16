using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace Haswell.Buildings
{
    /// <summary>
    /// Class Commercial.
    /// </summary>
    [Serializable]
    public class Commercial : Building, IBuilding
    {
        private const string CONFIGURATIONFILE = "Building Configuration/Commercial_";

        private double _income;
        private int _employees;

        public Commercial(BuildingConfiguration _bc)
            : base(Zones.Commercial)
        {
            this._buildingConfig = _bc;
            LoadConfiguration(_bc);
        }
        private void LoadConfiguration(BuildingConfiguration _bc)
        {
            if (0 < _bc.Version && _bc.Version <= 3)
            {
                switch (_bc.Version)
                {
                    case 1:
                        this._income = 70000;
                        this._employees = 500;
                        break;
                    case 2:
                        this._income = 1000000;
                        this._employees = 1000;
                        break;
                    case 3:
                        this._income = 50000000;
                        this._employees = 75000;
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
        protected override void Pollute(ResourceDict plot)
        {
            throw new NotImplementedException();
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
        protected override void PayTaxes(ResourceDict plot)
        {
            plot[ResourceType.Money] += ((float)this._income * 0.15f) / 4;
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
        }
        public int Employees
        {
            get
            {
                return this._employees;
            }
        }
    }
}
