using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    class ResourceDict : IDictionary<Resource, int> {
        private Dictionary<Resource, int> resources;

        public ResourceDict() {
            this.resources = new Dictionary<Resource, int>();
        }
        public ResourceDict(Dictionary<Resource, int> r) {
            this.resources = new Dictionary<Resource, int>(r);
        }


        #region IDictionary Stuff
        public void Add(Resource key, int value) {
            resources.Add(key, value);
        }

        public bool ContainsKey(Resource key) {
            return this.resources.ContainsKey(key);
        }

        public ICollection<Resource> Keys {
            get { return this.resources.Keys; }
        }

        public bool Remove(Resource key) {
            return this.resources.Remove(key);
        }

        public bool TryGetValue(Resource key, out int value) {
            return this.resources.TryGetValue(key, out value);
        }

        public ICollection<int> Values {
            get { return this.resources.Values; }
        }

        public int this[Resource key] {
            get {
                throw new NotImplementedException();
            }
            set {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<Resource, int> item) {
            this.resources.Add(item.Key, item.Value);
        }

        public void Clear() {
            this.resources.Clear();
        }

        public bool Contains(KeyValuePair<Resource, int> item) {
            return this.resources.Contains(item);
        }

        public void CopyTo(KeyValuePair<Resource, int>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public int Count {
            get { return this.resources.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(KeyValuePair<Resource, int> item) {
            return this.resources.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<Resource, int>> GetEnumerator() {
            return this.resources.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.resources.GetEnumerator();
        }
        #endregion
    }
}
