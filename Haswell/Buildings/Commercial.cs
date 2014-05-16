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

        private CommercialTypes commercialType;
        
        private int income;
        private 

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

                        break;
                    case 2:

                        break;
                    case 3:

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

        private CommercialTypes Type
        {
            get
            {
                return this.commercialType;
            }
            set
            {
                this.commercialType = value;
            }
        }
    }
    public enum CommercialTypes
    {
        thing1,
        thing2,
        thing3
    };
}
