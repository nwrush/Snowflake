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

        public Commercial(BuildingConfiguration _bc)
            : base(Zones.Commercial)
        {
            this._buildingConfig = _bc;

        }
        private void LoadConfiguration(BuildingConfiguration _bc)
        {
            if (0 < _bc.Version && _bc.Version <= 3)
            {
                string configText = File.OpenText(CONFIGURATIONFILE + _bc.Version + ".json").ReadToEnd();
                Commercial tmp = JsonConvert.DeserializeObject<Commercial>(configText);
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
