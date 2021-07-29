using HAN.Lib.Structure;

namespace HAN.Lib.ProgressSystem
{
    public static class Keys
    {
        public static ProgressSystemTypesKeys Types { get { return ProgressSystemTypesKeys.Instance; } }
        public class ProgressSystemTypesKeys : Singleton<ProgressSystemTypesKeys>
        {
            public readonly Key Progress = KeyFactory.Create( "Progress" );
            public readonly Key Stock = KeyFactory.Create( "Stock" );
            public readonly Key Acquired = KeyFactory.Create( "Acquired" );
        }
     

        public static ProgressDBDBKeys ProgressDB { get { return ProgressDBDBKeys.Instance; } }
        public class ProgressDBDBKeys : Singleton<ProgressDBDBKeys>
        {
            public readonly Key ProgressId = KeyFactory.Create( "ProgressId" );
            public readonly Key ProgressTypeKey = KeyFactory.Create( "ProgressTypeKey" );

            public readonly Key StartTime = KeyFactory.Create( "StartTime" );
            public readonly Key EndTime = KeyFactory.Create( "EndTime" );
        }


        public static ProgressSignalKeys Signal { get { return ProgressSignalKeys.Instance; } }
        public class ProgressSignalKeys : Singleton<ProgressSignalKeys>
        {
            public readonly Key ProgressChanged = KeyFactory.Create( "ProgressChanged" );
        }
    }
}