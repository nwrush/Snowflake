using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    public enum BuildingType
    {
        Residential,
        Commercial,
        Industrial,
        Infrastructure
    };

    public class BuildingConfiguration
    {

        private readonly BuildingType buildingType;
        private readonly int version;

        public BuildingConfiguration(BuildingType _bt, int _version)
        {
            this.buildingType = _bt;
            this.version = _version;
        }

        public BuildingType BuildingType
        {
            get
            {
                return this.buildingType;
            }
        }
        public int Version
        {
            get
            {
                return this.version;
            }
        }

        public override string ToString()
        {
            string tmp = "";
            tmp += this.buildingType.ToString();
            tmp += ", " + this.version;
            return tmp;
        }
        public static bool operator ==(BuildingConfiguration b1, BuildingConfiguration b2)
        {
            if (b1.buildingType == b2.buildingType && b1.version == b2.version)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(BuildingConfiguration b1, BuildingConfiguration b2)
        {
            if (b1 == b2)
            {
                return false;
            }
            return true;
        }
        public override bool Equals(object obj)
        {
            return (Haswell.BuildingConfiguration)obj == this;
        }
        public override int GetHashCode()
        {
            return this.version.GetHashCode() + this.buildingType.GetHashCode();
        }
    }
}
