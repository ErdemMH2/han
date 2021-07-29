namespace HAN.Lib.Basic.Unity
{
    /** 
     * Basic Command
     * Will implement Id and ExecutionState with default behavior
     */
    public abstract class AbstractCommandHBehavior : IdHObjectMonoBehavior
                                                   , ICommand
    {
        public override IHANObject HObject => m_command;

        public virtual bool CanExecute { get { return m_command.CanExecute; } }

        public IncrementalId State { get { return m_command.State; } }

        public virtual bool Execute() { return m_command.Execute(); }

        public void EvaluateExecute() { m_command.EvaluateExecute(); }

        protected ICommand m_command {
            get
            {
                if( m_commandInstance == null )
                {
                    m_commandInstance = createCommand();
                }

                return m_commandInstance;
            }
        }
        private ICommand m_commandInstance; 

        protected abstract ICommand createCommand();
    }
}