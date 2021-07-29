using HAN.Lib.Basic;
using HAN.Lib.Structure;
using System.Collections.Generic;
using UnityEngine;

namespace HAN.Lib.Extension.Timer
{
    /**
     * Factory to create timers, that are controlled via this's Update loop
     */
    public class HTimerFactory : SingletonMono<HTimerFactory>
    {
        public class Timer : HObject, ICompleteable
        {
            public override MetaType Type() { return new MetaType( KeyFactory.Create( "Timer" ) ); }

            public float ElapsedTime{ get; protected set; }
            public bool Running { get; protected set; }

            public float Interval { get; private set; } = -1f;

            public bool Completed { get; protected set; }

            public bool Repeat { get; private set; } = false;

            protected Signal m_completed;


            public Timer()
            {
                m_completed = new Signal( this );
                addSignal( Lib.Basic.Keys.Signal.Completed, m_completed );
            }


            public void WaitForSeconds( float a_seconds )
            {
                Interval = a_seconds;
            }


            public void Start( bool a_repeat = false)
            {
                Repeat = a_repeat;
                Running = true;
                Completed = false;
            }


            public void Reset()
            {
                ElapsedTime = 0f;
                Running = false;
                Completed = false;
            }
        }


        private class WritableTimer : Timer
        {
            private bool m_resetNextIteration = false;

            public void AddDeltaTime( float a_deltaTime )
            { 
                // we only want the ElapsedTime within one Interval; in repeating timers
                if( m_resetNextIteration ) {
                    m_resetNextIteration = false;
                    ElapsedTime = 0f;
                }

                ElapsedTime += a_deltaTime;

                if( ElapsedTime > Interval )
                {
                    if( !Repeat )
                    { 
                        Completed = true;
                        Running = false;
                    }
                    else
                    {
                        m_resetNextIteration = true;
                    }

                    m_completed.Emit();
                }
            }
        }


        private List<WritableTimer> m_timer = new List<WritableTimer>();


        public Timer Create()
        {
            WritableTimer timer = new WritableTimer();
            m_timer.Add( timer );
            return timer;
        }
            

        private void Update()
        {
            // collection can be changed through slots
            var timers = new List<WritableTimer>( m_timer );
            foreach( var timer in timers )
            {
                if( timer.Running )
                { 
                    timer.AddDeltaTime( Time.deltaTime );
                }
            }
        }
    }
}