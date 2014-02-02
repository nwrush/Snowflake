using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    class ResourceDict : IDictionary<Type, int> {
        private Dictionary<Type, int> resources;

        public ResourceDict() {
            this.resources = new Dictionary<Type, int>();
        }
        public ResourceDict(Dictionary<Type, int> r) {
            this.resources = new Dictionary<Type, int>(r);
        }

        #region IDictionary Stuff
        public void Add(Type key, int value) {
            resources.Add(key, value);
        }

        public bool ContainsKey(Type key) {
            return this.resources.ContainsKey(key);
        }

        public ICollection<Type> Keys {
            get { return this.resources.Keys; }
        }

        public bool Remove(Type key) {
            return this.resources.Remove(key);
        }

        public bool TryGetValue(Type key, out int value) {
            return this.resources.TryGetValue(key, out value);
        }

        public ICollection<int> Values {
            get { return this.resources.Values; }
        }

        public int this[Type key] {
            get {
                return this.resources[key];
            }
            set {
                this.resources[key] = value;
            }
        }

        public void Add(KeyValuePair<Type, int> item) {
            this.resources.Add(item.Key, item.Value);
        }

        public void Clear() {
            this.resources.Clear();
        }

        public bool Contains(KeyValuePair<Type, int> item) {
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

        public bool Remove(KeyValuePair<Type, int> item) {
            return this.resources.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<Type, int>> GetEnumerator() {
            return this.resources.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.resources.GetEnumerator();
        }
        #endregion
    }
}
