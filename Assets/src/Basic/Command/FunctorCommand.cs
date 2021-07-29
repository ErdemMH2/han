using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    public class FunctorCommand : AbstractCommand
    {
        public delegate bool Functor();

        private Functor m_executeFunctor;
        private Functor m_canExecuteFunctor;
        

        public FunctorCommand( Key a_id
                             , Functor a_executeFunctor
                             , Functor a_canExecuteFunctor = null ) 
          : base( a_id )
        {
            m_executeFunctor = a_executeFunctor;
            m_canExecuteFunctor = a_canExecuteFunctor;
        }


        public override bool Execute()
        {
            return m_executeFunctor();
        }


        public override void EvaluateExecute() 
        {
            if( m_canExecuteFunctor != null ) 
            { 
                m_canExecute.SetValue( m_canExecuteFunctor() );
            }
            else
            {
                m_canExecute.SetValue( true );
            }
        }
    }
}