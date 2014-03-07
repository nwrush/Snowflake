using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haswell;

namespace TEST {
    class Program {
        static List<Person> citizens = new List<Person>();
        static void Main(string[] args) {
            Haswell.Controller.init("Help");

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}
