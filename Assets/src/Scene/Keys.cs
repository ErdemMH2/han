using HAN.Lib.Structure;


namespace HAN.Lib.Scene
{
    public static class Keys
    {
        /**
         * Often used Signals (static) Keys
         */
        public static SceneKeys Scene { get { return SceneKeys.Instance; } }
        public class SceneKeys : Singleton<SceneKeys>
        {
            public readonly Key Popped = KeyFactory.Create( "Popped" );
        }
    }
}