using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell
{
    /// <summary>
    /// Class ResourceDict.
    /// </summary>
    [Serializable]
    public class ResourceDict : IDictionary<ResourceType, float>
    {
        /// <summary>
        /// The resources
        /// </summary>
        private Dictionary<ResourceType, float> resources;

        /// <summary>
        /// Initializes a new ResourceDict using the default values
        /// </summary>
        public ResourceDict()
        {
            this.resources = new Dictionary<ResourceType, float>();
            this.resources.Add(ResourceType.Money, 0);
            this.resources.Add(ResourceType.Population, 0);
            this.resources.Add(ResourceType.Energy, 0);
            this.resources.Add(ResourceType.Material, 0);
            this.resources.Add(ResourceType.Pollution, 0);
        }
        /// <summary>
        /// Initializes a new Resource Dict from a Dictionary of Resource Types
        /// </summary>
        /// <param name="r">The dictionary to use in creation</param>
        public ResourceDict(Dictionary<ResourceType, float> r)
        {
            this.resources = new Dictionary<ResourceType, float>(r);
        }

        #region IDictionary Stuff
        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(ResourceType key, float value)
        {
            resources.Add(key, value);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        public bool ContainsKey(ResourceType key)
        {
            return this.resources.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<ResourceType> Keys
        {
            get { return this.resources.Keys; }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Boolean.</returns>
        public bool Remove(ResourceType key)
        {
            return this.resources.Remove(key);
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Boolean.</returns>
        public bool TryGetValue(ResourceType key, out float value)
        {
            return this.resources.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public ICollection<float> Values
        {
            get { return this.resources.Values; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Single"/> with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.Single.</returns>
        public float this[ResourceType key]
        {
            get
            {
                return this.resources[key];
            }
            set
            {
                this.resources[key] = value;
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(KeyValuePair<ResourceType, float> item)
        {
            this.resources.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.resources.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        public bool Contains(KeyValuePair<ResourceType, float> item)
        {
            return this.resources.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<Type, float>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return this.resources.Count; }
        }

        /// <summary>
        /// Gets the is read only.
        /// </summary>
        /// <value>The is read only.</value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Boolean.</returns>
        public bool Remove(KeyValuePair<ResourceType, float> item)
        {
            return this.resources.Remove(item.Key);
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>System.Collections.Generic.IEnumerator&lt;System.Collections.Generic.KeyValuePair&lt;Haswell.ResourceType,System.Single&gt;&gt;.</returns>
        public IEnumerator<KeyValuePair<ResourceType, float>> GetEnumerator()
        {
            return this.resources.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>System.Collections.IEnumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.resources.GetEnumerator();
        }
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<ResourceType, float>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="r1">The r1.</param>
        /// <param name="r2">The r2.</param>
        /// <returns>The result of the operator.</returns>
        public static ResourceDict operator +(ResourceDict r1, ResourceDict r2)
        {
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                r1[type] = r1[type] + r2[type];
            }
            return r1;
        }

        /// <summary>
        /// To the string.
        /// </summary>
        /// <returns>System.String.</returns>
        public override string ToString()
        {
            String sb = "\n";
            sb += "Energy: " + this.resources[ResourceType.Energy].ToString() + ", \n";
            sb += "Material: " + this.resources[ResourceType.Material].ToString() + ", \n";
            sb += "Money: " + this.resources[ResourceType.Money].ToString() + ", \n";
            sb += "Population: " + this.resources[ResourceType.Population].ToString();
            return sb;
        }
    }
    public enum ResourceType
    {
        Material,
        Energy,
        Money,
        Population,
        Pollution
    };
}
