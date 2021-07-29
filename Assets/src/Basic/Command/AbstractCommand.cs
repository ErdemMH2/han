using HAN.Lib.Structure;

namespace HAN.Lib.Basic
{
    /** 
     * Basic Command
     * Will implement Id and ExecutionState with default behavior
     */
    public abstract class AbstractCommand : HObject, ICommand
    {
        public static readonly Key k_Command = KeyFactory.Create( "Command" );
        public override MetaType Type() { return new MetaType( k_Command ); }

        public Key Id { get; protected set; }

        public bool CanExecute { get { EvaluateExecute(); return m_canExecute.Value; } }
        protected Property<bool> m_canExecute;

        public IncrementalId State { get { return m_state.Value; } }
        protected Property<IncrementalId> m_state;


        public AbstractCommand( Key a_id )
        {
            Id = a_id;
            m_canExecute = new SignalizingProperty<bool>( Keys.Command.CanExecute, false, this );
            AddProperty( m_canExecute );

            m_state = new SignalizingProperty<IncrementalId>( Keys.Command.ExcutionState, null, this );
            AddProperty( m_state );
        }

        public abstract bool Execute();

        public virtual void EvaluateExecute() { }
    }
}