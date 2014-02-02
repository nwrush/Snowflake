using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    public class ResourceDict : IDictionary<Resource.Type, int> {
        private Dictionary<Resource.Type, int> resources;

        public ResourceDict() {
            this.resources = new Dictionary<Resource.Type, int>();
        }
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
    }
}
