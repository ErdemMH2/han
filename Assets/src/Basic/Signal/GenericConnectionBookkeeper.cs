using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    /**
     * Manages IGenericSignalConnection's
     * Adds, Removes and finds related connections
     */
    public class GenericConnectionBookkeeper : ISignalDisconnectable
                                             , IGenericSignalHandler
    {
        public IReadOnlyList<IGenericSignalConnection> Connections { get { return m_connections; } }
        private List<IGenericSignalConnection> m_connections = new List<IGenericSignalConnection>();


        public GenericConnectionBookkeeper()
        {
        }


        public void AddConnection( IGenericSignalConnection a_connection )
        {
            m_connections.Add( a_connection );
        }


        public void RemoveConnection( IGenericSignalConnection a_connection )
        {
            m_connections.Remove( a_connection );
        }


        public void DisconnectEstablishedConnections()
        {
            var connections = new List<IGenericSignalConnection>( m_connections );
            foreach( var connection in connections )
            {
                connection.Disconnect();
            }
        }


        public void DisconnectFromSender( IGenericSignalHandler a_sender )
        {
            var connections = FindWhereSender( a_sender );
            foreach( var connection in connections )
            {
                connection.Disconnect();
            }
        }


        public IEnumerable<IGenericSignalConnection> FindWhereSender( IGenericSignalHandler a_sender )
        {
            return m_connections.FindAll( c => c.Sender == a_sender );
        }


        public IEnumerable<IGenericSignalConnection> FindWhereSignal( IGenericSignal a_sender )
        {
            return m_connections.FindAll( c => c.Signal == a_sender );
        }

        public IEnumerable<IGenericSignalConnection> FindWhereReceiver( IGenericSignalHandler a_receiver )
        {
            return m_connections.FindAll( c => c.Receiver == a_receiver );
        }
    }
}