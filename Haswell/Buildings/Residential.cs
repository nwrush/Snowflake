using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    public class Residential : Building, IBuilding {
        private int _residents;
        private float _income;

        public Residential()
            : base(Zone.Type.Residential) {
            this.resouceChanges.Add(ResourceType.Money, 100);
        }
        public Residential(int residents, float income)
            : base(Zone.Type.Residential) {
            this._residents = residents;
            this._income = income;
        }

        public override void Update(ResourceDict pltRes) { base.Update(pltRes); }

        public override void UpdateHour(ResourceDict pltRes) {
            base.UpdateHour(pltRes);
        }

        public override void UpdateDaily(ResourceDict pltRes) {
            base.UpdateDaily(pltRes);
        }

        public override void UpdateWeekly(ResourceDict pltRes) {
            base.UpdateWeekly(pltRes);
        }

        public override void UpdateMonthly(ResourceDict pltRes) {
            base.UpdateMonthly(pltRes);
        }

        public override void UpdateQuaterly(ResourceDict pltRes) {
            base.UpdateQuaterly(pltRes);
        }

        public override void UpdataBiannually(ResourceDict pltRes) {
            base.UpdataBiannually(pltRes);
        }

        public override void UpdateYearly(ResourceDict pltRes) {
            PayTaxes(pltRes[ResourceType.Money]);
            base.UpdateYearly(pltRes);
        }
        protected void PayTaxes(float plotMoney) {
            plotMoney += this._income * 0.15f;
        }
    }
}
