using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings
{
    [Serializable]
    public class Residential : Building, IBuilding, ICloneable
    {

        private int _residents;
        private float _income;

        public Residential()
            : base(Zones.Residential)
        {
        }
        public Residential(ResidentialTypes r)
            : base(Zones.Residential)
        {

        }
        public Residential(int residents, float income)
            : base(Zones.Residential)
        {
            this._residents = residents;
            this._income = income;
        }
        /// <summary>
        /// Constructor for the Clone Function
        /// </summary>
        /// <param name="r">The building to create a copy from.</param>
        protected Residential(Residential r)
            : base(Zones.Residential)
        {
            this._facing = r.Facing;
            this._income = r._income;
            this._residents = r._residents;
            this.Parent = r.Parent;
            this.Initialized = r.Initialized;
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
        [Obsolete("Are you sure you need to use this?")]
        public override object Clone()
        {
            return new Residential(this);
        }
    }
    public enum ResidentialTypes
    {
        thing1,
        thing2,
        things
    };
}
