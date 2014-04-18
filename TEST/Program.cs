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

            while (true) {
                Haswell.Controller.Update(1f);
                Console.WriteLine("Update");
            }
        }
    }
}
