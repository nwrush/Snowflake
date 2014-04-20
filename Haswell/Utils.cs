using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Haswell {
    static class Utils {

        public static void SerializeCity(City c) {
            IFormatter serializer = new BinaryFormatter();
            Stream stream = new FileStream("City.bin", FileMode.Create, FileAccess.Write, FileShare.None);
            serializer.Serialize(stream, c);
            stream.Close();
        }

    }
}
