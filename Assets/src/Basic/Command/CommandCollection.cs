using HAN.Lib.Structure;
using System.Collections.Generic;

namespace HAN.Lib.Basic
{
    public class CommandCollection : CompleteableCommand
    {
        private List<ICommand> m_commands = new List<ICommand>();
        

        public CommandCollection( Key a_id ) 
          : base( a_id )
        {
            m_canExecute.SetValue( true );
        }


        public CommandCollection( Key a_id, IEnumerable<ICommand> a_commands )
          : base( a_id )
        {
            m_canExecute.SetValue( true );

            foreach( var command in a_commands )
            {
                Push( command );
            }
        }


        public void Push( ICommand a_command )
        {
            m_commands.Add( a_command );
            if( a_command is ICompleteable completable )
            {
                a_command.Connect( this, Lib.Basic.Keys.Signal.Completed
                                 , (BasicSignalParameter) => evaluateCompleted() );
            }

            EvaluateExecute();
        }


        public override void EvaluateExecute() 
        {
            bool canExecute = true;
            foreach( var command in m_commands )
            {
                canExecute &= command.CanExecute;
            }

            m_canExecute.SetValue( canExecute );
        }


        public override bool Execute()
        {
            bool executed = true;
            foreach( var command in m_commands )
            {
                executed &= command.Execute();
            }

            evaluateCompleted();

            return executed;
        }


        private void evaluateCompleted()
        {
            bool completed = true;
            foreach( var command in m_commands )
            {
                if( command is ICompleteable completable )
                {
                    if( !completable.Completed )
                    {
                        completed = false;
                    }
                }
            }

            if( completed )  {
                complete();
            }
        }

    }
}