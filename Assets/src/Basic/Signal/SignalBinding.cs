using HAN.Lib.Structure;
using System;

namespace HAN.Lib.Basic
{
    /**
     * Will forward a signal
     * When Connect is called the owner is connected to the publisher 
     * Depends on the owner/publisher relationship. 
     * If one goes out of scope the connection will not be valid anymore.
     * Can be reused by calling Connect again
     */
    public class SignalBinding : Signal
    {
        private IHANObject m_owner;
        private Action<BasicSignalParameter> m_slot;
        private Key m_id;


        public SignalBinding( IHANObject a_owner
                            , Key a_id )
          : base( a_owner )
        {
            m_owner = a_owner;
            m_id = a_id;
        }

        public SignalBinding( IHANObject a_owner
                            , Key a_id, Action<BasicSignalParameter> a_slot )
          : this( a_owner, a_id )
        {
            m_slot = a_slot;
        }

        public void Connect( ISignalPublisher a_publisher )
        {
            a_publisher.Connect( m_owner, m_id, onSignalReceived );
        }

        private void onSignalReceived( BasicSignalParameter a_param )
        {
            m_slot?.Invoke( a_param );
            Emit( a_param );
        }
    }
}