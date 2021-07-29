using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    /**
     * Compresses multiple Signal's
     * will be emited  when all Signal's were received.
     */
    public class SignalCompressor : HObject, ICompleteable
    {
        public override MetaType Type() {  return new MetaType( "SignalCompressor" ); }

        public virtual bool Completed { get; private set; } = false;
        protected Signal m_completedSignal;

        private Key m_signalKey;
        protected IReadOnlyList<ISignalPublisher> m_signalOwner;
        private Dictionary<ISignalPublisher, bool> m_completedSignals = new Dictionary<ISignalPublisher, bool>();


        public SignalCompressor( Key a_signalKey, IEnumerable<ISignalPublisher> a_signalOwner )
        {
            m_signalKey = a_signalKey;
            m_completedSignal = new Signal( this );
            addSignal( Keys.Signal.Completed, m_completedSignal );

            m_signalOwner = new List<ISignalPublisher>( a_signalOwner );

            foreach( var owner in m_signalOwner )
            {
                owner.Connect( this, m_signalKey, ( BasicSignalParameter ) => { completed( owner ); } );
            }
        }


        protected virtual void completed( ISignalPublisher a_signalOwner )
        {
            if( !m_completedSignals.ContainsKey( a_signalOwner ) )
            {
                m_completedSignals.Add( a_signalOwner, true );
            }

            if( m_signalOwner.Count == m_completedSignals.Count )
            {
                m_completedSignals.Clear();

                Completed = true;
                m_completedSignal.Emit();
                DisconnectEstablishedConnections();
            }
        }
    }
}