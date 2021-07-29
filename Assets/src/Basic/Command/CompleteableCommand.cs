using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /** 
     * Basic Command which will signalize complete after Exectute
     */
    public abstract class CompleteableCommand : AbstractCommand
                                              , ICompleteable
    {
        public bool Completed { get; protected set; }

        private Signal m_completedSignal;


        public CompleteableCommand( Key a_id )
          : base( a_id )
        {
            m_completedSignal = new Signal( this );
            addSignal( HAN.Lib.Basic.Keys.Signal.Completed, m_completedSignal );
        }


        protected void complete()
        {
            Completed = true;
            m_completedSignal.Emit();
        }
    }
}