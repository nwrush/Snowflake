using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Haswell {
    public class InfiniteGrid : ICollection<Plot> {
        List<Plot> elements;

        public InfiniteGrid() {
            elements = new List<Plot>();
        }

        public Plot ElementAt(int x, int y) {
            foreach (Plot e in elements) {
                if (e.X == x && e.Y == y) {
                    return e;
                }
            }
            /*don't return null because that's dumb 
            //return null;
            instead, since there's no grid element for these coordinates, let's create one */
            Plot p = new Plot(x, y);
            elements.Add(p);
            return p;
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

        public Plot[,] ToGrid() {
            Plot[,] tmp = null;
            makePlot(tmp);
            populateGrid(tmp);
            return tmp;
        }

        private void makePlot(Plot[,] t) {
            int lowX = 0, lowY = 0, highX = 0, highY = 0;
            foreach (Plot p in this.elements) {
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
        private void populateGrid(Plot[,] t) {
            foreach (Plot p in this.elements) {
                t[p.X, p.Y] = p;
            }
        }

        #region ICollection Stuff
        //ICollection
        private bool isReadOnly = false;

        void ICollection<Plot>.Add(Plot e) {
            foreach (Plot p in elements) {
                if (e == p) {
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
                if (item == p) {
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
        #endregion
    }
}
