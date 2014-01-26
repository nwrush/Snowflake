using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Haswell {
    public class InfiniteGrid : ICollection<Plot> {
        Dictionary<string,Plot> elements;

        public InfiniteGrid() {
            elements = new Dictionary<string, Plot>();
        }

        public Plot ElementAt(int x, int y) {
            try {
                Plot p = elements[x.ToString() + "," + y.ToString()];
                return p;
            }
            catch (KeyNotFoundException e) {
                //For now, add a plot where its trying to find one
                //In the future, don't catch this exception and let the caller deal with it.
                Plot p = new Plot(x, y);
                elements[x.ToString() + "," + y.ToString()] = p;
                return p;
            }
        }
        public Plot RemoveAt(int x, int y) {
            Plot tmp = elements[x.ToString() + "," + y.ToString()];
            elements.Remove(x.ToString() + "," + y.ToString());
            return tmp;
        }

        public Plot[,] ToGrid() {
            Plot[,] tmp = null;
            makePlot(tmp);
            populateGrid(ref tmp);
            return tmp;
        }

        private void makePlot(Plot[,] t) {
            int lowX = 0, lowY = 0, highX = 0, highY = 0;

            foreach (KeyValuePair<string, Plot> kvp in this.elements) {
                Plot p = kvp.Value;
                if (p.X > highX)
                    highX = p.X;
                else if (p.X < lowX)
                    lowX = p.X;

                if (p.Y > highY)
                    highY = p.Y;
                else if (p.Y < lowY)
                    lowY = p.Y;
            }
            t = new Plot[highX - lowX, highY - lowY];
        }
        private void populateGrid(ref Plot[,] t) {
            foreach (KeyValuePair<string, Plot> kvp in this.elements) {
                Plot p = kvp.Value;
                t[p.X, p.Y] = p;
            }
        }

        #region ICollection Stuff
        //ICollection
        private bool isReadOnly = false;
        public bool IsReadOnly {
            get {
                return this.isReadOnly;
            }
            set {
                this.isReadOnly = value;
            }
        }

        void ICollection<Plot>.Add(Plot e) {
            if (elements[e.X.ToString() + "," + e.Y.ToString()] != null) {
                throw new ElementAlreadyExistsException();
            }
            else {
                elements[e.X.ToString() + "," + e.Y.ToString()] = e;
            }
        }

        void ICollection<Plot>.Clear() {
            this.elements.Clear();
        }

        bool ICollection<Plot>.Contains(Plot item) {
            if (elements[item.X.ToString() + "," + item.Y.ToString()] != null) {
                if (item == elements[item.X.ToString() + "," + item.Y.ToString()]) { return true; }
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
            this.elements.Remove(item.X.ToString() + "," + item.Y.ToString());
            return true;
        }

        IEnumerator<Plot> IEnumerable<Plot>.GetEnumerator() {
            return this.elements.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.elements.GetEnumerator();
        }
        #endregion
    }
}
