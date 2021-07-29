using HAN.Lib.Structure;
using System;

namespace HAN.Lib.ProgressSystem
{
    /// Changeable Progress; only ment to be used internally!
    public class WritableProgress : Progress
    {
        public WritableProgress( Key a_id, Key a_type
                               , DateTime a_startTime, DateTime a_endTime )
            : base( a_id, a_type, a_startTime, a_endTime )
        {
        }


        public void Restart( int a_newDurationSeconds )
        {
            Completed = false;
            Success = false;

            m_startTime = DateTime.Now;
            m_endTime = m_startTime.AddSeconds( a_newDurationSeconds );

            Tick(); // recalculate values
        }


        /// Restarts the Progress
        public void Restart()
        {
            Completed = false;
            Success = false;

            var duration = m_endTime - m_startTime;
            m_startTime = DateTime.Now;
            m_endTime = m_startTime.Add( duration );

            Tick(); // recalculate values
        }
    }
}
