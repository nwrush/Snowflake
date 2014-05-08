using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    /// <summary>
    /// Class Person.
    /// </summary>
    [Serializable]
    public class Person : IComparable<Person>
    {
        /// <summary>
        /// The _name
        /// </summary>
        private string _name;
        /// <summary>
        /// The _age
        /// </summary>
        private int _age;
        /// <summary>
        /// The _ TTL
        /// </summary>
        private uint _TTL;

        /// <summary>
        /// The _residence
        /// </summary>
        private Building _residence;
        /// <summary>
        /// The _employer
        /// </summary>
        private Building _employer;

        /// <summary>
        /// The _health
        /// </summary>
        private float _health;
        /// <summary>
        /// The _education
        /// </summary>
        private float _education;
        /// <summary>
        /// The _happiness
        /// </summary>
        private float _happiness;

        /// <summary>
        /// Constructs a person using the specified parameters
        /// </summary>
        /// <param name="name">Name of the person to create</param>
        /// <param name="age">Age of the person to create</param>
        /// <param name="TTL">The TTL.</param>
        /// <param name="health">Health of the person as a float between 0 and 1, with 0 as about to die and 1 as completely healthy</param>
        /// <param name="education">Education of the person as a float between 0 and 1, with 0 as no education and 1 as very educated</param>
        /// <param name="happiness">Happiness of the person as a float between 0 and 1, with 0 as very unhappy and 1 as completely satisfied</param>
        public Person(string name, int age, uint TTL, float health, float education, float happiness) : this(name, age, TTL, health, education, happiness, null, null) { }
        /// <summary>
        /// Constructs a person using the specified parameters
        /// </summary>
        /// <param name="name">Name of the person to create</param>
        /// <param name="age">Age of the person to create</param>
        /// <param name="TTL">The TTL.</param>
        /// <param name="health">Health of the person as a float between 0 and 1, with 0 as about to die and 1 as completely healthy</param>
        /// <param name="education">Education of the person as a float between 0 and 1, with 0 as no education and 1 as very educated</param>
        /// <param name="happiness">Happiness of the person as a float between 0 and 1, with 0 as very unhappy and 1 as completely satisfied</param>
        /// <param name="residence">Building that the person lives in</param>
        /// <param name="employer">Building that the person works at or goes to school at</param>
        public Person(string name, int age, uint TTL, float health, float education, float happiness, Building residence, Building employer)
        {
            this._name = name;
            this._age = age;
            this._TTL = TTL;
            this._health = health;
            this._education = education;
            this._happiness = happiness;
            this._residence = residence;
            this._employer = employer;
        }

        /// <summary>
        /// Calculates the income.
        /// </summary>
        /// <returns>System.Double.</returns>
        public double CalculateIncome()
        {
            throw new NotImplementedException("Calculate Income is not yet implemmented");
        }

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <value>The age.</value>
        public int Age
        {
            get
            {
                return this._age;
            }
            private set
            {
                if (value < 0)
                    value = 0;
                this._age = value;
            }
        }
        /// <summary>
        /// Gets or sets the health.
        /// </summary>
        /// <value>The health.</value>
        public float Health
        {
            get
            {
                return this._health;
            }
            set
            {
                if (!(0 <= value && value <= 1))
                {
                    if (value < 0)
                        value = 0;
                    else if (1 < value)
                        value = 1;
                }
                this._health = value;
            }
        }
        /// <summary>
        /// Gets or sets the education.
        /// </summary>
        /// <value>The education.</value>
        public float Education
        {
            get
            {
                return this._education;
            }
            set
            {
                if (!(0 <= value && value <= 1))
                {
                    if (value < 0)
                        value = 0;
                    else if (1 < value)
                        value = 1;
                }
                this._education = value;
            }
        }
        /// <summary>
        /// Gets or sets the happiness.
        /// </summary>
        /// <value>The happiness.</value>
        public float Happiness
        {
            get
            {
                return this._happiness;
            }
            set
            {
                if (!(0 <= value && value <= 1))
                {
                    if (value < 0)
                        value = 0;
                    else if (1 < value)
                        value = 1;
                }
                this._happiness = value;
            }
        }
        /// <summary>
        /// Gets the residence.
        /// </summary>
        /// <value>The residence.</value>
        public Building Residence
        {
            get
            {
                return this._residence;
            }
            private set
            {
                this._residence = value;
            }
        }
        /// <summary>
        /// Gets or sets the employer.
        /// </summary>
        /// <value>The employer.</value>
        public Building Employer
        {
            get
            {
                return this._employer;
            }
            set
            {
                this._employer = value;
            }
        }

        /// <summary>
        /// Compares this instance of the Person to others based on TTL
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(Person other)
        {
            return this._TTL.CompareTo(other._TTL);
        }

        /// <summary>
        /// Updates the yearly.
        /// </summary>
        /// <param name="city">The city.</param>
        public void UpdateYearly(City city)
        {
            this._TTL = GetTTL(city);
        }
        /// <summary>
        /// Calculates the persons new Time to Live based on current enviromental factors
        /// </summary>
        /// <param name="city">The city to get the enviromental conditions from</param>
        /// <returns>The new TTL value</returns>
        private uint GetTTL(City city)
        {
            throw new NotImplementedException();
        }
    }
}
