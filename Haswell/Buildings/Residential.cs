using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Buildings {
    /// <summary>
    /// Class Residential.
    /// </summary>
    public class Residential : Building, IBuilding {
        /// <summary>
        /// The _residents
        /// </summary>
        private int _residents;
        /// <summary>
        /// The _income
        /// </summary>
        private float _income;

        /// <summary>
        /// Initializes a new instance of the <see cref="Residential"/> class.
        /// </summary>
        public Residential()
            : base(Zones.Residential) {
            this.resouceChanges.Add(ResourceType.Money, 100);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Residential"/> class.
        /// </summary>
        /// <param name="residents">The residents.</param>
        /// <param name="income">The income.</param>
        public Residential(int residents, float income)
            : base(Zones.Residential) {
            this._residents = residents;
            this._income = income;
        }

        /// <summary>
        /// Updates the specified PLT resource.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void Update(ResourceDict pltRes) {
            base.Update(pltRes);
        }

        /// <summary>
        /// Updates the hour.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void UpdateHour(ResourceDict pltRes) {
            base.UpdateHour(pltRes);
        }

        /// <summary>
        /// Updates the daily.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void UpdateDaily(ResourceDict pltRes) {
            base.UpdateDaily(pltRes);
        }

        /// <summary>
        /// Updates the weekly.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void UpdateWeekly(ResourceDict pltRes) {
            base.UpdateWeekly(pltRes);
        }

        /// <summary>
        /// Updates the monthly.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void UpdateMonthly(ResourceDict pltRes) {
            base.UpdateMonthly(pltRes);
        }

        /// <summary>
        /// Updates the Quarterly.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void UpdateQuarterly(ResourceDict pltRes) {
            base.UpdateQuarterly(pltRes);
        }

        /// <summary>
        /// Updates the biannually.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void UpdateBiannually(ResourceDict pltRes) {
            base.UpdateBiannually(pltRes);
        }

        /// <summary>
        /// Updates the yearly.
        /// </summary>
        /// <param name="pltRes">The PLT resource.</param>
        public override void UpdateYearly(ResourceDict pltRes) {
            PayTaxes(pltRes[ResourceType.Money]);
            base.UpdateYearly(pltRes);
        }
        /// <summary>
        /// Pays the taxes.
        /// </summary>
        /// <param name="plotMoney">The plot money.</param>
        protected void PayTaxes(float plotMoney) {
            plotMoney += this._income * 0.15f;
        }

        /// <summary>
        /// Gets or sets the residents.
        /// </summary>
        /// <value>The residents.</value>
        public int Residents {
            get {
                return this._residents;
            }
            set {
                this._residents = value;
            }
        }
    }
}
