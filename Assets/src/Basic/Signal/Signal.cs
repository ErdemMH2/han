using HAN.Lib.Structure;
using System;

namespace HAN.Lib.Basic
{
    /**
     * This Signal is used for extendable parameters. 
     * The BasicSignalParameter can be derived to provide a generic api 
     * for connection and slots.
     */
    public class Signal : Signal<BasicSignalParameter>, ISignalPublisher
    {
        public static readonly Key k_Signal = KeyFactory.Create( "Signal" );
        public MetaType Type() { return new MetaType( k_Signal, m_ownKey ); }

        protected ISignalHandler m_parent;

        private BasicSignalParameter m_param; // default parameter, will never change so reuse

        private Key m_ownKey;


        /**
         * Creates a signal owned by the parent a_parent.
         */
        public Signal( ISignalHandler a_parent ) 
          : base( a_parent )
        {
            m_parent = a_parent;
            m_param = new BasicSignalParameter( m_parent );
        }


        /**
         * Will emit the signal without any special parameters.
         */
        public new void Emit()
        {
            Emit( m_param );
        }


        public new void Emit( BasicSignalParameter a_param )
        {   
            base.Emit( a_param );
        }


        /**
         * Connect the slot to this signal.
         */
        public bool Connect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot )
        {
            m_ownKey = a_signal; // TODO: move Key into constructor!
            Connect( a_subscriber, a_slot );
            return true;
        }


        /**
         * Disconnect the slot from this signal. 
         */
        public bool Disconnect( ISignalSubscriber a_subscriber, Key a_signal, Action<BasicSignalParameter> a_slot )
        {
            Disconnect( a_subscriber, a_slot );
            return true;
        }


        public override string ToString()
        {
            if( m_ownKey != null ) {
                return m_ownKey.ToString();
            }
            else {
                return base.ToString();
            }
        }
    }
}