using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haswell.Exceptions {
    [Serializable]
    public class BuildingCreationFailedException : Exception
    {
        public BuildingCreationFailedException() { }
      public BuildingCreationFailedException( string message ) : base( message ) { }
      public BuildingCreationFailedException( string message, Exception inner ) : base( message, inner ) { }
      protected BuildingCreationFailedException( 
	    System.Runtime.Serialization.SerializationInfo info, 
	    System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}
