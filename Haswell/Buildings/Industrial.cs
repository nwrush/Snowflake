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
        private IndustrialTypes industrialType;

        public Industrial() : base(Zones.Industrial) { }
        public Industrial(BuildingConfiguration _bc)
            : base(Zones.Industrial)
        {
            this._buildingConfig = _bc;

        }
        private void LoadConfiguration(BuildingConfiguration _bc)
        {
            if (0 < _bc.Version && _bc.Version <= 3)
            {
                string configText = File.OpenText(CONFIGURATIONFILE + _bc.Version + ".json").ReadToEnd();
                Industrial tmp = JsonConvert.DeserializeObject<Industrial>(configText);
            }
        }
        public Industrial(IndustrialTypes i)
            : base(Zones.Industrial)
        {

        }
        protected Industrial(Industrial i)
            : base(Zones.Industrial)
        {
            this._facing = i.Facing;
            this.Parent = i.Parent;
            this.Initialized = i.Initialized;
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

        public IndustrialTypes Type
        {
            get
            {
                return this.industrialType;
            }
            set
            {
                this.industrialType = value;
            }
        }
    }
    public enum IndustrialTypes
    {
        thing1,
        thing2,
        thing3
    };
}
