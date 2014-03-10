using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark {
    public abstract class Animal : IComparable<Animal> {
        /// <summary>
        /// Name of the animal
        /// </summary>
        private string _name;
        /// <summary>
        /// Health of the animal as a float from 0 to 1, with 0 dead and 1 completely healthy
        /// </summary>
        private float _health;
        /// <summary>
        /// Rank of the animal on the food chain, lower number the lower on the food chain
        /// </summary>
        private uint _rank;

        protected Animal(string name, float health, uint rank) {
            this._name = name;
            this._health = health;
            this._rank = rank;
        }

        public int CompareTo(Animal other) {
            return this._rank.CompareTo(other._rank);
        }
        protected virtual void Update() {

        }
    }
}
