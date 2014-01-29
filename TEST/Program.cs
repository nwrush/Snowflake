using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haswell;

namespace TEST {
    class Program {
        static void Main(string[] args) {
            Console.ReadKey();
        }
    }
    class Fruit {
        string name;
        protected Fruit(string n) {
            this.name = n;
        }
        public virtual void Exit() { }
        public override string ToString() {
            return this.name;
        }
    }
    class Apple : Fruit {
        public Apple() : base("Apple") { }
        public override void Exit() { Environment.Exit(10); }
    }
    class Banana : Fruit {
        public Banana() : base("Banana") { }
        public override void Exit() { Console.WriteLine("LOL"); }
    }
}
