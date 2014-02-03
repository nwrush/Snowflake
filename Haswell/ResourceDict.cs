using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class ResourceDict : IDictionary<Resource.Type, int> {
        private Dictionary<Resource.Type, int> resources;

        /// <summary>
        /// Initializes a new ResourceDict using the default values
        /// </summary>
        public ResourceDict() {
            this.resources = new Dictionary<Resource.Type, int>();
            this.resources.Add(Resource.Type.Money, 0);
            this.resources.Add(Resource.Type.Population, 0);
            this.resources.Add(Resource.Type.Energy, 0);
            this.resources.Add(Resource.Type.Material, 0);
        }
        /// <summary>
        /// Initializes a new Resource Dict from a Dictionary of Resource Types
        /// </summary>
        /// <param name="r">The dictionary to use in creation</param>
        public ResourceDict(Dictionary<Resource.Type, int> r) {
            this.resources = new Dictionary<Resource.Type, int>(r);
        }

        #region IDictionary Stuff
        public void Add(Resource.Type key, int value) {
            resources.Add(key, value);
        }

        public bool ContainsKey(Resource.Type key) {
            return this.resources.ContainsKey(key);
        }

        public ICollection<Resource.Type> Keys {
            get { return this.resources.Keys; }
        }

        public bool Remove(Resource.Type key) {
            return this.resources.Remove(key);
        }

        public bool TryGetValue(Resource.Type key, out int value) {
            return this.resources.TryGetValue(key, out value);
        }

        public ICollection<int> Values {
            get { return this.resources.Values; }
        }

        public int this[Resource.Type key] {
            get {
                return this.resources[key];
            }
            set {
                this.resources[key] = value;
            }
        }

        public void Add(KeyValuePair<Resource.Type, int> item) {
            this.resources.Add(item.Key, item.Value);
        }

        public void Clear() {
            this.resources.Clear();
        }

        public bool Contains(KeyValuePair<Resource.Type, int> item) {
            return this.resources.Contains(item);
        }

        public void CopyTo(KeyValuePair<Type, int>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return this.resources.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(KeyValuePair<Resource.Type, int> item) {
            return this.resources.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<Resource.Type, int>> GetEnumerator() {
            return this.resources.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.resources.GetEnumerator();
        }
        public void CopyTo(KeyValuePair<Resource.Type, int>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }
        #endregion

        public static ResourceDict operator +(ResourceDict r1, ResourceDict r2) {
            ResourceDict rd = new ResourceDict();
            foreach (Resource.Type type in Enum.GetValues(typeof(Resource.Type))) {
                rd[type] = r1[type] + r2[type];
            }
            return rd;
        }

        public override string ToString() {
            String sb = "";
            sb += "Energy: " + this.resources[Resource.Type.Energy].ToString() + "\n";
            sb += "Material: " + this.resources[Resource.Type.Material].ToString() + "\n";
            sb += "Money: " + this.resources[Resource.Type.Money].ToString() + "\n";
            sb += "Energy: " + this.resources[Resource.Type.Energy].ToString();
            return sb;
        }
    }
}
