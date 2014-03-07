using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class Person :IComparable<Person>{
        private string _name;
        private int _age;
        private DateTime _birthDay;

        private Building _residence;
        private Building _employer;

        private float _health;
        private float _education;
        private float _happiness;

        /// <summary>
        /// Constructs a person using the specified parameters
        /// </summary>
        /// <param name="name">Name of the person to create</param>
        /// <param name="age">Age of the person to create</param>
        /// <param name="health">Health of the person as a float between 0 and 1, with 0 as about to die and 1 as completely healthy</param>
        /// <param name="education">Education of the person as a float between 0 and 1, with 0 as no education and 1 as very educated</param>
        /// <param name="happiness">Happiness of the person as a float between 0 and 1, with 0 as very unhappy and 1 as completely satisfied</param>
        public Person(string name, int age, float health, float education, float happiness) : this(name, age, health, education, happiness, null, null) { }
        /// <summary>
        /// Constructs a person using the specified parameters
        /// </summary>
        /// <param name="name">Name of the person to create</param>
        /// <param name="age">Age of the person to create</param>
        /// <param name="health">Health of the person as a float between 0 and 1, with 0 as about to die and 1 as completely healthy</param>
        /// <param name="education">Education of the person as a float between 0 and 1, with 0 as no education and 1 as very educated</param>
        /// <param name="happiness">Happiness of the person as a float between 0 and 1, with 0 as very unhappy and 1 as completely satisfied</param>
        /// <param name="residence">Building that the person lives in</param>
        /// <param name="employer">Building that the person works at or goes to school at</param>
        public Person(string name, int age, float health, float education, float happiness, Building residence, Building employer) {
            this._name = name;
            this._age = age;
            this._birthDay = Controller.Environment.CurrentTime;
            this._health = health;
            this._education = education;
            this._happiness = happiness;
            this._residence = residence;
            this._employer = employer;
        }

        public double CalculateIncome() {
            throw new NotImplementedException("Calculate Income is not yet implemmented");
        }

        public int Age {
            get {
                return this._age;
            }
            private set {
                if (value < 0)
                    value = 0;
                this._age = value;
            }
        }
        public float Health {
            get {
                return this._health;
            }
            set {
                if (!(0 <= value && value <= 1)) {
                    if (value < 0)
                        value = 0;
                    else if (1 < value)
                        value = 1;
                }
                this._health = value;
            }
        }
        public float Education {
            get {
                return this._education;
            }
            set {
                if (!(0 <= value && value <= 1)) {
                    if (value < 0)
                        value = 0;
                    else if (1 < value)
                        value = 1;
                }
                this._education = value;
            }
        }
        public float Happiness {
            get {
                return this._happiness;
            }
            set {
                if (!(0 <= value && value <= 1)) {
                    if (value < 0)
                        value = 0;
                    else if (1 < value)
                        value = 1;
                }
                this._happiness = value;
            }
        }
        public Building Residence {
            get {
                return this._residence;
            }
            private set {
                this._residence = value;
            }
        }
        public Building Employer {
            get {
                return this._employer;
            }
            set {
                this._employer = value;
            }
        }


        /// <summary>
        /// Compares this instance of the Person to others based on birthdate
        /// </summary>
        public int CompareTo(Person other) {
            return this._birthDay.CompareTo(other._birthDay);
        }
    }
}
