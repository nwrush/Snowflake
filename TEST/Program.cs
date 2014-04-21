using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Haswell;

namespace TEST {
    class Program {
        static List<Person> citizens = new List<Person>();
        static void Main(string[] args) {
            Haswell.Controller.init("Help");

            Point p = new Point(12, 12);
            Console.WriteLine(p);
            Console.ReadKey();
        }
    }
}
