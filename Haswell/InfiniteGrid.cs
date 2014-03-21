using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Haswell {
    public class InfiniteGrid : ICollection<Plot>, IEnumerable<Plot> {
        Dictionary<Point, Plot> elements;

        public InfiniteGrid() {
            elements = new Dictionary<Point, Plot>();

            for (int r = -10; r <= 10; r++) {
                for (int c = -10; c <= 10; c++) {
                    elements.Add(new Point(r, c), new Plot(r, c));
                }
            }
        }

        public Plot ElementAt(int x, int y) {
            try {
                Plot p = elements[new Point(x, y)];
                return p;
            } catch (KeyNotFoundException e) {
                //For now, add a plot where its trying to find one
                //In the future, don't catch this exception and let the caller deal with it.
                System.Diagnostics.Debug.WriteLine(e);
                Plot p = new Plot(x, y);
                elements[new Point(x, y)] = p;
                return p;
            }
        }
        public Plot RemoveAt(int x, int y) {
            Plot tmp = elements[new Point(x, y)];
            elements.Remove(new Point(x, y));
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

            foreach (KeyValuePair<Point, Plot> kvp in this.elements) {
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
            foreach (KeyValuePair<Point, Plot> kvp in this.elements) {
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
            if (elements[new Point(e.X, e.Y)] != null) {
                throw new ElementAlreadyExistsException();
            } else {
                elements[new Point(e.X, e.Y)] = e;
            }
        }

        void ICollection<Plot>.Clear() {
            this.elements.Clear();
        }

        bool ICollection<Plot>.Contains(Plot item) {
            if (elements[new Point(item.X, item.Y)] != null) {
                if (item == elements[new Point(item.X, item.Y)]) { return true; }
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
            this.elements.Remove(new Point(item.X, item.Y));
            return true;
        }

        IEnumerator<Plot> IEnumerable<Plot>.GetEnumerator() {
            return this.elements.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.elements.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Returns an array of plots in the specified selection
        /// </summary>
        /// <param name="topLeft">Top left corner of the selection</param>
        /// <param name="bottomRight">Bottom right corner of the selection</param>
        /// <returns>The list of selected plots</returns>
        public List<Plot> GetAllInSelection(Point topLeft, Point bottomRight) {
            return this.GetAllInSelection(new Rectangle(topLeft.X, topLeft.Y, topLeft.X - bottomRight.X, topLeft.Y - bottomRight.Y));
        }
        /// <summary>
        /// Returns a list of plots in the specified selection
        /// </summary>
        /// <param name="selection">The rectangle with the selection</param>
        /// <returns>The list of selected plots</returns>
        public List<Plot> GetAllInSelection(Rectangle selection) {
            //This method and the one above it is the worst thing I think I've ever written, fix it eventually
            List<Plot> tmp = new List<Plot>();
            for (int r = selection.Left; r <= selection.Right; ++r) {
                for (int c = selection.Bottom; c <= selection.Top; ++c) {
                    try {
                        tmp.Add(this.elements[new Point(r, c)]);
                    } catch (KeyNotFoundException e) { continue; }
                }
            }
            return null;
        }
    }
}
