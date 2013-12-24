﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Buildings {
    public interface CommercialBuilding {
        public int Employees; //Current number of workers. Should be MinJobs < Employees < MaxJobs
        protected int MaxJobs; //Job capacity - don't hire more than this.
        protected int MinJobs; //Minimum number of jobs to run - won't operate if this is not met.
    }
}