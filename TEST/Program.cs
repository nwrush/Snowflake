using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haswell;

namespace TEST {
    class Program {
        static void Main(string[] args) {
            City TEST = new City("TEST");

            try {
                Haswell.Controller.Update(long.MaxValue);
            } catch (Exception e) {
                Haswell.Controller.LogError(e);
            }

            Console.ReadKey();
        }
    }
}
