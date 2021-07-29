using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System;

namespace HAN.Lib.ProgressSystem
{
    /// A Progress represents a completable, that will be progressed by time
    /// It will be complete if the target time has been reached
    public class Progress : HObject
                          , ICompleteable
    {
        public override MetaType Type() { return new MetaType( "Progress" ); }

        public Key Id { get; private set; }
        public Key TypeKey { get; private set; }

        public bool Success { get; protected set; }
        public bool Completed { get; protected set; }

        public int Percentage { get { return (int) (ValueNormalized * 100f); } }
        public float ValueNormalized { get; private set; }

        public TimeSpan TimeLeft { get; private set; }

        public DateTime StartTime { get { return m_startTime;  } }
        protected DateTime m_startTime;
        public DateTime EndTime { get { return m_endTime; } }
        protected DateTime m_endTime;

        private Signal m_completedSignal;


        public Progress( Key a_id, Key a_type
                       , DateTime a_startTime, DateTime a_endTime )
        {
            Id = a_id;
            TypeKey = a_type;

            m_startTime = a_startTime;
            m_endTime = a_endTime;

            m_completedSignal = new Signal( this );
            addSignal( HAN.Lib.Basic.Keys.Signal.Completed, m_completedSignal );
        }


        /// Cancels the Progress and emits complete
        public void Cancel()
        {
            Success = false;
            Completed = true;

            m_completedSignal.Emit();
        }


        /// Ends succeeds and ends Progress 
        public void Succeed()
        {
            Success = true;
            Completed = true;

            m_completedSignal.Emit();
        }
        

        /// Will recalculate all values with the current system time
        public void Tick()
        {
            if( !Completed )
            { 
                var wholeTimeDiff = m_endTime - m_startTime;
                var elapsedTime = DateTime.Now - m_startTime;
                TimeLeft = m_endTime - DateTime.Now;
                ValueNormalized = (float) elapsedTime.Ticks / (float) wholeTimeDiff.Ticks;

                if( ValueNormalized >= 1.0f )
                {
                    // cap the value to 1
                    ValueNormalized = 1.0f;

                    Success = true;
                    Completed = true;

                    m_completedSignal.Emit();
                }
            }
        }
    }
}