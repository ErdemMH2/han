using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * The parameter of System.Action<BasicSignalParameter> propagated on signal emission
     */
    public class BasicSignalParameter : IHANType
    {
        public static readonly Key k_BasicSignalParameter = KeyFactory.Create( "BasicSignalParameter" );

        public ISignalPublisher Sender { get; private set; }

        protected MetaType m_type;
        public virtual MetaType Type() 
        { 
            if( m_type == null ) 
                m_type = new MetaType( k_BasicSignalParameter ); 

            return m_type; 
        }


        public BasicSignalParameter( ISignalPublisher a_sender ) 
        {
            Sender = a_sender;
        }
    }
}