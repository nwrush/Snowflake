using System;
using System.Collections.Generic;
using System.Text;

namespace DRT.Buildings {
    public class ResidentialBuilding : Building {
        public ResidenceType Type;
    }

    public enum ResidenceType {
        House,
        Apartment
    }
}
