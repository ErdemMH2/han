
namespace HAN.Lib.Basic
{
    /**
     * Can connect and disconnet to GenericSignals.
     * Will automatically disconnect on destruction.
     */
    public class GenericSignalHandler : IGenericSignalHandler
                                      , ISignalDisconnectable
    {
        protected GenericConnectionBookkeeper m_connectionBookkeeper;

        ~GenericSignalHandler()
        {
            m_connectionBookkeeper?.DisconnectEstablishedConnections();
        }


        public virtual void AddConnection( IGenericSignalConnection a_connection )
        {
            if( m_connectionBookkeeper == null ) {
                m_connectionBookkeeper = new GenericConnectionBookkeeper();
            }

            m_connectionBookkeeper.AddConnection( a_connection );
        }


        public virtual void RemoveConnection( IGenericSignalConnection a_connection )
        {
            m_connectionBookkeeper?.RemoveConnection( a_connection );
        }


        /**
         *  We dont want to send signals at all anymore. 
         *  All subscribers are will be notified, 
         *  that we dont want to send any signals anymore. 
         */
        public virtual void DisconnectEstablishedConnections()
        {
            m_connectionBookkeeper?.DisconnectEstablishedConnections();
        }
    }
}