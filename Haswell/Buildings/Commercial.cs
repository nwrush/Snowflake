using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings
{
    /// <summary>
    /// Class Commercial.
    /// </summary>
    [Serializable]
    public class Commercial : Building, IBuilding
    {
        private CommercialTypes commercialType;

        public Commercial() : base(Zones.Commercial) { }
        public Commercial(BuildingConfiguration _bc)
            : base(Zones.Commercial)
        {

        }
        public Commercial(CommercialTypes c)
            : base(Zones.Commercial)
        {
            this.Type = commercialType;
        }
        protected Commercial(Commercial c)
            : base(Zones.Commercial)
        {
            this._facing = c.Facing;
            this.Parent = c.Parent;
            this.Initialized = c.Initialized;
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
