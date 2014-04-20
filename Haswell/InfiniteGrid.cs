using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Haswell {
    /// <summary>
    /// Class InfiniteGrid.
    /// </summary>
    [Serializable]
    public class InfiniteGrid : ICollection<Plot>, IEnumerable<Plot> {
        /// <summary>
        /// The elements
        /// </summary>
        Dictionary<Point, Plot> elements;

        /// <summary>
        /// Initializes a new instance of the <see cref="InfiniteGrid"/> class.
        /// </summary>
        public InfiniteGrid() {
            elements = new Dictionary<Point, Plot>();
            for (int r = -10; r <= 10; r++) {
                for (int c = -10; c <= 10; c++) {
                    elements.Add(new Point(r, c), new Plot(r, c));
                }
            }
        }

        public Plot ElementAt(int x, int y) {
            if (!elements.ContainsKey(new Point(x,y))){
                Plot p = new Plot(x, y);
                elements[new Point(x, y)] = p;
                return p;
            } else {
                return elements[new Point(x, y)];
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

        public List<Plot> GetNeighbors(Plot p)
        {
            List<Plot> neighbors = new List<Plot>();
            neighbors.Add(ElementAt(p.X + 1, p.Y));
            neighbors.Add(ElementAt(p.X - 1, p.Y));
            neighbors.Add(ElementAt(p.X, p.Y + 1));
            neighbors.Add(ElementAt(p.X, p.Y - 1));
            return neighbors;
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
        /// <summary>
        /// Populates the grid.
        /// </summary>
        /// <param name="t">The t.</param>
        private void populateGrid(ref Plot[,] t) {
            foreach (KeyValuePair<Point, Plot> kvp in this.elements) {
                Plot p = kvp.Value;
                t[p.X, p.Y] = p;
            }
        }

        #region ICollection Stuff
        //ICollection
        /// <summary>
        /// The is read only
        /// </summary>
        private bool isReadOnly = false;
        /// <summary>
        /// Gets or sets the is read only.
        /// </summary>
        /// <value>The is read only.</value>
        public bool IsReadOnly {
            get {
                return this.isReadOnly;
            }
            set {
                this.isReadOnly = value;
            }
        }

        /// <summary>
        /// Adds.
        /// </summary>
        /// <param name="e">The e.</param>
        void ICollection<Plot>.Add(Plot e) {
            if (elements[new Point(e.X, e.Y)] != null) {
                throw new ElementAlreadyExistsException();
            } else {
                elements[new Point(e.X, e.Y)] = e;
            }
        }

        /// <summary>
        /// Clears.
        /// </summary>
        void ICollection<Plot>.Clear() {
            this.elements.Clear();
        }

        /// <summary>
        /// Containses.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Boolean.</returns>
        bool ICollection<Plot>.Contains(Plot item) {
            if (elements[new Point(item.X, item.Y)] != null) {
                if (item == elements[new Point(item.X, item.Y)]) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        void ICollection<Plot>.CopyTo(Plot[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        int ICollection<Plot>.Count {
            get { return this.elements.Count; }
        }

        /// <summary>
        /// Gets the is read only.
        /// </summary>
        /// <value>The is read only.</value>
        bool ICollection<Plot>.IsReadOnly {
            get { return this.isReadOnly; }
        }

        /// <summary>
        /// Removes.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Boolean.</returns>
        bool ICollection<Plot>.Remove(Plot item) {
            this.elements.Remove(new Point(item.X, item.Y));
            return true;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>System.Collections.Generic.IEnumerator&lt;Haswell.Plot&gt;.</returns>
        IEnumerator<Plot> IEnumerable<Plot>.GetEnumerator() {
            return this.elements.Values.GetEnumerator();
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>System.Collections.IEnumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.elements.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// Returns a List of plots in the specified selection
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
                    if(this.elements.ContainsKey(new Point(r,c)))
                        tmp.Add(this.elements[new Point(r,c)]);
                }
            }
            return tmp;
        }
    }
}
