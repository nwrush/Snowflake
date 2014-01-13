namespace Haswell {
    public struct ResourceVal {
        private const int MAXVAL = 100;
        private int val;
        public int Val {
            get {
                return this.val;
            }
            set {
                if (-1 < value && value < MAXVAL)
                    this.val = value;
            }
        }
    }
}
