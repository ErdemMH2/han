using System;
using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    public interface IGenericConnection
    {
        void Disconnect();
    }


    public class GenericConnection<T> : IGenericConnection 
                                        where T : Delegate
    {
        private IGenericSignalHandler m_sender;
        private IGenericSignalHandler m_receiver;

        private GenericSignal<T> m_signal;
        private T m_slot;


        public GenericConnection( IGenericSignalHandler a_sender
                                , IGenericSignalHandler a_receiver
                                , GenericSignal<T> a_signal
                                , T a_slot )
        {
            m_sender = a_sender;
            m_receiver = a_receiver;
            m_signal = a_signal;
            m_slot = a_slot;
        }


        public void Disconnect()
        {
            m_signal.Disconnect( m_sender, m_slot );
        }


        public override bool Equals( object a_otherConnection )
        {
            if( a_otherConnection is GenericConnection<T> connection )
            {
                return connection.m_sender == m_sender
                    && connection.m_receiver == m_receiver
                    && connection.m_signal == m_signal
                    && connection.m_slot == m_slot;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hashCode = 512142358;
            hashCode = hashCode * -1521134295 + EqualityComparer<IGenericSignalHandler>.Default.GetHashCode( m_sender );
            hashCode = hashCode * -1521134295 + EqualityComparer<IGenericSignalHandler>.Default.GetHashCode( m_receiver );
            hashCode = hashCode * -1521134295 + EqualityComparer<GenericSignal<T>>.Default.GetHashCode( m_signal );
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode( m_slot );
            return hashCode;
        }
    }
}