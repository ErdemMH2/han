using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    /**
     * Implements ICompletable
     * Can be Completed without sending a signal
     * or can be Completed before connecting to signal
     * The Completed state have to be checked before connecting to the signal
     */
    public class Completeable : HObject, ICompleteable
    {
        public override MetaType Type() { return new MetaType( KeyFactory.Create( "Completeable" ) ); }

        public bool Completed { get; private set; } = false;
        private bool m_internalCompleted = false;

        protected List<ICompleteable> m_waitConditions = new List<ICompleteable>();
        protected Signal m_completedSignal;


        public Completeable()
        {
            m_completedSignal = new Signal( this );
            addSignal( Keys.Signal.Completed, m_completedSignal );
        }


        /// Will wait for another ICompletable, before signalizing own completion
        public void WaitFor( ICompleteable a_condition )
        {
            if( !a_condition.Completed )
            {
                m_waitConditions.Add( a_condition );
                a_condition.Connect( this, Lib.Basic.Keys.Signal.Completed, 
                                    ( BasicSignalParameter ) => { completed( a_condition ); } );
            }
        }

        /// Will mark this as complete and check sub ICompletable, if awailable
        public virtual void SignalizeComplete()
        {
            m_internalCompleted = true;
            evaluateComplete();
        }


        private void evaluateComplete()
        {
            foreach( var completable in m_waitConditions )
            {
                if( !completable.Completed )
                {
                    return;
                }
            }

            if( m_internalCompleted )
            { 
                Completed = true;
                emitComplete();
            }
        }


        private void completed( ICompleteable a_condition )
        {
            evaluateComplete();
        }


        protected virtual void emitComplete()
        {
            m_completedSignal.Emit();
        }
    }
}