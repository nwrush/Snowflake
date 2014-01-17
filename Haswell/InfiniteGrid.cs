using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell {
    class InfiniteGrid:ICollection<Plot> {
        List<Plot> elements;
        private bool isReadOnly = false;
        public InfiniteGrid() {
            elements = new List<Plot>();
        }

        public Plot ElementAt(int x, int y) {
            foreach (Plot e in elements) {
                if (e.X == x && e.Y == y) {
                    return e;
                }
            }
            return null;
        }
        public Plot RemoveAt(int x, int y) {
            for (int i = 0; i < elements.Count; i++) {
                if (elements[i].X == x && elements[i].Y == y) {
                    Plot tmp = elements[i];
                    elements.RemoveAt(i);
                    return tmp;
                }
            }
            throw new ElementNotFoundException();
        }
        //ICollection
        void ICollection<Plot>.Add(Plot e) {
            foreach (Plot p in elements) {
                if (e==p) {
                    throw new StackOverflowException();
                }
            }
            elements.Add(e);
        }

        void ICollection<Plot>.Clear() {
            this.elements.Clear();
        }

        bool ICollection<Plot>.Contains(Plot item) {
            foreach (Plot p in this.elements) {
                if (item==p) {
                    return true;
                }
            }
            return false;
        }

        void ICollection<Plot>.CopyTo(Plot[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        int ICollection<Plot>.Count {
            get { return this.elements.Count; }
        }

        bool ICollection<Plot>.IsReadOnly {
            get { return this.isReadOnly; }
        }

        bool ICollection<Plot>.Remove(Plot item) {
            this.elements.Remove(item);
            return true;
        }

        IEnumerator<Plot> IEnumerable<Plot>.GetEnumerator() {
            return this.elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.elements.GetEnumerator();
        }
    }
}
