using HAN.Lib.Structure;
using UnityEngine;


namespace HAN.Lib.Basic.Renderer
{
    public static class ScreenConfig
    {
        public static class Keys
        {
            public static Key Portrait = KeyFactory.Create( "Portrait" );
            public static Key Landscape = KeyFactory.Create( "Landscape" );

            public static Key Touch = KeyFactory.Create( "Touch" );
            public static Key Mouse = KeyFactory.Create( "Mouse" );
        }


        public static Key Configured( Key a_scene )
        {
            return ( a_scene + Orientation() + InputType() );
        }


        public static Key Oriented( Key a_key )
        {
            return ( a_key + Orientation() );
        }


        public static Key Orientation()
        {
            bool landscape = Screen.width > Screen.height;
            if( landscape ) { 
                return Keys.Landscape;
            }
            else { 
                return Keys.Portrait;
            }
        }


        public static Key InputType()
        {
            return Keys.Touch;
        }

    }
}