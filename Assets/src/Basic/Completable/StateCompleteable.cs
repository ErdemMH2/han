using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /**
     * A completable that includes a state, that is defined by a Key
     * Default states are: Successful, Failed
     */
    public class StateCompleteable : Completeable
    {
        public Key State { get; private set; }
        protected BasicSignalParameter m_param = null;  // default is no param


        public StateCompleteable()
        {
            State = Keys.CompleteableState.Unknown; // default state
        }


        /// Signalize complete by copying the a_state from another StateCompleteable
        public void SignalizeComplete( StateCompleteable a_state )
        {
            State = a_state.State;
            m_param = a_state.m_param;

            SignalizeComplete();
        }


        public void SignalizeComplete( Key a_state
                                     , BasicSignalParameter a_param = null )
        {
            State = a_state;
            m_param = a_param;

            SignalizeComplete();
        }


        /// Signalize complete with Successful, when no State was defined before
        public override void SignalizeComplete()
        {
            if( State == Keys.CompleteableState.Unknown ) { 
                State = Keys.CompleteableState.Succeeded;
            }

            base.SignalizeComplete();
        }


        public void SignalizeFail()
        {
            State = Keys.CompleteableState.Failed;
            base.SignalizeComplete();
        }


        protected override void emitComplete()
        {
            // find resulting state
            foreach( var comp in m_waitConditions )
            {
                if( comp is StateCompleteable stateComp )
                {
                    // inconsitent state = failed
                    // Fail + Success = Fail
                    if( stateComp.State != State ) {
                        State = Keys.CompleteableState.Failed;
                        break;
                    }
                }
            }

            m_completedSignal.Emit( m_param );
        }
    }
}