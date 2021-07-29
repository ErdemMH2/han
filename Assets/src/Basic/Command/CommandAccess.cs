using HAN.Lib.Structure;
using System.Collections.Generic;


namespace HAN.Lib.Basic
{
    /** 
     * Basic Command access
     * will organize the Commands in a Dictionary
     */
    public class CommandAccess : ICommandAccess
    {
        protected Dictionary<Key, ICommand> m_commands = new Dictionary<Key, ICommand>();

        public bool HasCommand( Key a_key ) { return m_commands.ContainsKey( a_key ); }
        public ICommand Command( Key a_key ) { ICommand command = null; m_commands.TryGetValue( a_key, out command ); return command; }

        public bool ExecuteCommand( Key a_key ) { ICommand command = Command( a_key ); if( command != null ) return command.Execute(); else return false; }

        public bool AddCommand( ICommand a_command ) { if( m_commands.ContainsKey( a_command.Id ) ) return false; m_commands.Add( a_command.Id, a_command ); return true; }
        public bool RemoveCommand( Key a_key ) { return m_commands.Remove( a_key ); }
    }

}