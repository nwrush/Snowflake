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

        public static implicit operator ResourceVal(int i) {
            if (-1 < i && i < MAXVAL) {
                ResourceVal r = new ResourceVal();
                r.val = i;
                return r;
            }
            throw new InvalidResourceAmountException();
        }
    }

    [System.Serializable]
    public class InvalidResourceAmountException : System.Exception {
        public InvalidResourceAmountException() { }
        public InvalidResourceAmountException(string message) : base(message) { }
        public InvalidResourceAmountException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidResourceAmountException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
