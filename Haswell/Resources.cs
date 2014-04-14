using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    /// <summary>
    /// Stuct for holding a resource Value
    /// </summary>
    public struct Resource {//Todo: Remove this struct, it isn't used
        /// <summary>
        /// The value
        /// </summary>
        float value;
        /// <summary>
        /// The name
        /// </summary>
        string name;
        /// <summary>
        /// Initializes a new Resource with the given values
        /// </summary>
        /// <param name="_name">Name of the resource</param>
        /// <param name="_value">Value of the resource</param>
        public Resource(string _name, float _value) {
            this.name = _name;
            this.value = _value;
        }
        /// <summary>
        /// Initializes a new Resource with the given name and the default value for the Resouce 0.
        /// </summary>
        /// <param name="_name">Name of the Resource</param>
        public Resource(string _name):this(_name,0.0f){}

    }
    /// <summary>
    /// The different types of resources available
    /// </summary>
    public enum ResourceType {
        /// <summary>
        /// The material
        /// </summary>
        Material,
        /// <summary>
        /// The energy
        /// </summary>
        Energy,
        /// <summary>
        /// The money
        /// </summary>
        Money,
        /// <summary>
        /// The population
        /// </summary>
        Population,
        /// <summary>
        /// The none
        /// </summary>
        None
    };
}
