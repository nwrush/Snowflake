using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark {
    public abstract class Animal : IComparable<Animal> {
        private string _name;
        private float _health;
        private int _rank;

        protected Animal(string name, float health, int rank) {
            this._name = name;
            this._health = health;
            this._rank = rank;
        }

        public int CompareTo(Animal other) {
            return this._rank.CompareTo(other._rank);
        }
    }
}
