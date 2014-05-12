using System;
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
        private const string RESIDENTIAL_1 = "Building Configuration/Residential_1.json";
        private const string RESIDENTIAL_2 = "Building Configuration/Residential_2.json";
        private const string RESIDENTIAL_3 = "Building Configuration/Residential_3.json";

        private ResidentialTypes residentialType;
        private int _residents;
        private float _income;

        public Residential()
            : base(Zones.Residential)
        {
        }
        public Residential(ResidentialTypes r)
            : base(Zones.Residential)
        {
            this.residentialType = r;
            Residential tmp;
            tmp = JsonConvert.DeserializeObject<Residential>(GetResidentialTemplate(r));
            this._income = tmp._income;
            this._residents = tmp.Residents;
        }
        private string GetResidentialTemplate(ResidentialTypes r)
        {
            string fileText = "";

            switch (r)
            {
                case ResidentialTypes.Residential_1:
                    fileText = File.OpenText(RESIDENTIAL_1).ReadToEnd();
                    break;
                case ResidentialTypes.Residential_2:
                    fileText = File.OpenText(RESIDENTIAL_2).ReadToEnd();
                    break;
                case ResidentialTypes.Residential_3:
                    fileText = File.OpenText(RESIDENTIAL_3).ReadToEnd();
                    break;
            }

            return fileText;
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

        public ResidentialTypes Type
        {
            get
            {
                return this.residentialType;
            }
            private set
            {
                this.residentialType = value;
            }
        }
    }
    public enum ResidentialTypes
    {
        Residential_1,
        Residential_2,
        Residential_3
    };
}
