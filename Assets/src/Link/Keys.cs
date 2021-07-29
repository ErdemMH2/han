using HAN.Lib.Structure;

namespace HAN.Lib.LinkSystem
{
    public static class Keys
    {
        public static LinkKeys Link { get { return LinkKeys.Instance; } }
        public class LinkKeys : Singleton<LinkKeys>
        {
            public readonly Key Link     = KeyFactory.Create( "Link" );
            public readonly Key LinkType = KeyFactory.Create( "LinkType" );
        }

        public static LinkTypeKeys Type { get { return LinkTypeKeys.Instance; } }
        public class LinkTypeKeys : Singleton<LinkTypeKeys>
        {
            public readonly Key Successor = KeyFactory.Create( "Successors" );
            public readonly Key Predecessors = KeyFactory.Create( "Predecessors" );
        }
    }
}