using HAN.Lib.Basic.Collections;
using System;
using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    public class Signal<T> : GenericSignal<Action<T>>
    {
        public Signal( IGenericSignalHandler a_sender ) : base( a_sender ) {
            m_action = ( T a_param ) => genericSlot( a_param );
        }

        private void genericSlot( T a_param )
        {
            foreach( var connection in m_connections ) { 
                connection.Slot( a_param );
            }
        }
    }

    public class Signal<T, T2> : GenericSignal<Action<T, T2>>
    {
        public Signal( IGenericSignalHandler a_sender ) : base( a_sender )
        {
            m_action = ( T a_param, T2 a_param2 ) => genericSlot( a_param, a_param2 );
        }

        private void genericSlot( T a_param, T2 a_param2 )
        {
            foreach( var connection in m_connections ) { 
                connection.Slot( a_param, a_param2 );
            }
        }
    }

    public class Signal<T, T2, T3> : GenericSignal<Action<T, T2, T3>>
    {
        public Signal( IGenericSignalHandler a_sender ) : base( a_sender )
        {
            m_action = ( T a_param, T2 a_param2, T3 a_param3 ) => genericSlot( a_param, a_param2, a_param3 );
        }

        private void genericSlot( T a_param, T2 a_param2, T3 a_param3 )
        {
            foreach( var connection in m_connections ) {
                connection.Slot( a_param, a_param2, a_param3 );
            }
        }
    }

    /**
     * This Signal is used for all basic signals.
     * It stores event delegates in a collection. 
     * GenericSignal has to be extended to support calling the delegates, when Emit is invoked.
     */
    public abstract class GenericSignal<T> : IGenericSignal
                                             where T : Delegate
    {
        public T Emit { get { return m_action; } }

        protected T m_action;

        protected MutableEnumerable<GenericSignalConnection<T>> m_connections = new MutableEnumerable<GenericSignalConnection<T>>();
        protected readonly IGenericSignalHandler m_sender;


        public GenericSignal( IGenericSignalHandler a_sender )
        {
            m_sender = a_sender;
        }


        public IGenericSignalConnection Connect( IGenericSignalHandler a_receiver, T a_action )
        {
            var connection = findConnection( a_receiver, a_action );
            if( connection == null )
            { 
                connection = new GenericSignalConnection<T>( m_sender, a_receiver, this, a_action );
                m_sender.AddConnection( connection );
                a_receiver.AddConnection( connection );

                m_connections.Add( connection );
            }
            else
            {
                Debug.Logger.Warning( m_sender.ToString()
                                    , "Already has Connection to Sender: {0} Receiver: {1}"
                                    , m_sender, a_receiver );
            }

            return connection;
        }


        public void Disconnect( IGenericSignalHandler a_receiver, T a_action )
        {
            var connection = findConnection( a_receiver, a_action );
            if( connection != null )
            {
                m_sender.RemoveConnection( connection );
                a_receiver.RemoveConnection( connection );

                m_connections.Remove( connection );
            }
            else
            {
                Debug.Logger.Error( m_sender.ToString()
                                  , "Cannot find connection for Sender: {0} Receiver: {1}"
                                  , m_sender, a_receiver );
            }
        }


        public void Disconnect( IGenericSignalConnection a_connection )
        {
            if( a_connection is GenericSignalConnection<T> connection )
            {
                connection.Sender.RemoveConnection( connection );
                connection.Receiver.RemoveConnection( connection );

                m_connections.Remove( connection );
            }
        }


        public void DisconnectEstablishedConnections()
        {
            var connections = new List<IGenericSignalConnection>( m_connections );
            foreach( var connection in connections )
            {
                connection.Disconnect();
            }
        }


        private GenericSignalConnection<T> findConnection( IGenericSignalHandler a_receiver, T a_action )
        {
            var connection = m_connections.Find( (GenericSignalConnection<T> c) => {
                if( c.Receiver == a_receiver
                 && c.Slot == a_action )
                {
                    return true;
                }

                return false;
            } );

            return connection;
        }
    }
}