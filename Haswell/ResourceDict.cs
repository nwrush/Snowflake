using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class ResourceDict : IDictionary<ResourceType, float> {
        private Dictionary<ResourceType, float> resources;

        /// <summary>
        /// Initializes a new ResourceDict using the default values
        /// </summary>
        public ResourceDict() {
            this.resources = new Dictionary<ResourceType, float>();
            this.resources.Add(ResourceType.Money, 0);
            this.resources.Add(ResourceType.Population, 0);
            this.resources.Add(ResourceType.Energy, 0);
            this.resources.Add(ResourceType.Material, 0);
        }
        /// <summary>
        /// Initializes a new Resource Dict from a Dictionary of Resource Types
        /// </summary>
        /// <param name="r">The dictionary to use in creation</param>
        public ResourceDict(Dictionary<ResourceType, float> r) {
            this.resources = new Dictionary<ResourceType, float>(r);
        }

        #region IDictionary Stuff
        public void Add(ResourceType key, float value) {
            resources.Add(key, value);
        }

        public bool ContainsKey(ResourceType key) {
            return this.resources.ContainsKey(key);
        }

        public ICollection<ResourceType> Keys {
            get { return this.resources.Keys; }
        }

        public bool Remove(ResourceType key) {
            return this.resources.Remove(key);
        }

        public bool TryGetValue(ResourceType key, out float value) {
            return this.resources.TryGetValue(key, out value);
        }

        public ICollection<float> Values {
            get { return this.resources.Values; }
        }

        public float this[ResourceType key] {
            get {
                return this.resources[key];
            }
            set {
                this.resources[key] = value;
            }
        }

        public void Add(KeyValuePair<ResourceType, float> item) {
            this.resources.Add(item.Key, item.Value);
        }

        public void Clear() {
            this.resources.Clear();
        }

        public bool Contains(KeyValuePair<ResourceType, float> item) {
            return this.resources.Contains(item);
        }

        public void CopyTo(KeyValuePair<Type, float>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return this.resources.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(KeyValuePair<ResourceType, float> item) {
            return this.resources.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<ResourceType, float>> GetEnumerator() {
            return this.resources.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.resources.GetEnumerator();
        }
        public void CopyTo(KeyValuePair<ResourceType, float>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }
        #endregion

        public static ResourceDict operator +(ResourceDict r1, ResourceDict r2) {
            ResourceDict rd = new ResourceDict();
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType))) {
                rd[type] = r1[type] + r2[type];
            }
            return rd;
        }

        public override string ToString() {
            String sb = "";
            sb += "Energy: " + this.resources[ResourceType.Energy].ToString() + "\n";
            sb += "Material: " + this.resources[ResourceType.Material].ToString() + "\n";
            sb += "Money: " + this.resources[ResourceType.Money].ToString() + "\n";
            sb += "Energy: " + this.resources[ResourceType.Energy].ToString();
            return sb;
        }
    }
}
