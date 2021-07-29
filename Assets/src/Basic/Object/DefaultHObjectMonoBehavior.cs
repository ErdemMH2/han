using HAN.Lib.Structure;

namespace HAN.Lib.Basic.Unity
{
    public class DefaultHObjectMonoBehavior : HObjectMonoBehavior
    {
        public override MetaType Type() { return new MetaType( KeyFactory.Create( "DefaultHObjectMonoBehavior" ) ); }


        public override IHANObject HObject => m_hObject;
        protected DefaultHObject m_hObject = new DefaultHObject();
    }
}