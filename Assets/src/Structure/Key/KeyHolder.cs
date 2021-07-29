using HAN.Lib.Basic;


namespace HAN.Lib.Structure
{
    /**
     * A key reference in a KeyCollection for usage as MonoBehavior.
     * The KeyHolder is the abstract base class to hold keys. 
     * The specialization will determine how to retrieve the key.
     */
    public abstract class KeyHolder : HObjectMonoBehavior
    {
        public abstract Key Key { get; protected set; }


        public override IHANObject HObject { get { return m_hObject; } }
        private DefaultHObject m_hObject = new DefaultHObject();
    }
}