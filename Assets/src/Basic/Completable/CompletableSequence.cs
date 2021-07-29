using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    public delegate ICompleteable CompletableDelegate();

    /**
     * A sequence of ICompletable will be sequencially processed
     */
    public class CompletableSequence : Completeable
    {
        public override MetaType Type() { return new MetaType( "CompletableSequence" ); }

        private Queue<CompletableDelegate> m_completableDelegates = new Queue<CompletableDelegate>();


        public void AddCompletable( CompletableDelegate a_function )
        {
            m_completableDelegates.Enqueue( a_function );
        }


        public void Execute()
        {
            if( m_completableDelegates.Count <= 0 )
            {
                SignalizeComplete();
            }
            else
            {
                var completableDelegate = m_completableDelegates.Dequeue();
                var completable = completableDelegate();

                if( !completable.Completed )
                {
                    completable.Connect( this, Lib.Basic.Keys.Signal.Completed, 
                                         (BasicSignalParameter) => Execute() );
                }
                else
                {
                    Execute();
                }
            }
        }
    }
}