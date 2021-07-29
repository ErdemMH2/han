using System;

namespace HAN.Lib.Basic
{
    public class SignalDelayer<T, T2, T3> : SignalDelayer
    {
        private T m_delayedParam;
        private T2 m_delayedParam2;
        private T3 m_delayedParam3;
        private GenericSignal<Action<T, T2, T3>> m_signal;

        public SignalDelayer( GenericSignal<Action<T, T2, T3>> a_signal, bool a_clearOnEmit )
            : base( a_clearOnEmit ) { m_signal = a_signal; }

        public void Delay( T a_param, T2 a_param2, T3 a_param3 )
        {
            m_delayedParam = a_param;
            m_delayedParam2 = a_param2;
            m_delayedParam3 = a_param3;
        }

        public override void Emit()
        {
            m_signal.Emit( m_delayedParam, m_delayedParam2, m_delayedParam3 );
            base.Emit();
        }

        public override void Clear()
        {
            m_delayedParam = default;
            m_delayedParam2 = default;
            m_delayedParam3 = default;
        }
    }


    public class SignalDelayer<T, T2> : SignalDelayer
    {
        private T m_delayedParam;
        private T2 m_delayedParam2;
        private GenericSignal<Action<T, T2>> m_signal;

        public SignalDelayer( GenericSignal<Action<T, T2>> a_signal, bool a_clearOnEmit )
            : base( a_clearOnEmit ) { m_signal = a_signal; }

        public void Delay( T a_param, T2 a_param2 )
        {
            m_delayedParam = a_param;
            m_delayedParam2 = a_param2;
        }

        public override void Emit()
        {
            m_signal.Emit( m_delayedParam, m_delayedParam2 );
            base.Emit();
        }

        public override void Clear()
        {
            m_delayedParam = default;
            m_delayedParam2 = default;
        }
    }


    public class SignalDelayer<T> : SignalDelayer
    {
        private T m_delayedParam;
        private GenericSignal<Action<T>> m_signal;

        public SignalDelayer( GenericSignal<Action<T>> a_signal, bool a_clearOnEmit ) 
            : base( a_clearOnEmit ) { m_signal = a_signal; }

        public void Delay( T a_param )
        {
            m_delayedParam = a_param;
        }

        public override void Emit()
        {
            m_signal.Emit( m_delayedParam );
            base.Emit();
        }

        public override void Clear()
        {
            m_delayedParam = default;
        }
    }


    public abstract class SignalDelayer
    {
        protected readonly bool m_clearOnEmit;
        public SignalDelayer( bool a_clearOnEmit ) { m_clearOnEmit = a_clearOnEmit; }
        public virtual void Emit()
        {
            if( m_clearOnEmit )
            {
                Clear();
            }
        }
        public abstract void Clear();
    }
}