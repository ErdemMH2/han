using HAN.Lib.Structure;


namespace HAN.Lib.Basic
{
    public static class GamePadKeys
    {
        public static InputKeys Input { get { return InputKeys.Instance; } }
        public class InputKeys : Singleton<InputKeys>
        {
            public readonly Key XButton         = KeyFactory.Create( "X_Button" );
            public readonly Key CircleButton    = KeyFactory.Create( "Circle_Button" );
            public readonly Key R1Button        = KeyFactory.Create( "R1Button" );
            public readonly Key L1Button        = KeyFactory.Create( "L1Button" );
            public readonly Key RightArrow      = KeyFactory.Create( "RightArrow" );
            public readonly Key LeftArrow       = KeyFactory.Create( "LeftArrow" );
            public readonly Key UpArrow         = KeyFactory.Create( "UpArrow" );
            public readonly Key DownArrow       = KeyFactory.Create( "DownArrow" );
        }

        public static SignalKeys Signal { get { return SignalKeys.Instance; } }
        public class SignalKeys : Singleton<SignalKeys>
        {
            public readonly Key Clicked     = KeyFactory.Create( "Clicked" );
            public readonly Key Pressed     = KeyFactory.Create( "Pressed" );
            public readonly Key Released    = KeyFactory.Create( "Released" );
        }
    }
}
